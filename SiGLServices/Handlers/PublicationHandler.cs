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
    public class PublicationHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<publication> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<publication>().OrderBy(e => e.publication_id).ToList();

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
            publication anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<publication>().FirstOrDefault(e => e.publication_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectPublications")]
        public OperationResult GetProjectPublications(Int32 projectId)
        {
            List<publication> entities = null;

            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<project_publication>().Where(sf => sf.project_id == projectId).Select(ft => ft.publication).ToList();
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
        public OperationResult POST(publication anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.description) && string.IsNullOrEmpty(anEntity.title) && string.IsNullOrEmpty(anEntity.url))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<publication>(anEntity);
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
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectPublication")]
        public OperationResult AddProjectPublication(Int32 projectId, publication entity)
        {
            project_publication anEntity = null;
            List<publication> publicationList = null;
            try
            {
                if (projectId <= 0 || (string.IsNullOrEmpty(entity.title) && string.IsNullOrEmpty(entity.description) && string.IsNullOrEmpty(entity.url))) throw new BadRequestException("Invalid input parameters");
                
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        if (sa.Select<project>().First(s => s.project_id == projectId) == null)
                            throw new NotFoundRequestException();

                        publication pub = sa.Select<publication>().FirstOrDefault(p => p.title == entity.title && p.description == entity.description && p.url == entity.url);
                        if (pub == null)
                        {
                            pub = sa.Add<publication>(entity);
                        }

                        if (sa.Select<project_publication>().FirstOrDefault(nt => nt.publication_id == pub.publication_id && nt.project_id == projectId) == null)
                        {
                            anEntity = new project_publication();
                            anEntity.project_id = projectId;
                            anEntity.publication_id = pub.publication_id;
                            anEntity = sa.Add<project_publication>(anEntity);
                            sm(sa.Messages);
                        }
                        //return list of freq types
                        publicationList = sa.Select<publication>().Where(nn => nn.project_publication.Any(nns => nns.project_id == projectId)).ToList();
                        
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = publicationList, Description = this.MessageString };
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
        public OperationResult Put(Int32 entityId, publication anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.description) && string.IsNullOrEmpty(anEntity.title) && string.IsNullOrEmpty(anEntity.url))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Update<publication>(entityId, anEntity);
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
            publication anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<publication>().FirstOrDefault(i => i.publication_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<publication>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
        
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveProjectPublication")]
        public OperationResult RemoveProjectPublication(Int32 projectId, Int32 publicationId)
        {
            try
            {
                if (projectId <= 0 || publicationId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        if (sa.Select<project>().First(s => s.project_id == projectId) == null)
                            throw new NotFoundRequestException();

                        project_publication ObjectToBeDeleted = sa.Select<project_publication>().SingleOrDefault(nns => nns.project_id == projectId && nns.publication_id == publicationId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<project_publication>(ObjectToBeDeleted);
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