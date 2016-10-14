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
// 09.21.16 - TR - Created
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
    public class MonitorCoordinationHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<monitoring_coordination> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<monitoring_coordination>().OrderBy(e => e.effort).ToList();

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
            monitoring_coordination anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<monitoring_coordination>().FirstOrDefault(e => e.monitoring_coordination_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectMonitorCoords")]
        public OperationResult GetProjectMonitorCoords(Int32 projectId)
        {
            List<monitoring_coordination> entities = null;

            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<project_monitor_coord>().Where(sf => sf.project_id == projectId).Select(ft => ft.monitoring_coordination).ToList();
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
        public OperationResult POST(monitoring_coordination anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.effort))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<monitoring_coordination>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        //posts relationship, then returns list of media_type for the site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectMonitorCoord")]
        public OperationResult AddProjectMonitorCoord(Int32 projectId, Int32 monitorCoordId)
        {
            project_monitor_coord anEntity = null;
            List<monitoring_coordination> monitorCoordList = null;
            try
            {
                if (projectId <= 0 || monitorCoordId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        project aProj = sa.Select<project>().FirstOrDefault(s => s.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<monitoring_coordination>().First(n => n.monitoring_coordination_id == monitorCoordId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<project_monitor_coord>().FirstOrDefault(nt => nt.monitoring_coordination_id == monitorCoordId && nt.project_id == projectId) == null)
                        {
                            anEntity = new project_monitor_coord();
                            anEntity.project_id = projectId;
                            anEntity.monitoring_coordination_id = monitorCoordId;
                            anEntity = sa.Add<project_monitor_coord>(anEntity);

                            aProj.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(aProj);

                            sm(sa.Messages);
                        }
                        //return list of freq types
                        monitorCoordList = sa.Select<monitoring_coordination>().Where(nn => nn.project_monitor_coord.Any(nns => nns.project_id == projectId)).ToList();

                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = monitorCoordList, Description = this.MessageString };
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
        public OperationResult Put(Int32 entityId, monitoring_coordination anEntity)
        {
            try
            {
                if (entityId <= 0)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<monitoring_coordination>(entityId, anEntity);
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
            monitoring_coordination anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<monitoring_coordination>().FirstOrDefault(i => i.monitoring_coordination_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<monitoring_coordination>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveProjectMonitorCoord")]
        public OperationResult RemoveProjectMonitorCoord(Int32 projectId, Int32 monitorCoordId)
        {
            try
            {
                if (projectId <= 0 || monitorCoordId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project aProj = sa.Select<project>().FirstOrDefault(s => s.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        project_monitor_coord ObjectToBeDeleted = sa.Select<project_monitor_coord>().SingleOrDefault(nns => nns.project_id == projectId && nns.monitoring_coordination_id == monitorCoordId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<project_monitor_coord>(ObjectToBeDeleted);

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