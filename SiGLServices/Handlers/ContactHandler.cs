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
    public class ContactHandler : SiGLHandlerBase
    {
        #region GetMethods
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole})]
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<contact> entities = null;

            try
            {
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        entities = sa.Select<contact>().OrderBy(e => e.contact_id).ToList();

                        sm(MessageType.info, "Count: " + entities.Count());
                        sm(sa.Messages);

                    }//end using
                }//end using
                return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }            
        }//end HttpMethod.GET

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get(Int32 entityId)
        {
            contact anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<contact>().FirstOrDefault(e => e.contact_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }//end HttpMethod.GET
                
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectContacts")]
        public OperationResult GetProjectContacts(Int32 projectId)
        {
            List<contact> entities = null;

            try
            {
                //Return BadRequest if there is no ID
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");
                
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project_contacts>().Where(pc => pc.project_id == projectId).Select(c => c.contact).ToList();

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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetOrgSysContacts")]
        public OperationResult GetOrgSysContacts(Int32 orgSysId)
        {
            List<contact> entities = null;

            try
            {
                //Return BadRequest if there is no ID
                if (orgSysId <= 0) throw new BadRequestException("Invalid input parameters");
                
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<organization_system>().Include(o => o.contacts).SingleOrDefault(o => o.organization_system_id == orgSysId).contacts.ToList();

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
        public OperationResult POST(contact anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || !anEntity.organization_system_id.HasValue || string.IsNullOrEmpty(anEntity.email) || string.IsNullOrEmpty(anEntity.phone) )
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<contact>(anEntity);
                        sm(sa.Messages);

                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }

        }//end HttpMethod.GET

        //posts relationship, then returns list of network_names for the anEntity.site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectContact")]
        public OperationResult AddNetworkName(Int32 projectId, Int32 contactId)
        {
            project_contacts anEntity = null;
            List<contact> contactList = null;
            try
            {
                if (projectId <= 0 || contactId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        if (sa.Select<project>().First(p => p.project_id == projectId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<contact>().First(n => n.contact_id == contactId) == null)
                            throw new NotFoundRequestException();

                        if (sa.Select<project_contacts>().FirstOrDefault(nt => nt.contact_id == contactId && nt.project_id == projectId) == null)
                        {
                            anEntity = new project_contacts();
                            anEntity.project_id = projectId;
                            anEntity.contact_id = contactId;
                            anEntity = sa.Add<project_contacts>(anEntity);
                            sm(sa.Messages);
                        }
                        //return list of network types
                        contactList = sa.Select<contact>().Where(nn => nn.project_contacts.Any(nns => nns.project_id == projectId)).ToList();

                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = contactList, Description = this.MessageString };
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
        public OperationResult Put(Int32 entityId, contact anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || !anEntity.organization_system_id.HasValue || string.IsNullOrEmpty(anEntity.email) || string.IsNullOrEmpty(anEntity.phone))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<contact>(entityId, anEntity);
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
        [RequiresRole(AdminRole)]
        [HttpOperation(HttpMethod.DELETE)]
        public OperationResult Delete(Int32 entityId)
        {
            contact anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<contact>().FirstOrDefault(i => i.contact_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<contact>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }

        }//end HttpMethod.PUT

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveProjectContact")]
        public OperationResult RemoveProjectContact(Int32 projectId, Int32 contactId)
        {
            try
            {
                if (projectId <= 0 || contactId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        if (sa.Select<project>().First(s => s.project_id == projectId) == null)
                            throw new NotFoundRequestException();

                        project_contacts ObjectToBeDeleted = sa.Select<project_contacts>().SingleOrDefault(pc => pc.project_id == projectId && pc.contact_id == contactId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<project_contacts>(ObjectToBeDeleted);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }
        #endregion
    }//end ContactHandler
}
