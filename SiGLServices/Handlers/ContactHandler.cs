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
using OpenRasta.Security;
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
       
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<contact> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<contact>().OrderBy(e => e.contact_id).ToList();

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
            contact anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
               
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<contact>().FirstOrDefault(e => e.contact_id == entityId);
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectContacts")]
        public OperationResult GetProjectContacts(Int32 projectId)
        {
            List<contact> entities = null;

            try
            {
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

        //[SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        //[HttpOperation(HttpMethod.POST)]
        //public OperationResult POST(contact anEntity)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(anEntity.name) || !anEntity.organization_system_id.HasValue || string.IsNullOrEmpty(anEntity.email) || string.IsNullOrEmpty(anEntity.phone) )
        //            throw new BadRequestException("Invalid input parameters");
        //        using (EasySecureString securedPassword = GetSecuredPassword())
        //        {
        //            using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
        //            {
        //                anEntity = sa.Add<contact>(anEntity);
        //                sm(sa.Messages);

        //            }//end using
        //        }//end using
        //        return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
        //    }
        //    catch (Exception ex)
        //    { return HandleException(ex); }

        //}//end HttpMethod.GET

        //posts relationship, then returns list of network_names for the anEntity.site_id
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectContact")]
        public OperationResult AddProjectContact(Int32 projectId, contact anEntity)
        {
            project_contacts projContact = null;
            List<contact> contactList = null;
            try
            {
                if (projectId <= 0 || string.IsNullOrEmpty(anEntity.name) || !anEntity.organization_system_id.HasValue || string.IsNullOrEmpty(anEntity.email) || string.IsNullOrEmpty(anEntity.phone))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        project aProj = sa.Select<project>().First(p => p.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        contact con = sa.Select<contact>().FirstOrDefault(p => p.name == anEntity.name && p.organization_system_id == anEntity.organization_system_id && p.email == anEntity.email && p.phone == anEntity.phone);
                        if (con == null)
                        {
                            con = sa.Add<contact>(anEntity);
                        }

                        if (sa.Select<project_contacts>().FirstOrDefault(nt => nt.contact_id == anEntity.contact_id && nt.project_id == projectId) == null)
                        {
                            projContact = new project_contacts();
                            projContact.project_id = projectId;
                            projContact.contact_id = con.contact_id;
                            projContact = sa.Add<project_contacts>(projContact);
                            sm(sa.Messages);
                        }
                        //update project's last edited stamp
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);

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

                        List<project> projList = sa.Select<project>().Include(p => p.project_contacts).Where(p => p.project_contacts.Any(c => c.contact_id == entityId)).ToList();
                        foreach (project p in projList)
                        {
                            p.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(p);
                        }
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
                        project aProj = sa.Select<project>().First(p => p.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        project_contacts ObjectToBeDeleted = sa.Select<project_contacts>().SingleOrDefault(pc => pc.project_id == projectId && pc.contact_id == contactId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<project_contacts>(ObjectToBeDeleted);

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
    }//end ContactHandler
}
