﻿//------------------------------------------------------------------------------

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
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using WiM.Security;

namespace SiGLServices.Handlers
{
    public class FrequencyHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<frequency_type> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<frequency_type>().OrderBy(e => e.frequency).ToList();

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
            frequency_type anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<frequency_type>().FirstOrDefault(e => e.frequency_type_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteFrequencies")]
        public OperationResult GetSiteFrequencies(Int32 siteId)
        {
            List<frequency_type> entities = null;

            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<site_frequency>().Where(sf => sf.site_id == siteId).Select(ft => ft.frequency_type).ToList();
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
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(frequency_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.frequency))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<frequency_type>(anEntity);
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
        [HttpOperation(HttpMethod.POST, ForUriName = "AddSiteFrequency")]
        public OperationResult AddSiteFrequency(Int32 siteId, Int32 frequencyTypeId)
        {
            site_frequency anEntity = null;
            List<frequency_type> freqTypeList = null;
            try
            {
                if (siteId <= 0 || frequencyTypeId <= 0) throw new BadRequestException("Invalid input parameters");
                
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<frequency_type>().First(n => n.frequency_type_id == frequencyTypeId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<site_frequency>().FirstOrDefault(nt => nt.frequency_type_id == frequencyTypeId && nt.site_id == siteId) == null)
                        {
                            anEntity = new site_frequency();
                            anEntity.site_id = siteId;
                            anEntity.frequency_type_id = frequencyTypeId;
                            anEntity = sa.Add<site_frequency>(anEntity);

                            project aProj = sa.Select<project>().First(p => p.project_id == aSite.project_id);
                            aProj.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(aProj);
                            sm(sa.Messages);
                        }
                        //return list of freq types
                        freqTypeList = sa.Select<frequency_type>().Where(nn => nn.site_frequency.Any(nns => nns.site_id == siteId)).OrderBy(nn=>nn.frequency).ToList();
                        
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = freqTypeList, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddSiteFrequencyList")]
        public OperationResult AddSiteFrequencyList(Int32 siteId, List<frequency_type> entities)
        {
            //site_frequency anEntity = null;
         //   List<frequency_type> freqTypeList = null;
            try
            {
                if (siteId <= 0 || entities.Count <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        sa.context.Configuration.AutoDetectChangesEnabled = false;
                        //site aSite = sa.Select<site>().AsNoTracking().First(s => s.site_id == siteId);
                        Int32 projId = sa.Select<site>().AsNoTracking().FirstOrDefault(s => s.site_id == siteId).project_id.Value;

                        if (projId <= 0)
                            throw new NotFoundRequestException();
                        IQueryable<frequency_type> query = null;
                        query = sa.Select<frequency_type>().AsNoTracking();

                        foreach(frequency_type f in entities)
                        {
                            if (query.First(n => n.frequency_type_id == f.frequency_type_id) == null)
                                throw new NotFoundRequestException();

                            if (sa.Select<site_frequency>().AsNoTracking().FirstOrDefault(nt => nt.frequency_type_id == f.frequency_type_id && nt.site_id == siteId) == null)
                            {
                                site_frequency anEntity = new site_frequency();
                                anEntity.site_id = siteId;
                                anEntity.frequency_type_id = f.frequency_type_id;
                                anEntity = sa.Add<site_frequency>(anEntity);
                            }
                        }//end foreach

                        project aProj = sa.Select<project>().First(p => p.project_id == projId);
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);
                        sm(sa.Messages);
                        
                        //return list of freq types
                     //   freqTypeList = sa.Select<frequency_type>().Where(nn => nn.site_frequency.Any(nns => nns.site_id == siteId)).OrderBy(nn => nn.frequency).ToList();

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
        public OperationResult Put(Int32 entityId, frequency_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.frequency))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {                        
                        anEntity = sa.Update<frequency_type>(entityId, anEntity);
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
            frequency_type anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<frequency_type>().FirstOrDefault(i => i.frequency_type_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<frequency_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
        
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveSiteFrequency")]
        public OperationResult RemoveSiteFrequency(Int32 siteId, Int32 frequencyTypeId)
        {
            try
            {
                if (siteId <= 0 || frequencyTypeId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        site_frequency ObjectToBeDeleted = sa.Select<site_frequency>().SingleOrDefault(nns => nns.site_id == siteId && nns.frequency_type_id == frequencyTypeId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<site_frequency>(ObjectToBeDeleted);
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