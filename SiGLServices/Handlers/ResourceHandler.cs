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
// 07.11.16 - TR - Created
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
    public class ResourceHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<resource_type> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<resource_type>().OrderBy(e => e.resource_type_id).ToList();

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
            resource_type anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<resource_type>().FirstOrDefault(e => e.resource_type_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteResources")]
        public OperationResult GetSiteLake(Int32 siteId)
        {
            List<resource_type> entities = null;

            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<site_resource>().Where(sf => sf.site_id == siteId).Select(ft => ft.resource_type).ToList();
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
        [SiGLRequiresRole(new string[] { AdminRole})]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(resource_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.resource_name))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<resource_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        //posts relationship, then returns list of resource_type for the site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddSiteResource")]
        public OperationResult AddSiteResource(Int32 siteId, Int32 resourceTypeId)
        {
            site_resource anEntity = null;
            List<resource_type> resTypeList = null;
            try
            {
                if (siteId <= 0 || resourceTypeId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<resource_type>().First(n => n.resource_type_id == resourceTypeId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<site_resource>().FirstOrDefault(nt => nt.resource_type_id == resourceTypeId && nt.site_id == siteId) == null)
                        {
                            anEntity = new site_resource();
                            anEntity.site_id = siteId;
                            anEntity.resource_type_id = resourceTypeId;
                            anEntity = sa.Add<site_resource>(anEntity);

                            project aProj = sa.Select<project>().First(p => p.project_id == aSite.project_id);
                            aProj.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(aProj);
                            sm(sa.Messages);
                        }
                        //return list of res types
                        resTypeList = sa.Select<resource_type>().Where(nn => nn.site_resource.Any(nns => nns.site_id == siteId)).OrderBy(nn=>nn.resource_name).ToList();

                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = resTypeList, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }
        #endregion

        #region PutMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole })]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, resource_type anEntity)
        {
            try
            {
                if (entityId <= 0)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<resource_type>(entityId, anEntity);
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
            resource_type anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<resource_type>().FirstOrDefault(i => i.resource_type_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<resource_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "removeSiteResource")]
        public OperationResult removeSiteResource(Int32 siteId, Int32 resourceTypeId)
        {
            try
            {
                if (siteId <= 0 || resourceTypeId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        site aSite = sa.Select<site>().First(s => s.site_id == siteId);
                        if (aSite == null)
                            throw new NotFoundRequestException();

                        site_resource ObjectToBeDeleted = sa.Select<site_resource>().SingleOrDefault(nns => nns.site_id == siteId && nns.resource_type_id == resourceTypeId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<site_resource>(ObjectToBeDeleted);

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