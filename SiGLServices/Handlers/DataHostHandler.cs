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
// 06.24.16 - TR - Created
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
    public class DataHostHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<data_host> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<data_host>().OrderBy(e => e.data_host_id).ToList();

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
            data_host anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<data_host>().FirstOrDefault(e => e.data_host_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectDataHosts")]
        public OperationResult GetProjectDataHosts(Int32 projectId)
        {
            List<data_host> entities = null;

            try
            {
                //Return BadRequest if there is no ID
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.data_host).SingleOrDefault(pc => pc.project_id == projectId).data_host.ToList();

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

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(data_host anEntity)
        {
            List<data_host> entities = null;
            try
            {
                if (anEntity.project_id <= 0 ||
                    (string.IsNullOrEmpty(anEntity.host_name) && string.IsNullOrEmpty(anEntity.description) && string.IsNullOrEmpty(anEntity.portal_url)))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project requestedProject = sa.Select<project>().Include(p => p.data_manager).First(p => p.project_id == anEntity.project_id);
                        if (requestedProject == null)
                            throw new BadRequestException("project does not exist");

                        if (!IsAuthorizedToEdit(requestedProject.data_manager.username))
                            return new OperationResult.Forbidden { Description = "Not authorized" };

                        anEntity = sa.Add<data_host>(anEntity);
                        requestedProject.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(requestedProject); //WILL THIS WORK??
                        sm(sa.Messages);
                        entities = sa.Select<data_host>().Where(dh => dh.project_id == anEntity.project_id).ToList();
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }

        }//end HttpMethod.GET

        #endregion
        #region PutMethods

        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, data_host anEntity)
        {
            try
            {
                if (anEntity.project_id <= 0 ||
                    (string.IsNullOrEmpty(anEntity.host_name) && string.IsNullOrEmpty(anEntity.description) && string.IsNullOrEmpty(anEntity.portal_url)))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<data_host>(entityId, anEntity);
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
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE)]
        public OperationResult Delete(Int32 entityId)
        {
            data_host anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<data_host>().FirstOrDefault(i => i.data_host_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        Int32 projId = anEntity.project_id.Value;                        
                        sa.Delete<data_host>(anEntity);

                        project updateProj = sa.Select<project>().SingleOrDefault(p => p.project_id == projId);
                        if (updateProj == null) throw new WiM.Exceptions.NotFoundRequestException();
                        updateProj.last_edited_stamp = DateTime.Now.Date;                        
                        sa.Update<project>(updateProj);
                        
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }

        }//end HttpMethod.PUT

        #endregion
    }
}