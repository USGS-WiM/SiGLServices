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
// 07.13.16 - TR - Created
#endregion

using OpenRasta.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Runtime.InteropServices;
using SiGLServices.Utilities.ServiceAgent;
using System.Text;
using MoreLinq;
using SiGLDB;
using WiM.Exceptions;
using WiM.Resources;
using OpenRasta.Security;
using WiM.Security;
using SiGLServices.Security;
using SiGLServices.Resources;


namespace SiGLServices.Handlers
{
    public class SiteHandler : SiGLHandlerBase
    {
        #region GetMethods
        [HttpOperation(HttpMethod.GET, ForUriName = "GetAllSites")]
        public OperationResult Get()
        {
            List<site> entities = null;

            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<site>().OrderBy(e => e.site_id).ToList();

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
            site anEntity = null;

            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
       
                using (SiGLAgent sa = new SiGLAgent())
                {
                    anEntity = sa.Select<site>().FirstOrDefault(e => e.site_id == entityId);
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
       
        [HttpOperation(HttpMethod.GET, ForUriName = "GetFullSite")]
        public OperationResult GetFullSite(Int32 siteId)
        {
            FullSite anEntity = null;
            IQueryable<site> query = null;
            try
            {
                if (siteId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    query = sa.Select<site>().Include(s => s.lake_type).Include(s => s.status_type)
                        .Include(s => s.site_resource).Include("site_resource.resource_type")
                        .Include(s => s.site_media).Include("site_media.media_type")
                        .Include(s => s.site_frequency).Include("site_frequency.frequency_type")
                        .Include(s => s.site_parameters).Include("site_parameters.parameter_type").Where(s => s.site_id == siteId);

                    anEntity = query.AsEnumerable().Select(s => new FullSite 
                    {
                        SiteId = s.site_id,
                        Name = s.name,
                        latitude = s.latitude,
                        longitude = s.longitude,
                        State = s.state_province,
                        Country = s.country,
                        lake_type_id = s.lake_type_id,
                        Lake = s.lake_type.lake,
                        Waterbody = s.waterbody,
                        Watershed = s.watershed_huc8,
                        Description = s.description,
                        StartDate = s.start_date != null ? s.start_date.Value.ToString() : "",
                        EndDate = s.end_date != null ? s.end_date.Value.ToString() : "",
                        status_type_id = s.status_type_id,
                        Status = s.status_type_id > 0 ? s.status_type.status : "",
                        SamplePlatform = s.sample_platform,
                        AdditionalInfo = s.additional_info,
                        url = s.url,
                        Resources = s.site_resource.Select(rt => new resource_type
                            {
                                resource_type_id = rt.resource_type.resource_type_id,
                                resource_name = rt.resource_type.resource_name 
                            }).ToList(),
                        Media = s.site_media.Select(mt => new media_type
                        {
                            media_type_id = mt.media_type.media_type_id,
                            media = mt.media_type.media
                        }).ToList(),
                        Frequencies = s.site_frequency.Select(ft => new frequency_type
                        {
                            frequency_type_id = ft.frequency_type.frequency_type_id,
                            frequency = ft.frequency_type.frequency
                        }).ToList(),
                        Parameters = s.site_parameters.Select(pt => new parameter_type
                        {
                            parameter_type_id = pt.parameter_type.parameter_type_id,
                            parameter_group = pt.parameter_type.parameter_group,
                            parameter = pt.parameter_type.parameter
                        }).ToList()
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectSites")]
        public OperationResult GetProjectSites(Int32 projectId)
        {
            List<site> entities = null;

            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<project>().Include(p => p.sites).SingleOrDefault(p => p.project_id == projectId).sites.ToList();
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
 
        [HttpOperation(HttpMethod.GET, ForUriName = "GetProjectFullSites")]
        public OperationResult GetProjectFullSites(Int32 projectId)
        {
            List<FullSite> entities = null;
            IQueryable<site> query = null;
            try
            {
                if (projectId <= 0) throw new BadRequestException("Invalid input parameters");

                using (SiGLAgent sa = new SiGLAgent())
                {
                    query = sa.Select<site>().Include(s => s.lake_type).Include(s => s.status_type)
                        .Include(s => s.site_resource).Include("site_resource.resource_type")
                        .Include(s => s.site_media).Include("site_media.media_type")
                        .Include(s => s.site_frequency).Include("site_frequency.frequency_type")
                        .Include(s => s.site_parameters).Include("site_parameters.parameter_type").Where(s => s.project_id == projectId);

            
                    entities = query.AsEnumerable().Select(s => new FullSite 
                    {
                        SiteId = s.site_id,
                        Name = s.name,
                        latitude = s.latitude,
                        longitude = s.longitude,
                        State = s.state_province,
                        Country = s.country,
                        lake_type_id = s.lake_type_id,
                        Lake = s.lake_type.lake,
                        Waterbody = s.waterbody,
                        Watershed = s.watershed_huc8,
                        Description = s.description,
                        StartDate = s.start_date != null ? s.start_date.Value.ToString() : "",
                        EndDate = s.end_date != null ? s.end_date.Value.ToString() : "",
                        status_type_id = s.status_type_id,
                        Status = s.status_type_id > 0 ? s.status_type.status : "",
                        SamplePlatform = s.sample_platform,
                        AdditionalInfo = s.additional_info,
                        url = s.url,
                        Resources = s.site_resource.Select(rt => new resource_type
                            {
                                resource_type_id = rt.resource_type.resource_type_id,
                                resource_name = rt.resource_type.resource_name 
                            }).OrderBy(rt=>rt.resource_name).ToList(),
                        Media = s.site_media.Select(mt => new media_type
                        {
                            media_type_id = mt.media_type.media_type_id,
                            media = mt.media_type.media
                        }).OrderBy(mt=>mt.media).ToList(),
                        Frequencies = s.site_frequency.Select(ft => new frequency_type
                        {
                            frequency_type_id = ft.frequency_type.frequency_type_id,
                            frequency = ft.frequency_type.frequency
                        }).OrderBy(ft=>ft.frequency).ToList(),
                        Parameters = s.site_parameters.Select(pt => new parameter_type
                        {
                            parameter_type_id = pt.parameter_type.parameter_type_id,
                            parameter_group = pt.parameter_type.parameter_group,
                            parameter = pt.parameter_type.parameter
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
        }//end HttpMethod.GET
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetLakeSites")]
        public OperationResult GetLakeSites(Int32 lakeId)
        {
            List<site> entities = null;

            try
            {
                if (lakeId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<lake_type>().Include(l => l.sites).SingleOrDefault(l => l.lake_type_id == lakeId).sites.ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetStatusSites")]
        public OperationResult GetStatusSites(Int32 statusId)
        {
            List<site> entities = null;

            try
            {
                if (statusId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<status_type>().Include(st => st.sites).SingleOrDefault(st => st.status_id == statusId).sites.ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetMediaSites")]
        public OperationResult GetMediaSites(Int32 mediaId)
        {
            List<site> entities = null;

            try
            {
                if (mediaId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<site_media>().Include(st => st.site).Where(st => st.media_type_id == mediaId).Select(cm=>cm.site).ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetResourceSites")]
        public OperationResult GetResourceSites(Int32 resourceTypeId)
        {
            List<site> entities = null;

            try
            {
                if (resourceTypeId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<site_resource>().Include(st => st.site).Where(st => st.resource_type_id == resourceTypeId).Select(cm => cm.site).ToList();
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
        
        [HttpOperation(HttpMethod.GET, ForUriName = "GetParameterSites")]
        public OperationResult GetParameterSites(Int32 parameterId)
        {
            List<site> entities = null;

            try
            {
                if (parameterId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<site_parameters>().Include(st => st.site).Where(st => st.parameter_type_id == parameterId).Select(cm => cm.site).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetFrequencySites")]
        public OperationResult GetFrequencySites(Int32 frequencyId)
        {
            List<site> entities = null;

            try
            {
                if (frequencyId <= 0) throw new BadRequestException("Invalid input parameters");
                using (SiGLAgent sa = new SiGLAgent())
                {
                    entities = sa.Select<site_frequency>().Include(st => st.site).Where(st => st.frequency_type_id == frequencyId).Select(cm => cm.site).ToList();
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

        [HttpOperation(HttpMethod.GET, ForUriName = "GetFilteredSites")]
        public OperationResult GetFilteredSites([Optional] string lakes, [Optional] string durationIDs, [Optional] string media, [Optional] string objIDs, [Optional] string monCoordIDs, [Optional] string orgID,
            [Optional] string parameters, [Optional] string resComps, [Optional] string states, [Optional] string statusIDs)
        {
            List<site> entities = null;
            try
            {
                char[] delimiterChar = { ',' };

                List<Int32> _durationIds = !string.IsNullOrEmpty(durationIDs) ? durationIDs.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _lakeIds = !string.IsNullOrEmpty(lakes) ? lakes.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _mediaIds = !string.IsNullOrEmpty(media) ? media.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _objectivesIds = !string.IsNullOrEmpty(objIDs) ? objIDs.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _monCoordIds = !string.IsNullOrEmpty(monCoordIDs) ? monCoordIDs.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _parameterIds = !string.IsNullOrEmpty(parameters) ? parameters.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<Int32> _resourceIds = !string.IsNullOrEmpty(resComps) ? resComps.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                List<string> _stateList = !string.IsNullOrEmpty(states) ? states.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).ToList() : null;
                List<Int32> _statusIds = !string.IsNullOrEmpty(statusIDs) ? statusIDs.Split(delimiterChar, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList() : null;
                
                using (SiGLAgent sa = new SiGLAgent())
                {
                    IQueryable<site> query;

                    //query = sa.Select<site>();
                    query = sa.Select<site>().Include(s => s.project).Include(s => s.site_media).Include("project.project_objectives").Include("project.project_monitor_coord")
                        .Include("project.project_cooperators").Include(s => s.site_parameters).Include(s => s.site_resource);

                    if (_durationIds != null && _durationIds.Count > 0)
                        query = query.Where(s => s.project != null && _durationIds.Contains(s.project.proj_duration_id.Value));


                    if (_lakeIds != null && _lakeIds.Count > 0)
                        query = query.Where(s => _lakeIds.Any(lt => lt == s.lake_type_id));

                    if (_mediaIds != null && _mediaIds.Count > 0) 
                        query = query.Where(s => s.site_media.Any(sp => _mediaIds.Contains(sp.media_type_id)));

                    if (_objectivesIds != null && _objectivesIds.Count > 0)
                        query = query.Where(s => s.project != null && s.project.project_objectives.Any(a => _objectivesIds.Contains(a.objective_id.Value)));

                    if (_monCoordIds != null && _monCoordIds.Count > 0)
                        query = query.Where(s => s.project != null && s.project.project_monitor_coord.Any(a => _monCoordIds.Contains(a.monitoring_coordination_id.Value)));

                    if (!string.IsNullOrEmpty(orgID))
                    {
                        if (Convert.ToInt32(orgID) > 0)
                        {
                            //sites where project_cooperators have the org NAME that is this OrgID passed
                            List<Int32> _orgs = new List<Int32>();
                            Int32 _orgId = Convert.ToInt32(orgID);
                            List<organization_system> orgSystemsWithThisOrg = sa.Select<organization_system>().Where(b => b.org_id == _orgId).ToList();
                            //add all the ids to the list of dec
                            orgSystemsWithThisOrg.ForEach(no => _orgs.Add(Convert.ToInt32(no.organization_system_id)));
                            query = query.Where(s => s.project.project_cooperators.Any(a => _orgs.Contains(a.organization_system_id.Value)));
                        }
                    }

                    if (_parameterIds != null && _parameterIds.Count > 0)
                        query = query.Where(s => s.site_parameters.Any(a => _parameterIds.Contains(a.parameter_type_id)));  

                    if (_resourceIds != null && _resourceIds.Count > 0)
                        query = query.Where(s => s.site_resource.Any(a => _resourceIds.Contains(a.resource_type_id)));

                    if (_stateList != null && _stateList.Count > 0)
                        query = query.Where(s => _stateList.Any(x => x == s.state_province));

                    if (_statusIds != null && _statusIds.Count > 0)
                        query = query.Where(s => s.project != null && _statusIds.Contains(s.project.proj_status_id.Value));                                                            

                    entities = query.ToList();
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
        
       // .AtUri(siteResource + "/StatesWithSites").Named("GetStatesThatHaveSites")
        [HttpOperation(HttpMethod.GET, ForUriName = "GetStatesThatHaveSites")]
        public OperationResult GetStatesThatHaveSites()
        {
            List<string> stateNames = null;
            
            try
            {
                using (SiGLAgent sa = new SiGLAgent())
                {
                    stateNames = sa.Select<site>().DistinctBy(x => x.state_province).OrderBy(x=> x.state_province).Select(p => p.state_province).ToList();

                    sm(MessageType.info, "Count: " + stateNames.Count());
                    sm(sa.Messages);

                }//end using
                return new OperationResult.OK { ResponseResource = stateNames, Description = this.MessageString };
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }//end HttpMethod.GET

        //not sure that I really need this (going to make a getFullSite instead, using a siteResource)
        //[HttpOperation(HttpMethod.GET, ForUriName = "GetSitesView")]
        //public OperationResult GetSitesView()
        //{
        //    List<site_list_view> entities = null;
        //    try
        //    {
        //        using (SiGLAgent sa = new SiGLAgent())
        //        {
        //            entities = sa.getTable<site_list_view>(new Object[1] { null }).ToList();
        //            sm(MessageType.info, "Count: " + entities.Count());
        //            sm(sa.Messages);
        //        }//end using
                
        //        return new OperationResult.OK { ResponseResource = entities, Description = this.MessageString };
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}//end HttpMethod.GET
    
        #endregion
        
        #region PostMethods
        /// 
        /// Force the user to provide authentication and authorization 
        ///
        [SiGLRequiresRole(new string[] { AdminRole, ManagerRole })]
        [HttpOperation(HttpMethod.POST)]
        public OperationResult POST(site anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || anEntity.latitude < 0 || anEntity.longitude > 0 || string.IsNullOrEmpty(anEntity.country)
                    || !anEntity.project_id.HasValue || string.IsNullOrEmpty(anEntity.state_province) || !anEntity.lake_type_id.HasValue)
                    throw new BadRequestException("Invalid input parameters");

                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Add<site>(anEntity);
                        project aProj = sa.Select<project>().First(p => p.project_id == anEntity.project_id);
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);
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
        public OperationResult Put(Int32 entityId, site anEntity)
        {
            try
            {
                if (string.IsNullOrEmpty(anEntity.name) || anEntity.latitude < 0 || anEntity.longitude > 0 || string.IsNullOrEmpty(anEntity.country)
                    || anEntity.project_id < 0 || string.IsNullOrEmpty(anEntity.state_province) || !anEntity.lake_type_id.HasValue)
                    throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        //no time please
                        if (anEntity.start_date.HasValue) anEntity.start_date = anEntity.start_date.Value.Date;
                        if (anEntity.end_date.HasValue) anEntity.end_date = anEntity.end_date.Value.Date;

                        anEntity = sa.Update<site>(entityId, anEntity);

                        project aProj = sa.Select<project>().First(p => p.project_id == anEntity.project_id);
                        aProj.last_edited_stamp = DateTime.Now.Date;
                        sa.Update<project>(aProj);
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
        [SiGLRequiresRole(new string[] {AdminRole, ManagerRole})]
        [HttpOperation(HttpMethod.DELETE)]
        public OperationResult Delete(Int32 entityId)
        {
            site anEntity = null;
            Int32 projId = 0;
            try
            {
                if (entityId <= 0) throw new BadRequestException("Invalid input parameters");
                using (EasySecureString securedPassword = GetSecuredPassword())
                {
                    using (SiGLAgent sa = new SiGLAgent(username, securedPassword))
                    {
                        anEntity = sa.Select<site>().SingleOrDefault(s => s.site_id == entityId);
                        if (anEntity == null) throw new WiM.Exceptions.NotFoundRequestException();
                        projId = anEntity.project_id.Value;

                        sa.Delete<site>(anEntity);

                        sa.Select<site_frequency>().Where(freq => freq.site_id == entityId).ToList().ForEach(x=> sa.Delete<site_frequency>(x));
                        sa.Select<site_media>().Where(med => med.site_id == entityId).ToList().ForEach(x => sa.Delete<site_media>(x));
                        sa.Select<site_parameters>().Where(pa => pa.site_id == entityId).ToList().ForEach(x => sa.Delete<site_parameters>(x));
                        sa.Select<site_resource>().Where(res => res.site_id == entityId).ToList().ForEach(x => sa.Delete<site_resource>(x));

                        project aProj = sa.Select<project>().First(p => p.project_id == projId);
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