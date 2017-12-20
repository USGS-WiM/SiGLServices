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
    public class KeywordHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<keyword> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<keyword>().OrderBy(e => e.keyword_id).ToList();

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
            keyword anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<keyword>().FirstOrDefault(e => e.keyword_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetKeywordByTerm")]
        public OperationResult GetKeywordByTerm(string term)
        {
            keyword anEntity = null;

            try
            {
                if (string.IsNullOrEmpty(term)) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<keyword>().FirstOrDefault(e => e.term == term);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectKeyword")]
        public OperationResult GetProjectKeyword(Int32 projectId)
        {
            List<keyword> entities = null;

            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    entities = sa.Select<project_keywords>().Where(pk => pk.project_id == projectId).Select(k => k.keyword).ToList();
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
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole})]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(keyword anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.term))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<keyword>(anEntity);
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
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectKeyword")]
        public OperationResult AddProjectKeyword(Int32 projectId, string term)
        {
            project_keywords anEntity = null;
            keyword newKey = null;
            List<keyword> keywordList = null;
            try
            {
                if (projectId <= 0 || string.IsNullOrEmpty(term)) throw new BadRequestException("Invalid input parameters");
                
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        project aProj = sa.Select<project>().First(s => s.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        newKey = sa.Select<keyword>().FirstOrDefault(n => n.term.ToLower() == term.ToLower());
                        if (newKey == null)
                        {
                            newKey = new keyword();
                            newKey.term = term;
                            newKey = sa.Add<keyword>(newKey);
                        }
                        

                        if (sa.Select<project_keywords>().FirstOrDefault(nt => nt.keyword_id == newKey.keyword_id && nt.project_id == projectId) == null)
                        {
                            anEntity = new project_keywords();
                            anEntity.project_id = projectId;
                            anEntity.keyword_id = newKey.keyword_id;
                            anEntity = sa.Add<project_keywords>(anEntity);

                            aProj.last_edited_stamp = DateTime.Now.Date;
                            sa.Update<project>(aProj);

                            sm(sa.Messages);
                        }
                        //return list of freq types
                        keywordList = sa.Select<keyword>().Where(nn => nn.project_keywords.Any(nns => nns.project_id == projectId)).ToList();
                        
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = keywordList, Description = this.MessageString };
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
        public OperationResult Put(Int32 entityId, keyword anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.term))
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {                        
                        anEntity = sa.Update<keyword>(entityId, anEntity);
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
            keyword anEntity = null;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<keyword>().FirstOrDefault(i => i.keyword_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<keyword>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE
        
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveProjectKeyword")]
        public OperationResult RemoveProjectKeyword(Int32 projectId, Int32 keywordId)
        {
            try
            {
                if (projectId <= 0 || keywordId <= 0) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project aProj = sa.Select<project>().First(s => s.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        project_keywords ObjectToBeDeleted = sa.Select<project_keywords>().SingleOrDefault(nns => nns.project_id == projectId && nns.keyword_id == keywordId);

                        if (ObjectToBeDeleted == null) throw new NotFoundRequestException();
                        sa.Delete<project_keywords>(ObjectToBeDeleted);
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