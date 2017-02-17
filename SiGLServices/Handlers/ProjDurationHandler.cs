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
using System.Text;
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using OpenRasta.Security;
using WiM.Security;
using SiGLServices.Security;
using SiGLServices.Resources;


namespace SiGLServices.Handlers
{
    public class ProjDurationHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<proj_duration> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<proj_duration>().OrderBy(e => e.duration_value).ToList();

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
            proj_duration anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<proj_duration>().FirstOrDefault(e => e.proj_duration_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectDuration")]
        public OperationResult GetProjectDuration(Int32 projectId)
        {
            proj_duration anEntity = null;

            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<project>().Include(p => p.proj_duration).FirstOrDefault(p => p.project_id == projectId).proj_duration;
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
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(proj_duration anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.duration_value))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<proj_duration>(anEntity);
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
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, proj_duration anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.duration_value))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<proj_duration>(entityId, anEntity);
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
            proj_duration anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<proj_duration>().FirstOrDefault(i => i.proj_duration_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<proj_duration>(anEntity);
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