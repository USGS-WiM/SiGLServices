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
// 07.08.16 - TR - Created
#endregion

using OpenRasta.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using SiGLServices.Utilities.ServiceAgent;
using SiGLServices.Resources;
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using OpenRasta.Security;
using WiM.Security;
using SiGLServices.Security;


namespace SiGLServices.Handlers
{
    public class OrganizationSystemHandler : SiGLHandlerBase
    {
        #region GetMethods
        //get all organization_systems
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get()
        {
            List<organization_system> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<organization_system>().OrderBy(e => e.organization_system_id).ToList();

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

        //get all organizationResources
        [HttpOperation(HttpMethod.GET, ForUriName = "AllOrgResources")]
        public OperationResult GetAllOrgResources()
        {
            List<organization_system> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    IQueryable<organization_system> query = sa.Select<organization_system>().Include(os => os.organization).Include(os => os.division).Include(os => os.section).Where(os => os.organization_system_id > 0);

                    entities = query.AsEnumerable().Select(orgRes => new OrganizationResource 
                        {
                            organization_system_id = orgRes.organization_system_id,
                            org_id = orgRes.org_id,
                            OrganizationName = orgRes.organization.organization_name,
                            div_id = orgRes.div_id > 0 ? orgRes.div_id : 0,
                            DivisionName = orgRes.div_id > 0 ? orgRes.division.division_name : "",
                            sec_id = orgRes.sec_id > 0 ? orgRes.sec_id : 0,
                            SectionName = orgRes.sec_id > 0 ? orgRes.section.section_name : "",
                            science_base_id = orgRes.science_base_id
                        }).ToList<organization_system>();

                    sm(MessageType.info, "Count: " + entities.Count());
                    sm(sa.Messages);

                }//end using
                return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }        

        //get an organization_system
        [HttpOperation(HttpMethod.GET)]
        public OperationResult Get(Int32 entityId)
        {
            organization_system anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<organization_system>().FirstOrDefault(e => e.organization_system_id == entityId);
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

        //get an organizationResource
        [HttpOperation(HttpMethod.GET, ForUriName = "GetAnOrgSystemResource")]
        public OperationResult GetAnOrgSystemResource(Int32 orgSystemId)
        {
            organization_system anEntity = null;

            try
            {
                if (orgSystemId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<organization_system>().Include(os => os.organization).Include(os => os.division).Include(os => os.section)
                        .Where(e => e.organization_system_id == orgSystemId).Select(orgRes => new OrganizationResource
                        {
                            organization_system_id = orgRes.organization_system_id,
                            org_id = orgRes.org_id,
                            OrganizationName = orgRes.organization.organization_name,
                            div_id = orgRes.division != null ? orgRes.div_id : 0,
                            DivisionName = orgRes.division != null ? orgRes.division.division_name : "",
                            sec_id = orgRes.section != null ? orgRes.sec_id : 0,
                            SectionName = orgRes.section != null ? orgRes.section.section_name : "",
                            science_base_id = orgRes.science_base_id
                        }).FirstOrDefault();

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
        
        //get project organizationResources        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectOrganizations")]
        public OperationResult GetProjectOrganizations(Int32 projectId)
        {
            List<organization_system> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<organization_system>().Include(os => os.project_cooperators).Include(os => os.organization).Include(os => os.division).Include(os => os.section)
                                .Where(o=>o.project_cooperators.Any(p=>p.project_id == projectId)).Select(orgRes => new OrganizationResource
                    {
                        organization_system_id = orgRes.organization_system_id,
                        org_id = orgRes.org_id,
                        OrganizationName = orgRes.organization.organization_name,
                        div_id = orgRes.division != null ? orgRes.div_id : 0,
                        DivisionName = orgRes.division != null ? orgRes.division.division_name : "",
                        sec_id = orgRes.section != null ? orgRes.sec_id : 0,
                        SectionName = orgRes.section != null ? orgRes.section.section_name : "",
                        science_base_id = orgRes.science_base_id
                    }).ToList<organization_system>();

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
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(organization_system anEntity)
        {
            try
            {
                if (anEntity.org_id <= 0)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<organization_system>(anEntity);
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = anEntity, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST, ForUriName = "AddProjectOrganization")]
        public OperationResult AddProjectOrganization(Int32 projectId, Int32 orgId, [Optional] string divId, [Optional] string secId)
        {
            project_cooperators anEntity = null;
            List<organization_system> CooperatorList = null;
            try
            {
                if (projectId <= 0 || orgId <= 0)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project aProj = sa.Select<project>().Include(p=>p.data_manager).First(p => p.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        Int32 dID = divId != null && divId != "0" ? Convert.ToInt32(divId) : 0;
                        Int32 sID = secId != null && secId != "0" ? Convert.ToInt32(secId) : 0;

                        organization_system orgSys = sa.Select<organization_system>().FirstOrDefault(org => org.org_id == orgId && org.div_id == dID && org.sec_id == sID);

                        if (orgSys == null)
                        {
                            //post it
                            organization_system newOrg = new organization_system();
                            newOrg.org_id = orgId; newOrg.div_id = dID; newOrg.sec_id = sID;
                            orgSys = sa.Add<organization_system>(newOrg);
                        }

                        //check authorization
                        if (!IsAuthorizedToEdit(aProj.data_manager.username))
                            return new OperationResult.Forbidden { Description = "Not Authorized" };

                        if (sa.Select<project_cooperators>().FirstOrDefault(pc => pc.organization_system_id == orgSys.organization_system_id && pc.project_id == projectId) == null)
                        {
                            anEntity = new project_cooperators();
                            anEntity.project_id = projectId;
                            anEntity.organization_system_id = orgSys.organization_system_id;
                            anEntity = sa.Add<project_cooperators>(anEntity);
                            sm(sa.Messages);
                        }

                        //update project's last edited stamp
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);

                        //return list of orgResources for this project
                        CooperatorList = sa.Select<organization_system>().Include(os => os.project_cooperators).Include(os => os.organization).Include(os => os.division).Include(os => os.section)
                                .Where(o => o.project_cooperators.Any(p => p.project_id == projectId)).Select(orgRes => new OrganizationResource
                                {
                                    organization_system_id = orgRes.organization_system_id,
                                    org_id = orgRes.org_id,
                                    OrganizationName = orgRes.organization.organization_name,
                                    div_id = orgRes.division != null ? orgRes.div_id : 0,
                                    DivisionName = orgRes.division != null ? orgRes.division.division_name : "",
                                    sec_id = orgRes.section != null ? orgRes.sec_id : 0,
                                    SectionName = orgRes.section != null ? orgRes.section.section_name : "",
                                    science_base_id = orgRes.science_base_id
                                }).ToList<organization_system>();
                    }//end using
                }//end using
                return new OperationResult.Created { ResponseResource = CooperatorList, Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HttpMethod.POST

        #endregion

        #region PutMethods
        // Don't think there will ever be any PUTs on this entity
       
        #endregion

        #region DeleteMethods
        /// 
        /// Force the user to provide authentication and authorization 
        /// Won't be any delete because this same organization_system can be used on many projects (project_cooperators table)
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "RemoveProjectOrganization")]
        public OperationResult RemoveProjectOrganization(Int32 projectId, Int32 orgSystemId)
        {
            try
            {
                if (projectId <= 0 || orgSystemId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project aProj = sa.Select<project>().Include(p=>p.data_manager).First(p => p.project_id == projectId);
                        if (aProj == null)
                            throw new NotFoundRequestException();

                        //check authorization
                        if (!IsAuthorizedToEdit(aProj.data_manager.username))
                            return new OperationResult.Forbidden { Description = "Not Authorized" };

                        project_cooperators projCoop = sa.Select<project_cooperators>().SingleOrDefault(po => po.project_id == projectId && po.organization_system_id == orgSystemId);
                        if (projCoop == null) throw new WiM.Exceptions.NotFoundRequestException();

                        sa.Delete<project_cooperators>(projCoop);

                        //update project's last edited stamp
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);

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