﻿//------------------------------------------------------------------------------

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
// 07.11.16 - TR - Created
#endregion

using OpenRasta.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using SiGLServices.Utilities.ServiceAgent;
using SiGLServices.Security;
using SiGLServices.Resources;
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using WiM.Security;
using OpenRasta.Collections;

namespace SiGLServices.Handlers
{
    public class ProjectHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET, ForUriName = "GetAllProjects")]
        public OperationResult Get()
        {
            List<project> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().OrderBy(e => e.name).ToList();
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
            project anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<project>().FirstOrDefault(e => e.project_id == entityId);
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteProject")]
        public OperationResult GetSiteProject(Int32 siteId)
        {
            project anEntity = null;

            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<site>().Include(s => s.project).FirstOrDefault(e => e.site_id == siteId).project;
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetDataHostProject")]
        public OperationResult GetDataHostProject(Int32 dataHostId)
        {
            project anEntity = null;

            try
            {
                if (dataHostId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<data_host>().Include(s => s.project).FirstOrDefault(e => e.data_host_id == dataHostId).project;
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetContactProjects")]
        public OperationResult GetContactProjects(Int32 contactId)
        {
            List<project> entities = null;

            try
            {
                if (contactId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_contacts).Where(p => p.project_contacts.Any(c=>c.contact_id == contactId)).ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetDataManagersProjects")]
        public OperationResult GetDataManagersProjects(Int32 dataManagerId)
        {
            List<project> entities = null;

            try
            {
                if (dataManagerId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Where(p => p.data_manager_id == dataManagerId).ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetKeyWordProjects")]
        public OperationResult GetKeyWordProjects(Int32 keywordId)
        {
            List<project> entities = null;

            try
            {
                if (keywordId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_keywords).Where(p => p.project_keywords.Any(c => c.keyword_id == keywordId)).ToList();
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetPublicationProjects")]
        public OperationResult GetPublicationProjects(Int32 publicationId)
        {
            List<project> entities = null;

            try
            {
                if (publicationId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_publication).Where(p => p.project_publication.Any(c => c.publication_id == publicationId)).ToList();
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetObjectiveProjects")]
        public OperationResult GetObjectiveProjects(Int32 objectiveId)
        {
            List<project> entities = null;

            try
            {
                if (objectiveId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_objectives).Where(p => p.project_objectives.Any(c => c.objective_id == objectiveId)).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetFreqSiteProjects")]
        public OperationResult GetFreqSiteProjects(Int32 frequencyId)
        {
            List<project> entities = new List<project>();

            try
            {
                if (frequencyId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> freqSites = sa.Select<site>().Include(s=>s.project).Where(x => x.site_frequency.Any(c => c.frequency_type_id == frequencyId)).ToList();
                    foreach (site s in freqSites)
                    {
                        if (s.project != null)
                            entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetLakeSiteProjects")]
        public OperationResult GetLakeSiteProjects(Int32 lakeId)
        {
            List<project> entities = new List<project>();

            try
            {
                if (lakeId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> lakeSites = sa.Select<site>().Include(s => s.project).Where(x => x.lake_type_id == lakeId).ToList();
                    foreach (site s in lakeSites)
                    {
                        entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetMediaSiteProjects")]
        public OperationResult GetMediaSiteProjects(Int32 mediaId)
        {
            List<project> entities = new List<project>(); 

            try
            {
                if (mediaId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> mediaSites = sa.Select<site>().Include(s => s.project).Where(x => x.site_media.Any(m=> m.media_type_id == mediaId)).ToList();
                    foreach (site s in mediaSites)
                    {
                        entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetMonitorCoordinationProjects")]
        public OperationResult GetMonitorCoordinationProjects(Int32 monitorCoordId)
        {
            List<project> entities = null;

            try
            {
                if (monitorCoordId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_monitor_coord).Where(p => p.project_monitor_coord.Any(c => c.monitoring_coordination_id == monitorCoordId)).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetParameterSiteProjects")]
        public OperationResult GetParameterSiteProjects(Int32 parameterId)
        {
            List<project> entities = new List<project>(); 

            try
            {
                if (parameterId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> paramSites = sa.Select<site>().Include(s => s.project).Where(x => x.site_parameters.Any(m => m.parameter_type_id == parameterId)).ToList();
                    foreach (site s in paramSites)
                    {
                        entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetResourceSiteProjects")]
        public OperationResult GetResourceSiteProjects(Int32 resourceId)
        {
            List<project> entities = new List<project>();

            try
            {
                if (resourceId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> resSites = sa.Select<site>().Include(s => s.project).Where(x => x.site_resource.Any(m => m.resource_type_id == resourceId)).ToList();
                    foreach (site s in resSites)
                    {
                        entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetSiteStatusProjects")]
        public OperationResult GetSiteStatusProjects(Int32 statusId)
        {
            List<project> entities = new List<project>();

            try
            {
                if (statusId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    List<site> statusSites = sa.Select<site>().Include(s => s.project).Where(x => x.status_type_id == statusId).ToList();
                    foreach (site s in statusSites)
                    {
                        entities.Add(s.project);
                    }
                    entities = entities.Distinct().ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetOrgSysProjects")]
        public OperationResult GetOrgSysProjects(Int32 orgSystemId)
        {
            List<project> entities = null;

            try
            {
                if (orgSystemId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.project_cooperators).Where(p => p.project_cooperators.Any(c => c.organization_system_id == orgSystemId)).ToList();                    
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectDurationProjects")]
        public OperationResult GetProjectDurationProjects(Int32 durationId)
        {
            List<project> entities = null;

            try
            {
                if (durationId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Where(p => p.proj_duration_id == durationId).ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectStatusProjects")]
        public OperationResult GetProjectStatusProjects(Int32 statusId)
        {
            List<project> entities = null;

            try
            {
                if (statusId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Where(p => p.proj_status_id == statusId).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetFlaggedProjects")]
        public OperationResult GetFlaggedProjects(Int32 flag)
        {
            List<project> entities = null;

            try
            {
                //if (flag <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    if (flag == 0)
                        entities = sa.Select<project>().Where(p => p.ready_flag == 0 || !p.ready_flag.HasValue).ToList();
                    else 
                        entities = sa.Select<project>().Where(p => p.ready_flag == flag).ToList();

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

        //used to populate sigl map project dropdown
        [HttpOperation(HttpMethod.GET, ForUriName="ProjectsWithSiteCount")]
        public OperationResult GetProjectsWithSiteCount()
        {
            List<project_sitecount_view> entities = null;
            try
            {
                using(SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.getTable<project_sitecount_view>(new Object[1] { null }).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetIndexProjects")]
        public OperationResult GetIndexProjects([Optional] Int32 dataManagerId)
        {
            List<project_sitecount_view> entities = null;
            IQueryable<project_sitecount_view> query = null;
            try
            {
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        query = sa.getTable<project_sitecount_view>(new Object[1] { null });
                        
                        if (IsAuthorized(AdminRole))
                        {
                            //for admin - if there's a dm ID, want all projects for this dm in this model format (SiGLDMS - Account page, user's project list)
                            if (dataManagerId > 0)
                            {
                                entities = query.Where(dp => dp.data_manager_id == dataManagerId).ToList();
                            }
                            else
                            { //else just give me all the projects in this format (SiGLDMS - project list)
                                entities = query.ToList();
                            }
                        }
                        else
                        {
                            //not authorized to see all projects, just give me my own in this format
                            data_manager dm = sa.Select<data_manager>().FirstOrDefault(y => y.username == Context.User.Identity.Name);

                            entities = query.Where(p => p.data_manager_id == dm.data_manager_id).ToList();
                        }

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

        //"/projects/GetFullProject/{scienceBaseId}").Named("GetFullProject")
        [HttpOperation(HttpMethod.GET, ForUriName = "GetFullProject")]
        public OperationResult GetFullProject([Optional] String scienceBaseId, [Optional] String projectId)
        {
            FullProject anEntity = null;
            IQueryable<project> query = null;
            try
            {
                if (string.IsNullOrEmpty(scienceBaseId) && string.IsNullOrEmpty(projectId)) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent(true))
                {
                    if (!string.IsNullOrEmpty(scienceBaseId))
                    {
                        query = sa.Select<project>().Include(e => e.data_host).Include(e=>e.sites).Include("sites.lake_type").Include(e => e.project_objectives).Include("project_objectives.objective_type")
                            .Include(e => e.project_monitor_coord).Include("project_monitor_coord.monitoring_coordination")
                            .Include(e => e.proj_status).Include(e => e.proj_duration).Include(e => e.project_keywords).Include("project_keywords.keyword")
                            .Include(e => e.project_cooperators).Include("project_cooperators.organization_system")
                            .Include("project_cooperators.organization_system.organization")
                            .Include("project_cooperators.organization_system.division")
                            .Include("project_cooperators.organization_system.section")
                            .Include(e => e.project_contacts).Include("project_contacts.contact")
                            .Include("project_contacts.contact.organization_system.organization")
                            .Include("project_contacts.contact.organization_system.division")
                            .Include("project_contacts.contact.organization_system.section")
                            .Include(e => e.project_publication).Include("project_publication.publication")
                            .Where(e => e.science_base_id == scienceBaseId);
                    }
                    if (!string.IsNullOrEmpty(projectId))
                    {
                        Int32 projId = Convert.ToInt32(projectId);
                        query = sa.Select<project>().Include(e => e.data_host).Include(e => e.sites).Include("sites.lake_type").Include(e => e.project_objectives).Include("project_objectives.objective_type")
                            .Include(e => e.project_monitor_coord).Include("project_monitor_coord.monitoring_coordination")
                            .Include(e=>e.proj_status).Include(e=>e.proj_duration)
                            .Include(e => e.project_keywords).Include("project_keywords.keyword")
                            .Include(e => e.project_cooperators).Include("project_cooperators.organization_system")
                            .Include("project_cooperators.organization_system.organization")
                            .Include("project_cooperators.organization_system.division")
                            .Include("project_cooperators.organization_system.section")
                            .Include(e => e.project_contacts).Include("project_contacts.contact")
                            .Include("project_contacts.contact.organization_system.organization")
                            .Include("project_contacts.contact.organization_system.division")
                            .Include("project_contacts.contact.organization_system.section")
                            .Include(e => e.project_publication).Include("project_publication.publication")
                            .Where(e => e.project_id == projId);
                    }
                    anEntity = query.AsEnumerable().Select(p => new FullProject
                        {
                            ProjectId = p.project_id,
                            ScienceBaseId = p.science_base_id,
                            Name = p.name,
                            StartDate = p.start_date != null ? p.start_date.Value.ToString() : "",
                            EndDate = p.end_date != null ? p.end_date.Value.ToString() : "",
                            DataManagerId = p.data_manager_id,
                            status_id = p.proj_status_id.HasValue ? p.proj_status_id : 0,
                            Status = p.proj_status_id > 0 && p.proj_status_id.HasValue ? p.proj_status.status_value : "",
                            duration_id = p.proj_duration_id.HasValue ? p.proj_duration_id : 0,
                            Duration = p.proj_duration_id > 0 && p.proj_duration_id.HasValue ? p.proj_duration.duration_value : "",
                            Description = p.description,
                            AdditionalInfo = p.additional_info,
                            Objectives = p.project_objectives.Select(po => new objective_type
                            {
                                objective_type_id = po.objective_type.objective_type_id, 
                                objective = po.objective_type.objective 
                            }).ToList(),
                            MonitoringCoords = p.project_monitor_coord.Select(pm => new monitoring_coordination
                            {
                                monitoring_coordination_id = pm.monitoring_coordination.monitoring_coordination_id,
                                effort = pm.monitoring_coordination.effort
                            }).ToList(),
                            Keywords = p.project_keywords.Select(po => new keyword 
                            { 
                                keyword_id = po.keyword.keyword_id, 
                                term = po.keyword.term 
                            }).ToList(),
                            ProjectWebsite = p.url,
                            DataHosts = p.data_host.Select(d => new data_host
                            { 
                                data_host_id = d.data_host_id, 
                                description = d.description, 
                                host_name = d.host_name, 
                                portal_url = d.portal_url, 
                                project_id = d.project_id
                            }).ToList(),
                            Organizations = p.project_cooperators.Where(pc => pc.organization_system_id > 0).Select(c => new OrganizationResource
                            {
                                organization_system_id = c.organization_system.organization_system_id,
                                org_id = c.organization_system.org_id,
                                OrganizationName = c.organization_system.organization.organization_name,
                                div_id = c.organization_system.div_id > 0 ? c.organization_system.div_id : 0,
                                DivisionName = c.organization_system.div_id > 0 ? c.organization_system.division.division_name : "",
                                sec_id = c.organization_system.sec_id > 0 ? c.organization_system.sec_id : 0,
                                SectionName = c.organization_system.sec_id > 0 ? c.organization_system.section.section_name : "",
                                science_base_id = c.organization_system.science_base_id
                            }).ToList(),
                            Contacts = p.project_contacts.Select(pc => new contactResource
                            {
                                contact_id = pc.contact.contact_id,
                                science_base_id = pc.contact.science_base_id,
                                name = pc.contact.name,
                                email = pc.contact.email,
                                phone = pc.contact.phone,
                                ContactOrgName = pc.contact.organization_system.organization.organization_name,
                                ContactDivName = pc.contact.organization_system.div_id > 0 ? pc.contact.organization_system.division.division_name : "",
                                ContactSecName = pc.contact.organization_system.sec_id > 0 ? pc.contact.organization_system.section.section_name : ""
                            }).ToList(),
                            Publications = p.project_publication.Select(pp => new publication
                            {
                                publication_id = pp.publication.publication_id,
                                description = pp.publication.description,
                                science_base_id = pp.publication.science_base_id,
                                title = pp.publication.title,
                                url = pp.publication.url
                            }).ToList(),
                            projectSites = p.sites.Select(s=> new SimpleSite
                            {
                                name = s.name,
                                latitude = s.latitude,
                                longitude = s.longitude,
                                site_id = s.site_id,
                                project_id = s.project_id.Value,
                                Lake = s.lake_type.lake,
                                State = s.state_province,
                                Country = s.country
                            }).ToList<SimpleSite>(),
                            created_stamp = p.created_stamp,
                            last_edited_stamp = p.last_edited_stamp
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
        }

        [HttpOperation(HttpMethod.GET, ForUriName = "GetFilteredProjects")]
        public OperationResult GetFilteredProjects([Optional] string lakes, [Optional] string durationIDs, [Optional] string media, [Optional] string objIDs, [Optional] string monCoordIDs, [Optional] string orgID,
            [Optional] string parameters, [Optional] string resComps, [Optional] string states, [Optional] string statusIDs)
        {
            List<FilteredProject> entities = new List<FilteredProject>();
            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {                   
                    // create where statement to pass
                    string wherePart = "";
                    if (!string.IsNullOrEmpty(durationIDs))
                    {
                        if (wherePart == "")
                            wherePart += "p.proj_duration_id = ANY('{" + durationIDs + "}'::int[])";
                        else
                            wherePart += " AND p.proj_duration_id = ANY('{" + durationIDs + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(lakes))
                    {
                        if (wherePart == "")
                            wherePart += "s.lake_type_id = ANY('{" + lakes + "}'::int[])";
                        else
                            wherePart += " AND s.lake_type_id = ANY('{" + lakes + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(media))
                    {
                        if (wherePart == "")
                            wherePart += "sm.media_type_id = ANY('{" + media + "}'::int[])";
                        else
                            wherePart += " AND sm.media_type_id = ANY('{" + media + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(objIDs))
                    {
                        if (wherePart == "")
                            wherePart += "po.objective_id = ANY('{" + objIDs + "}'::int[])";
                        else
                            wherePart += " AND po.objective_id = ANY('{" + objIDs + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(monCoordIDs))
                    {
                        if (wherePart == "")
                            wherePart += "pm.monitoring_coordination_id = ANY('{" + monCoordIDs + "}'::int[])";
                        else
                            wherePart += " AND pm.monitoring_coordination_id = ANY('{" + monCoordIDs + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(parameters))
                    {
                        if (wherePart == "")
                            wherePart += "sp.parameter_type_id = ANY('{" + parameters + "}'::int[])";
                        else
                            wherePart += " AND sp.parameter_type_id = ANY('{" + parameters + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(resComps))
                    {
                        if (wherePart == "")
                            wherePart += "sr.resource_type_id = ANY('{" + resComps + "}'::int[])";
                        else
                            wherePart += " AND sr.resource_type_id = ANY('{" + resComps + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(states))
                    {
                        if (wherePart == "")
                            wherePart += "s.state_province = ANY('{" + states + "}'::text[])";
                        else
                            wherePart += " AND s.state_province = ANY('{" + states + "}'::text[])";
                    }
                    if (!string.IsNullOrEmpty(statusIDs))
                    {
                        if (wherePart == "")
                            wherePart += "p.proj_status_id = ANY('{" + statusIDs + "}'::int[])";
                        else
                            wherePart += " AND p.proj_status_id = ANY('{" + statusIDs + "}'::int[])";
                    }
                    if (!string.IsNullOrEmpty(orgID))
                    {
                        if (Convert.ToInt32(orgID) > 0)
                        {
                            //sites where project_cooperators have the org NAME that is this OrgID passed
                            List<Int32> _orgs = new List<Int32>();
                            string orgsysIds = "";
                            Int32 _orgId = Convert.ToInt32(orgID);
                            List<organization_system> orgSystemsWithThisOrg = sa.Select<organization_system>().Where(b => b.org_id == _orgId).ToList();
                            //add all the ids to the list of dec
                            orgSystemsWithThisOrg.ForEach(no => _orgs.Add(Convert.ToInt32(no.organization_system_id)));
                            orgsysIds = string.Join(",", _orgs);

                            if (wherePart == "")
                                wherePart += "pc.organization_system_id = ANY('{" + orgsysIds + "}'::int[])";
                            else
                                wherePart += " AND pc.organization_system_id = ANY('{" + orgsysIds + "}'::int[])";
                        }
                    }


                    List<Int32> projIDs = sa.getTable<project_list>(new Object[1] { wherePart }).Select(p => p.project_id).ToList();
                                          

                    entities = sa.Select<project>().Include(p=> p.sites).Include("sites.lake_type").Where(p => projIDs.Contains(p.project_id))
                        .Include(e => e.project_cooperators).Include("project_cooperators.organization_system")
                        .Include("project_cooperators.organization_system.organization").Select(p => new FilteredProject()
                    {
                        name = p.name,
                        project_id = p.project_id,
                        Organizations = p.project_cooperators.Where(pc => pc.organization_system_id > 0).Select(c =>
                            c.organization_system.organization.organization_name
                        ),
                        projectSites = p.sites.Select(s => new SimpleSite()
                        {
                            site_id = s.site_id,
                            latitude = s.latitude,
                            longitude = s.longitude,
                            name = s.name,
                            project_id = s.project_id.Value,
                            Lake = s.lake_type.lake,
                            State = s.state_province,
                            Country = s.country
                        }).ToList()
                    }).ToList();                                      

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
        
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole})]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(project anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || anEntity.proj_duration_id <= 0 || anEntity.proj_status_id <= 0)
                    throw new BadRequestException("Invalid input parameters");

                SiGLGPServiceAgent gpAgent = null;
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        if (anEntity.data_manager_id <= 0)
                        {
                            data_manager user = sa.Select<data_manager>().FirstOrDefault(u => String.Equals(u.username.ToUpper(), Context.User.Identity.Name.ToUpper()));
                            anEntity.data_manager_id = user.data_manager_id;
                        }
                        anEntity.created_stamp = DateTime.Now.Date; anEntity.last_edited_stamp = DateTime.Now.Date;
                        anEntity = sa.Add<project>(anEntity);
                        sm(sa.Messages);

                        //flagged ready
                        gpAgent = new SiGLGPServiceAgent();
                        List<FullSite> projSites = anEntity.sites.Select(x => new FullSite(x)).ToList();

                        if (projSites.Count > 0)
                        {
                            //there's sites, now see if we are adding them or removing them
                            if (anEntity.ready_flag != null)
                            {
                                if (anEntity.ready_flag > 0)
                                {
                                    //remove then add
                         //           gpAgent.DELETESite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                         //           gpAgent.POSTSite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                                }
                                else
                                {
                                    //ready flag is 0 - remove them 
                         //           gpAgent.DELETESite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                                }
                            }
                        }
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
        public OperationResult Put(Int32 entityId, project anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || anEntity.proj_duration_id <= 0 || anEntity.proj_status_id <= 0)
                    throw new BadRequestException("Invalid input parameters");

                SiGLGPServiceAgent gpAgent = null;
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project objectToBeUpdated = sa.Select<project>().Include(p => p.data_manager).SingleOrDefault(p => p.project_id == entityId);
                        if (!IsAuthorizedToEdit(objectToBeUpdated.data_manager.username))
                            return new OperationResult.Forbidden { Description = "Not Authorized" };

                        objectToBeUpdated.name = anEntity.name;
                        objectToBeUpdated.start_date = anEntity.start_date;
                        objectToBeUpdated.end_date = anEntity.end_date;
                        objectToBeUpdated.url = anEntity.url;
                        objectToBeUpdated.additional_info = anEntity.additional_info;
                        objectToBeUpdated.data_manager_id = anEntity.data_manager_id;
                        objectToBeUpdated.science_base_id = anEntity.science_base_id;
                        objectToBeUpdated.description = anEntity.description;
                        objectToBeUpdated.proj_status_id = anEntity.proj_status_id;
                        objectToBeUpdated.proj_duration_id = anEntity.proj_duration_id;
                        objectToBeUpdated.ready_flag = anEntity.ready_flag;
                        objectToBeUpdated.created_stamp = anEntity.created_stamp;
                        objectToBeUpdated.last_edited_stamp = DateTime.Now.Date;

                        anEntity = sa.Update<project>(entityId, objectToBeUpdated);
                        sm(sa.Messages);

                        //flagged ready
                        gpAgent = new SiGLGPServiceAgent();
                        List<FullSite> projSites = objectToBeUpdated.sites.Select(x => new FullSite(x)).ToList();

                        if (projSites.Count > 0)
                        {
                            //there's sites, now see if we are adding them or removing them
                            if (objectToBeUpdated.ready_flag != null)
                            {
                                if (objectToBeUpdated.ready_flag > 0)
                                {
                                    // remove then add
                       //             gpAgent.DELETESite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                       //             gpAgent.POSTSite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                                }
                                else
                                {
                                    //ready flag is 0 - remove them 
                        //            gpAgent.DELETESite(ConfigurationManager.AppSettings["AGSSiglUpdate"], projSites);
                                }
                            }
                        }

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
        [SiGLRequiresRole(new string[]{AdminRole, ManagerRole})]
        [HttpOperation(HttpMethod.DELETE)]
        public OperationResult Delete(Int32 entityId)
        {
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                
                SiGLGPServiceAgent gpAgent = null;
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project anEntity = sa.Select<project>().SingleOrDefault(p => p.project_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();

                      ///  data_manager dm = anEntity.data_manager;
                        //data_manager dataM = sa.Select<data_manager>().First(d => d.data_manager_id == dm);
                      //  if (!IsAuthorizedToEdit(dm.username)) return new OperationResult.Forbidden { Description = "Not Authorized" };

                        sa.Delete<project>(anEntity);
                        sa.Select<project_contacts>().Where(c => c.project_id == entityId).ToList().ForEach(x => sa.Delete<project_contacts>(x));
                        sa.Select<project_cooperators>().Where(coop => coop.project_id == entityId).ToList().ForEach(x => sa.Delete<project_cooperators>(x));
                        sa.Select<project_keywords>().Where(k => k.project_id == entityId).ToList().ForEach(x => sa.Delete<project_keywords>(x));
                        sa.Select<project_objectives>().Where(o => o.project_id == entityId).ToList().ForEach(x => sa.Delete<project_objectives>(x));
                        sa.Select<project_publication>().Where(p => p.project_id == entityId).ToList().ForEach(x => sa.Delete<project_publication>(x));
                        
                     //   sa.Select<site>().Where(s => s.project_id == entityId).ToList().ForEach(x => sa.Delete<site>(x));
                        //delete all sites for this project
                        List<site> projSites = sa.Select<site>().Where(s => s.project_id == entityId).ToList();
                        if (projSites.Count >= 1)
                        {
                            //first Delete to gpservices
                            gpAgent = new SiGLGPServiceAgent();
                            List<FullSite> fullSites = projSites.Select(x => new FullSite(x)).ToList();
                     //       gpAgent.DELETESite(ConfigurationManager.AppSettings["AGSSiglUpdate"], fullSites);
                        }
                        sa.Select<site>().Where(s => s.project_id == entityId).ToList().ForEach(x => sa.Delete<site>(x));
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE

        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.DELETE)]//projectResource + "/{scienceBaseId}/ClearParts").Named("ClearProjectParts")
        public OperationResult ClearProjectParts(string scienceBaseId)
        {
            try
            {
                if (string.IsNullOrEmpty(scienceBaseId)) throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        project anEntity = sa.Select<project>().SingleOrDefault(p => p.science_base_id.ToUpper() == scienceBaseId.ToUpper());
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();
                        Int32 projId = anEntity.project_id;
                        
                        sa.Select<project_contacts>().Where(c => c.project_id == projId).ToList().ForEach(x => sa.Delete<project_contacts>(x));
                        sa.Select<project_cooperators>().Where(coop => coop.project_id == projId).ToList().ForEach(x => sa.Delete<project_cooperators>(x));
                        sa.Select<project_keywords>().Where(k => k.project_id == projId).ToList().ForEach(x => sa.Delete<project_keywords>(x));
                        sa.Select<project_objectives>().Where(o => o.project_id == projId).ToList().ForEach(x => sa.Delete<project_objectives>(x));
                        sa.Select<project_publication>().Where(p => p.project_id == projId).ToList().ForEach(x => sa.Delete<project_publication>(x));
                        
                        sm(sa.Messages);
                    }//end using
                }//end using
                return new OperationResult.OK { Description = this.MessageString };
            }
            catch (Exception ex)
            { return HandleException(ex); }
        }//end HTTP.DELETE

        [SiGLRequiresRole(AdminRole)]
        [HttpOperation(HttpMethod.DELETE, ForUriName = "DeleteAllDMProjects")]
        public OperationResult DeleteAllDMProjects(Int32 dataManagerId)
        {
            List<project> entities = null;
            try
            {
                if (dataManagerId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword, true))
                    {
                        entities = sa.Select<project>().Where(p => p.data_manager_id == dataManagerId).ToList();

                        foreach (project e in entities)
                        {
                            e.project_contacts.Clear();
                            e.project_cooperators.Clear();
                            e.project_keywords.Clear();
                            e.project_objectives.Clear();
                            e.project_publication.Clear();
                            e.sites.Clear();
                        }

                        foreach (project proj in entities)
                        {
                            sa.Delete<project>(proj);
                        }
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