//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2016 WiM - USGS

//    authors:  Jeremy K. Newson USGS Wisconsin Internet Mapping
//              Tonia Roddick USGS Wisconsin Internet Mapping
//  
//   purpose:   Handles Site resources through the HTTP uniform interface.
//              Equivalent to the controller in MVC.
//
//discussion:   Handlers are objects which handle all interaction with resources. 
//              https://github.com/openrasta/openrasta/wiki/Handlers
//
//     

#region Comments
// 07.07.16 - TR - Created
#endregion

using OpenRasta.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using SiGLServices.Utilities.ServiceAgent;
using SiGLServices.Security;
using SiGLServices.Resources;
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using WiM.Security;

namespace SiGLServices.Handlers
{
    public class ParameterHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<parameter_type> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<parameter_type>().OrderBy(e => e.parameter_type_id).ToList();

                    sm(MessageType.info, "Count: " + entities.Count());
                    sm(sa.Messages);

                }//end using
                return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }//end HttpMethod.GET

        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get(Int32 entityId)
        {
            parameter_type anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<parameter_type>().FirstOrDefault(e => e.parameter_type_id == entityId);
                    if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();
                    sm(sa.Messages);

                }//end using
             
                return new OperationResult.OK { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }//end HttpMethod.GET

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteParameters")]
        public OperationResult GetSiteParameters(Int32 siteId)
        {
            List<parameter_type> entities = null;

            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<site_parameters>().Where(sf => sf.site_id == siteId).Select(ft => ft.parameter_type).ToList();
                    sm(MessageType.info, "Count: " + entities.Count());
                    sm(sa.Messages);
                }//end using
                return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }//end HttpMethod.GET

        [HttpOperation(HttpMethod.GET, ForUriName = "GetParametersByGroupName")]
        public OperationResult GetParametersByGroupName(string groupNames)
        {
            IQueryable<parameter_type> query = null;
            ParameterGroups entities = null;
            List<string> groups = null;

            try
            {
                if (string.IsNullOrEmpty(groupNames))
                    groupNames = "Physical, Chemical, Biological, Microbiological, Toxicological";

                char[] delimiter = { ',' };
                groups = groupNames.Split(delimiter, StringSplitOptions.RemoveEmptyEntries).AsEnumerable().Select(p => p.Trim()).ToList();

                using (SiGLAgent sa = new SiGLAgent())
                {
                    query = sa.Select<parameter_type>();
                    entities = new ParameterGroups();
                    int eCount = 0;
                    if (groups.Contains("Physical"))
                    {
                        entities.Physical = query.Where(a => a.parameter_group.Equals("Physical", StringComparison.OrdinalIgnoreCase)).ToList(); eCount += entities.Physical.Count();
                    }

                    if (groups.Contains("Chemical"))
                    {
                        entities.Chemical = query.Where(a => a.parameter_group.Equals("Chemical", StringComparison.OrdinalIgnoreCase)).ToList(); eCount += entities.Chemical.Count();
                    }
                    if (groups.Contains("Biological"))
                    {
                        entities.Biological = query.Where(a => a.parameter_group.Equals("Biological", StringComparison.OrdinalIgnoreCase)).ToList(); eCount += entities.Biological.Count();
                    }
                    if (groups.Contains("Microbiological"))
                    {
                        entities.Microbiological = query.Where(a => a.parameter_group.Equals("Microbiological", StringComparison.OrdinalIgnoreCase)).ToList(); eCount += entities.Microbiological.Count();
                    }
                    if (groups.Contains("Toxicological"))
                    {
                        entities.Toxicological = query.Where(a => a.parameter_group.Equals("Toxicological", StringComparison.OrdinalIgnoreCase)).ToList(); eCount += entities.Toxicological.Count();
                    }

                    sm(MessageType.info, "Count: " +  eCount);
                    sm(sa.Messages);

                }//end using
                return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(parameter_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.parameter) || string.IsNullOrEmpty(anEntity.parameter_group))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<parameter_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        //posts relationship, then returns list of frequency_type for the site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddSiteParameter")]
        public OperationResult AddSiteParameter(Int32 siteId, Int32 parameterTypeId)
        {
            site_parameters anEntity = null;
            List<parameter_type> parameterTypeList = null;
            try
            {
                if (siteId <= 0 || parameterTypeId <= 0) throw new BadRequestException("Invalid input parameters");
                
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<parameter_type>().First(n => n.parameter_type_id == parameterTypeId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<site_parameters>().FirstOrDefault(nt => nt.parameter_type_id == parameterTypeId && nt.site_id == siteId) == null)
                        {
                            anEntity = new site_parameters();
                            anEntity.site_id = siteId;
                            anEntity.parameter_type_id = parameterTypeId;
                            anEntity = sa.Add<site_parameters>(anEntity);

                            project aProj = sa.Select<project>().First(p => p.project_id == aSite.project_id);
                            aProj.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(aProj);
                            sm(sa.Messages);
                        }
                        //return list of freq types
                        parameterTypeList = sa.Select<parameter_type>().Where(nn => nn.site_parameters.Any(nns => nns.site_id == siteId)).ToList();
                        
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = parameterTypeList, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }

        //posts relationship, then returns list of frequency_type for the site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddSiteParameterList")]
        public OperationResult AddSiteParameterList(Int32 siteId, List<parameter_type> entities)
        {
            try
            {
                if (siteId <= 0 || entities.Count <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        sa.context.Configuration.AutoDetectChangesEnabled = false;

                        Int32 projId = sa.Select<site>().AsNoTracking().FirstOrDefault(s => s.site_id == siteId).project_id.Value;

                        if (projId <= 0)
                            throw new NotFoundRequestException();
                        //IQueryable<parameter_type> query = null;
                        //query = sa.Select<parameter_type>().AsNoTracking();

                        foreach (parameter_type p in entities)
                        {
                            //if (query.First(n => n.parameter_type_id == p.parameter_type_id) == null)
                            //    throw new NotFoundRequestException();

                            if (sa.Select<site_parameters>().AsNoTracking().FirstOrDefault(nt => nt.parameter_type_id == p.parameter_type_id && nt.site_id == siteId) == null)
                            {
                                site_parameters anEntity = new site_parameters(); 
                                
                                anEntity.site_id = siteId;
                                anEntity.parameter_type_id = p.parameter_type_id;
                                sa.Add<site_parameters>(anEntity);                                
                            }
                        }//end foreach
                        project aProj = sa.Select<project>().First(p => p.project_id == projId);
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);
                        sm(sa.Messages);
                        //return list of freq types
                       // parameterTypeList = sa.Select<parameter_type>().Where(nn => nn.site_parameters.Any(nns => nns.site_id == siteId)).ToList();

                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }
        #endregion

        #region PutMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, parameter_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.parameter) || string.IsNullOrEmpty(anEntity.parameter_group))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<parameter_type>(entityId, anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using

                return new OperationResult.Modified { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.PUT

        #endregion

        #region DeleteMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(AdminRole)]
        [HttpOperation(HttpMethod.DELETE)]
        public OperationResult Delete(Int32 entityId)
        {
            parameter_type anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<parameter_type>().FirstOrDefault(i => i.parameter_type_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<parameter_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
        
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveSiteParameter")]
        public OperationResult RemoveSiteParameter(Int32 siteId, Int32 parameterTypeId)
        {
            try
            {
                if (siteId <= 0 || parameterTypeId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        site_parameters ObjectToBeDeleted = sa.Select<site_parameters>().SingleOrDefault(nns => nns.site_id == siteId && nns.parameter_type_id == parameterTypeId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<site_parameters>(ObjectToBeDeleted);

                        project aProj = sa.Select<project>().First(p => p.project_id == aSite.project_id);
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }

        #endregion       
    }
}