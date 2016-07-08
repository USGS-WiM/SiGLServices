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
    public class DataManagerHandler : SiGLHandlerBase
    {
        #region GetMethods
        [RequiresAuthentication]
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<data_manager> entities = null;

            try
            {
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        entities = sa.Select<data_manager>().OrderBy(e => e.data_manager_id).ToList();

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

        [RequiresAuthentication]
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get(Int32 entityId)
        {
            data_manager anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<data_manager>().FirstOrDefault(e => e.data_manager_id == entityId);
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

        [RequiresAuthentication]
        [HttpOperation(HttpMethod.GET, ForUriName = "GetLoggedUser")]
        public OperationResult GetLoggedUser()
        {
            data_manager aMember = null;
            try
            {
                //Get basic authentication password
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        List<data_manager> MemberList = sa.Select<data_manager>()
                                        .Where(m => m.username.ToUpper() == username.ToUpper()).ToList();
                        aMember = MemberList.First<data_manager>();

                    }//end using
                }//end using
                return new OperationResult.OK { ResponseResource = aMember };
            }
            catch
            {
                return new OperationResult.BadRequest();
            }
        }//end HttpMethod.GET

        [RequiresAuthentication]
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectDataManager")]
        public OperationResult GetProjectDataManager(Int32 projectId)
        {
            data_manager anEntity = null;

            try
            {
                //Return BadRequest if there is no ID
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<project>().Include(p => p.data_manager).SingleOrDefault(pc => pc.project_id == projectId).data_manager;
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

        [RequiresAuthentication]
        [HttpOperation(HttpMethod.GET, ForUriName = "GetDMListModel")]
        public OperationResult GetDataManagerList()
        {
            List<dm_list_view> entities = null;
            try
            {
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {

                        entities = sa.getTable<dm_list_view>(new Object[1] { null }).ToList();
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
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(data_manager anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.fname) || string.IsNullOrEmpty(anEntity.lname) ||
                    string.IsNullOrEmpty(anEntity.username) || string.IsNullOrEmpty(anEntity.phone) ||
                    string.IsNullOrEmpty(anEntity.email))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        if (string.IsNullOrEmpty(anEntity.password))
                            anEntity.password = generateDefaultPassword(anEntity);
                        else
                            anEntity.password = Encoding.UTF8.GetString(Convert.FromBase64String(anEntity.password));

                        if (anEntity.role_id <= 0) anEntity.role_id = sa.Select<role>().SingleOrDefault(r => r.role_name == ManagerRole).role_id;
                        anEntity.salt = Cryptography.CreateSalt();
                        anEntity.password = Cryptography.GenerateSHA256Hash(anEntity.password, anEntity.salt);
                        anEntity = sa.Add<data_manager>(anEntity);
                        sm(sa.Messages);

                        //remove info not relevant
                        anEntity.password = string.Empty;
                        anEntity.salt = string.Empty;

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
        [RequiresAuthentication]
        [HttpOperation(HttpMethod.PUT)]
        public OperationResult Put(Int32 entityId, data_manager anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.fname) || string.IsNullOrEmpty(anEntity.lname) ||
                    string.IsNullOrEmpty(anEntity.username) || string.IsNullOrEmpty(anEntity.phone) ||
                    string.IsNullOrEmpty(anEntity.email))
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        //fetch the object to be updated (assuming that it exists)
                        data_manager ObjectToBeUpdated = sa.Select<data_manager>().SingleOrDefault(m => m.data_manager_id == entityId);
                        if (ObjectToBeUpdated == null) throw new NotFoundRequestException("Requested member not found.");

                        if (!IsAuthorizedToEdit(ObjectToBeUpdated.username))
                            return new OperationResult.Forbidden { Description = "Not authorized to edit specified user" };

                        ObjectToBeUpdated.username = anEntity.username;
                        ObjectToBeUpdated.fname = anEntity.fname;
                        ObjectToBeUpdated.lname = anEntity.lname;
                        ObjectToBeUpdated.organization_system_id = anEntity.organization_system_id;
                        ObjectToBeUpdated.phone = anEntity.phone;
                        ObjectToBeUpdated.email = anEntity.email;
                        
                        //admin can only change role
                        if (IsAuthorized(AdminRole) && anEntity.role_id > 0)
                            ObjectToBeUpdated.role_id = anEntity.role_id;


                        if (!string.IsNullOrEmpty(anEntity.password) && !Cryptography
                            .VerifyPassword(Encoding.UTF8.GetString(Convert.FromBase64String(anEntity.password)),
                                                                    ObjectToBeUpdated.salt, ObjectToBeUpdated.password))
                        {
                            ObjectToBeUpdated.salt = Cryptography.CreateSalt();
                            ObjectToBeUpdated.password = Cryptography.GenerateSHA256Hash(Encoding.UTF8
                                .GetString(Convert.FromBase64String(anEntity.password)), ObjectToBeUpdated.salt);
                            ObjectToBeUpdated.reset_flag = null;
                            sm(MessageType.info, "Password updated.");
                        }

                        anEntity = sa.Update<data_manager>(entityId, ObjectToBeUpdated);
                        sm(sa.Messages);
                        //remove info not relevant
                        anEntity.password = string.Empty;
                        anEntity.salt = string.Empty;

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
            data_manager anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<data_manager>().FirstOrDefault(i => i.data_manager_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<data_manager>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
        #endregion

        #region Helper Methods

        private string generateDefaultPassword(data_manager dm)
        {
            //STNDefau1t+numbercharInlastname+first2letterFirstName
            string generatedPassword = "SiGLDefau1t" + dm.lname.Count() + dm.fname.Substring(0, 2);
            sm(MessageType.info, "Generated Password: " + generatedPassword);

            return generatedPassword;
        }//end buildDefaultPassword
        #endregion

    }
}