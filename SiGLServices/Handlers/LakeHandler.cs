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
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using WiM.Security;

namespace SiGLServices.Handlers
{
    public class LakeHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<lake_type> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<lake_type>().OrderBy(e => e.lake).ToList();

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
            lake_type anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<lake_type>().FirstOrDefault(e => e.lake_type_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteLake")]
        public OperationResult GetSiteLake(Int32 siteId)
        {
            lake_type anEntity = null;

            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<site>().Include(s => s.lake_type).FirstOrDefault(e => e.site_id == siteId).lake_type;
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
       
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole})]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(lake_type anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.lake))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<lake_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        #endregion

        #region PutMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole })]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, lake_type anEntity)
        {
            try
            {
                if (entityId <= 0)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<lake_type>(entityId, anEntity);
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
            lake_type anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<lake_type>().FirstOrDefault(i => i.lake_type_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<lake_type>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
              
        #endregion       
    }
}