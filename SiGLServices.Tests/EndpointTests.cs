using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiGLDB;
using WiM.Test;
using WiM.Hypermedia;
using SiGLServices;
using SiGLServices.Resources;

namespace SiGLServices.Test
{
    [TestClass]
    public class EndpointTests : EndpointTestBase
    {
        #region Private Fields
        private string host = "http://localhost/";
        private string basicAuth = "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                .GetBytes("lampadmin:adminPa55word"));


        #endregion
        #region Constructor
        public EndpointTests() : base(new Configuration()) { }
        #endregion
        #region Test Methods
        [TestMethod]
        public void ContactRequest()
        {
            //GET LIST
            List<contact> RequestList = this.GETRequest<List<contact>>(host + Configuration.contactResource, basicAuth);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());            
            
            //GET project contacts
            List<contact> projRequestList = this.GETRequest<List<contact>>(host + Configuration.projectResource + "/685/" + Configuration.contactResource);
            Assert.IsNotNull(projRequestList);

            //GET orgSystem contacts
            List<contact> OrgSysRequestList = this.GETRequest<List<contact>>(host + Configuration.orgSystemResource + "/15/" + Configuration.contactResource);
            Assert.IsNotNull(OrgSysRequestList);

            //POST
            contact postObj;
            postObj = this.POSTRequest<contact>(host + Configuration.contactResource, new contact() {  name = "post-test", organization_system_id = 1, email = "postTest@post.test",phone="(123) 345-5678" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.contact_id.ToString());

            //GET POSTed item
            contact RequestObj = this.GETRequest<contact>(host + Configuration.contactResource + "/" + postObj.contact_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //PUT POSTed item
            postObj.name = "put-test"; postObj.email = "putTest@put.test";
            contact putObj = this.PUTRequest<contact>(host + Configuration.contactResource + "/" + postObj.contact_id, postObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<contact>(host + Configuration.contactResource + "/" + postObj.contact_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void DataManagerRequest()
        {
            //GET LIST
            List<data_manager> RequestList = this.GETRequest<List<data_manager>>(host + Configuration.dataManagerResource, basicAuth);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());
                        
            //GET dm_list_view
            List<dm_list_view> dmListView = this.GETRequest<List<dm_list_view>>(host + Configuration.dataManagerResource + "/DMList", basicAuth);
            Assert.IsNotNull(dmListView);

            //POST
            data_manager postObj;
            postObj = this.POSTRequest<data_manager>(host + Configuration.dataManagerResource, new data_manager()
            {
                username = "newUser",
                fname = "New",
                lname = "user",
                phone = "(123) 456-7890",
                email = "test@usgs.gov",
                organization_system_id = 1,
                password = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("SiGLDef@u1t"))
            }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.data_manager_id.ToString());

            //GET POSTed item
            data_manager RequestObj = this.GETRequest<data_manager>(host + Configuration.dataManagerResource + "/1");
            Assert.IsNotNull(RequestObj);

            //GET login
            data_manager loginM = this.GETRequest<data_manager>(host + "/login", basicAuth);
            Assert.IsNotNull(loginM);

            //GET  GetProjectDataManager 
            data_manager projDM = this.GETRequest<data_manager>(host + Configuration.projectResource + "/7/dataManager", basicAuth);
            Assert.IsNotNull(projDM);            

            //PUT POSTed item
            RequestObj.password = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("adminPa55word"));
            data_manager putObj = this.PUTRequest<data_manager>(host + Configuration.dataManagerResource + "/" + RequestObj.data_manager_id, RequestObj, basicAuth);
            Assert.IsNotNull(putObj);

            //Delete POSTed item
            bool success = this.DELETERequest<data_manager>(host + Configuration.dataManagerResource + "/" + postObj.data_manager_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void DataHostRequest()
        {
            //GET LIST
            List<data_host> RequestList = this.GETRequest<List<data_host>>(host + Configuration.dataHostResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET project data hosts
            List<data_host> projRequestList = this.GETRequest<List<data_host>>(host + Configuration.projectResource + "/2023/" + Configuration.dataHostResource);
            Assert.IsNotNull(projRequestList);
            
            //POST
            List<data_host> postObj;
            postObj = this.POSTRequest<data_host, List<data_host>>(host + Configuration.dataHostResource, new data_host() { host_name = "post-test", project_id = 2023 }, basicAuth);
            Assert.IsNotNull(postObj, postObj.Count.ToString());

            //GET POSTed item
            data_host RequestObj = this.GETRequest<data_host>(host + Configuration.dataHostResource + "/" + postObj[0].data_host_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //PUT POSTed item
            postObj[0].host_name = "put-test";
            data_host putObj = this.PUTRequest<data_host>(host + Configuration.dataHostResource + "/" + postObj[0].data_host_id, postObj[0], basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<data_host>(host + Configuration.dataHostResource + "/" + postObj[0].data_host_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void DivisionRequest()
        {
            //GET LIST
            List<division> RequestList = this.GETRequest<List<division>>(host + Configuration.divisionResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            division postObj;
            postObj = this.POSTRequest<division>(host + Configuration.divisionResource, new division() { division_name = "post-test", org_id = 1 }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.division_id.ToString());

            //GET POSTed item
            division RequestObj = this.GETRequest<division>(host + Configuration.divisionResource + "/" + postObj.division_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //PUT POSTed item
            postObj.division_name = "put-test";
            division putObj = this.PUTRequest<division>(host + Configuration.divisionResource + "/" + postObj.division_id, postObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<division>(host + Configuration.divisionResource + "/" + postObj.division_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void FrequencyRequest()
        {
            //GET LIST
            List<frequency_type> RequestList = this.GETRequest<List<frequency_type>>(host + Configuration.frequencyResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET site Frequencies
            List<frequency_type> siteFreqList = this.GETRequest<List<frequency_type>>(host + Configuration.siteResource + "/1638/" + Configuration.frequencyResource);
            Assert.IsNotNull(siteFreqList, siteFreqList.Count.ToString());
            
            //POST
            frequency_type postObj;
            postObj = this.POSTRequest<frequency_type>(host + Configuration.frequencyResource , new frequency_type() { frequency = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.frequency_type_id.ToString());

            //GET POSTed item
            frequency_type RequestObj = this.GETRequest<frequency_type>(host + Configuration.frequencyResource + "/80", basicAuth);
            Assert.IsNotNull(RequestObj);

            //POST siteFrequency
            List<frequency_type> siteFreqResp;
            siteFreqResp = this.POSTRequest<frequency_type, List<frequency_type>>(host + Configuration.siteResource + "/1638/addFrequency?FrequencyTypeId=80", null, basicAuth);
            Assert.IsNotNull(siteFreqResp, siteFreqResp.Count.ToString());

            //PUT POSTed item
            RequestObj.frequency = "put-test";
            frequency_type putObj = this.PUTRequest<frequency_type>(host + Configuration.frequencyResource + "/" + RequestObj.frequency_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item "/Sites/{siteId}/removeNetworkName?NetworkNameId={networkNameId}"
            bool siteFreqsuccess = this.DELETERequest<frequency_type>(host + Configuration.siteResource + "/1638/removeFrequencyType?FrequencyTypeId=80", basicAuth);
            Assert.IsTrue(siteFreqsuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<frequency_type>(host + Configuration.frequencyResource + "/80", basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void KeywordRequest()
        {
            //GET LIST
            List<keyword> RequestList = this.GETRequest<List<keyword>>(host + Configuration.keywordResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET project keywords
            List<keyword> projKeywordList = this.GETRequest<List<keyword>>(host + Configuration.projectResource + "/88/" + Configuration.keywordResource);
            Assert.IsNotNull(projKeywordList, projKeywordList.Count.ToString());

            //POST
            keyword postObj;
            postObj = this.POSTRequest<keyword>(host + Configuration.keywordResource, new keyword() { term = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.keyword_id.ToString());

            //GET POSTed item
            keyword RequestObj = this.GETRequest<keyword>(host + Configuration.keywordResource + "/" + postObj.keyword_id);
            Assert.IsNotNull(RequestObj);

            //GET item by term
            keyword TermObj = this.GETRequest<keyword>(host + Configuration.keywordResource + "?term=post-test");
            Assert.IsNotNull(TermObj);

            //POST projKeyword
            List<keyword> projKeywordResp;
            projKeywordResp = this.POSTRequest<keyword, List<keyword>>(host + Configuration.projectResource + "/88/addKeyword?Word=post-test", null, basicAuth);
            Assert.IsNotNull(projKeywordResp, projKeywordResp.Count.ToString());

            //POST projKeyword
            List<keyword> projKeyword2Resp;
            projKeyword2Resp = this.POSTRequest<keyword, List<keyword>>(host + Configuration.projectResource + "/88/addKeyword?Word=test123456", null, basicAuth);
            Assert.IsNotNull(projKeyword2Resp, projKeyword2Resp.Count.ToString());

            //PUT POSTed item
            RequestObj.term = "put-test";
            keyword putObj = this.PUTRequest<keyword>(host + Configuration.keywordResource + "/" + RequestObj.keyword_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool projKeywordsuccess = this.DELETERequest<keyword>(host + Configuration.projectResource + "/88/removeKeyword?KeywordId=" + RequestObj.keyword_id, basicAuth);
            Assert.IsTrue(projKeywordsuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<keyword>(host + Configuration.keywordResource + "/" + RequestObj.keyword_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void LakeRequest()
        {
            //GET LIST
            List<lake_type> RequestList = this.GETRequest<List<lake_type>>(host + Configuration.lakeResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());
            
            //POST
            lake_type postObj;
            postObj = this.POSTRequest<lake_type>(host + Configuration.lakeResource, new lake_type() { lake = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.lake_type_id.ToString());

            //GET POSTed item
            lake_type RequestObj = this.GETRequest<lake_type>(host + Configuration.lakeResource + "/" + postObj.lake_type_id);
            Assert.IsNotNull(RequestObj);

            //GET site lake
            lake_type siteLake = this.GETRequest<lake_type>(host + Configuration.siteResource + "/1336/lake");
            Assert.IsNotNull(siteLake);            

            //PUT POSTed item
            RequestObj.lake = "put-test";
            lake_type putObj = this.PUTRequest<lake_type>(host + Configuration.lakeResource + "/" + RequestObj.lake_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<lake_type>(host + Configuration.lakeResource + "/" + RequestObj.lake_type_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void MediaRequest()
        {
            //GET LIST
            List<media_type> RequestList = this.GETRequest<List<media_type>>(host + Configuration.mediaResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET site Media
            List<media_type> siteMediaList = this.GETRequest<List<media_type>>(host + Configuration.siteResource + "/1638/" + Configuration.mediaResource);
            Assert.IsNotNull(siteMediaList, siteMediaList.Count.ToString());

            //POST
            media_type postObj;
            postObj = this.POSTRequest<media_type>(host + Configuration.mediaResource, new media_type() { media = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.media_type_id.ToString());

            //GET POSTed item
            media_type RequestObj = this.GETRequest<media_type>(host + Configuration.mediaResource + "/" + postObj.media_type_id);
            Assert.IsNotNull(RequestObj);

            //POST siteFrequency
            List<media_type> sitemediaResp;
            sitemediaResp = this.POSTRequest<media_type, List<media_type>>(host + Configuration.siteResource + "/1638/addMedia?MediaTypeId=" + RequestObj.media_type_id, null, basicAuth);
            Assert.IsNotNull(sitemediaResp, sitemediaResp.Count.ToString());

            //PUT POSTed item
            RequestObj.media = "put-test";
            media_type putObj = this.PUTRequest<media_type>(host + Configuration.mediaResource + "/" + RequestObj.media_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item "/Sites/{siteId}/removeNetworkName?NetworkNameId={networkNameId}"
            bool siteMediasuccess = this.DELETERequest<media_type>(host + Configuration.siteResource + "/1638/removeMediaType?MediaTypeId=" + RequestObj.media_type_id, basicAuth);
            Assert.IsTrue(siteMediasuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<media_type>(host + Configuration.mediaResource + "/" + RequestObj.media_type_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ObjectiveRequest()
        {
            //GET LIST
            List<objective_type> RequestList = this.GETRequest<List<objective_type>>(host + Configuration.objectiveResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET project objectives
            List<objective_type> projObjList = this.GETRequest<List<objective_type>>(host + Configuration.projectResource + "/1607/" + Configuration.objectiveResource);
            Assert.IsNotNull(projObjList, projObjList.Count.ToString());

            //POST
            objective_type postObj;
            postObj = this.POSTRequest<objective_type>(host + Configuration.objectiveResource, new objective_type() { objective = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.objective_type_id.ToString());

            //GET POSTed item
            objective_type RequestObj = this.GETRequest<objective_type>(host + Configuration.objectiveResource + "/" + postObj.objective_type_id);
            Assert.IsNotNull(RequestObj);

            //POST projectObjective
            List<objective_type> projObjResp;
            projObjResp = this.POSTRequest<objective_type, List<objective_type>>(host + Configuration.projectResource + "/1513/addObjective?ObjectiveTypeId=" + RequestObj.objective_type_id, null, basicAuth);
            Assert.IsNotNull(projObjResp, projObjResp.Count.ToString());

            //PUT POSTed item
            RequestObj.objective = "put-test";
            objective_type putObj = this.PUTRequest<objective_type>(host + Configuration.objectiveResource + "/" + RequestObj.objective_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item 
            bool projObjSuccess = this.DELETERequest<objective_type>(host + Configuration.projectResource + "/1607/removeObjective?ObjectiveTypeId=299", basicAuth);
            Assert.IsTrue(projObjSuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<objective_type>(host + Configuration.objectiveResource + "/299", basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void OrganizationRequest()
        {
            //GET LIST
            List<organization> RequestList = this.GETRequest<List<organization>>(host + Configuration.organizationResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            organization postObj;
            postObj = this.POSTRequest<organization>(host + Configuration.organizationResource, new organization() { organization_name = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.organization_id.ToString());

            //GET POSTed item
            organization RequestObj = this.GETRequest<organization>(host + Configuration.organizationResource + "/" + postObj.organization_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //PUT POSTed item
            postObj.organization_name = "put-test";
            organization putObj = this.PUTRequest<organization>(host + Configuration.organizationResource + "/" + postObj.organization_id, postObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<organization>(host + Configuration.organizationResource + "/" + postObj.organization_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void OrganizationSystemRequest()
        {
            //GET LIST of organization_system
            List<organization_system> RequestList = this.GETRequest<List<organization_system>>(host + Configuration.orgSystemResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET LIST of organizationResources (test in browser instead)
            //GET LIST of project OrganizationResources (test in browser)

            //POST (organization_system)
            organization_system postObj;
            postObj = this.POSTRequest<organization_system>(host + Configuration.orgSystemResource, new organization_system() { org_id = 1, div_id = 44, sec_id = 1 }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.organization_system_id.ToString());

            //POST (projectOrganization)
            List<organization_system> postProjOrg;
            postProjOrg = this.POSTRequest<organization_system, List<organization_system>>(host + Configuration.projectResource + "/1/addOrganization?OrganizationId=1&DivisionId=44&SectionId=1", null, basicAuth);
            Assert.IsNotNull(postProjOrg, postProjOrg.Count.ToString());

            //GET POSTed item (organization_system)
            organization_system RequestObj = this.GETRequest<organization_system>(host + Configuration.orgSystemResource + "/" + postObj.organization_system_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //GET item (organizationResource) Test in browser instead         

            // NO PUT            
            // No Delete POSTed item

            //Delete projectCooperator
            bool success = this.DELETERequest<organization_system>(host + Configuration.projectResource + "/removeOrganization?OrgSystemId=" + postProjOrg[0].organization_system_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ParameterRequest()
        {
            //GET LIST
            List<parameter_type> RequestList = this.GETRequest<List<parameter_type>>(host + Configuration.parameterResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET site parameters
            List<parameter_type> siteParamList = this.GETRequest<List<parameter_type>>(host + Configuration.siteResource + "/1301/" + Configuration.parameterResource);
            Assert.IsNotNull(siteParamList, siteParamList.Count.ToString());

            //POST
            parameter_type postObj;
            postObj = this.POSTRequest<parameter_type>(host + Configuration.parameterResource, new parameter_type() { parameter_group = "post-test", parameter="posttest" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.parameter_type_id.ToString());

            //GET POSTed item
            parameter_type RequestObj = this.GETRequest<parameter_type>(host + Configuration.parameterResource + "/" + postObj.parameter_type_id);
            Assert.IsNotNull(RequestObj);

            //GET parametersByGroupName item (Test in browser)
            //ParameterGroups RequestPObj = this.GETRequest<ParameterGroups>(host + Configuration.parameterResource + "?GroupNames=Physical");
            //Assert.IsNotNull(RequestPObj);

            //POST siteParameter
            List<parameter_type> siteParameterResp;
            siteParameterResp = this.POSTRequest<parameter_type, List<parameter_type>>(host + Configuration.siteResource + "/1301/addParameter?ParameterTypeId=" + RequestObj.parameter_type_id, null, basicAuth);
            Assert.IsNotNull(siteParameterResp, siteParameterResp.Count.ToString());
            
            //PUT POSTed item
            RequestObj.parameter = "puttest";
            parameter_type putObj = this.PUTRequest<parameter_type>(host + Configuration.parameterResource + "/" + RequestObj.parameter_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item "/Sites/{siteId}/removeNetworkName?NetworkNameId={networkNameId}"
            bool siteParametersuccess = this.DELETERequest<parameter_type>(host + Configuration.siteResource + "/1301/removeParameterType?ParameterTypeId=" + RequestObj.parameter_type_id, basicAuth);
            Assert.IsTrue(siteParametersuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<parameter_type>(host + Configuration.parameterResource + "/" + RequestObj.parameter_type_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ProjStatusRequest()
        {
            //GET LIST
            List<proj_status> RequestList = this.GETRequest<List<proj_status>>(host + Configuration.projStatusResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            proj_status postObj;
            postObj = this.POSTRequest<proj_status>(host + Configuration.projStatusResource, new proj_status() { status_value = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.proj_status_id.ToString());

            //GET POSTed item
            proj_status RequestObj = this.GETRequest<proj_status>(host + Configuration.projStatusResource + "/" + postObj.proj_status_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //GET project proj_status
            proj_status projStat = this.GETRequest<proj_status>(host + Configuration.projectResource + "/681/projStatus", basicAuth);
            Assert.IsNotNull(projStat);

            //PUT POSTed item
            postObj.status_value = "put-test";
            proj_status putObj = this.PUTRequest<proj_status>(host + Configuration.projStatusResource + "/" + postObj.proj_status_id, postObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<proj_status>(host + Configuration.projStatusResource + "/" + postObj.proj_status_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ProjDurationRequest()
        {
            //GET LIST
            List<proj_duration> RequestList = this.GETRequest<List<proj_duration>>(host + Configuration.projDurationResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            proj_duration postObj;
            postObj = this.POSTRequest<proj_duration>(host + Configuration.projDurationResource, new proj_duration() { duration_value = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.proj_duration_id.ToString());

            //GET POSTed item
            proj_duration RequestObj = this.GETRequest<proj_duration>(host + Configuration.projDurationResource + "/" + postObj.proj_duration_id);
            Assert.IsNotNull(RequestObj);

            //GET project proj_duration
            proj_duration projDuration = this.GETRequest<proj_duration>(host + Configuration.projectResource + "/1512/projDuration");
            Assert.IsNotNull(projDuration);

            //PUT POSTed item
            postObj.duration_value = "put-test";
            proj_duration putObj = this.PUTRequest<proj_duration>(host + Configuration.projDurationResource + "/" + postObj.proj_duration_id, postObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<proj_duration>(host + Configuration.projDurationResource + "/" + postObj.proj_duration_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ProjectRequest()
        {
            //GET LIST GetAllProjects
            List<project> RequestList = this.GETRequest<List<project>>(host + Configuration.projectResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GetContactProjects
            List<project> ContProjList = this.GETRequest<List<project>>(host + Configuration.contactResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(ContProjList, ContProjList.Count.ToString());

            //GetDataManagersProjects
            List<project> DMProjList = this.GETRequest<List<project>>(host + Configuration.dataManagerResource + "/163/" + Configuration.projectResource);
            Assert.IsNotNull(DMProjList, DMProjList.Count.ToString());

            //GetKeyWordProjects
            List<project> KeyProjList = this.GETRequest<List<project>>(host + Configuration.keywordResource + "/10/" + Configuration.projectResource);
            Assert.IsNotNull(KeyProjList, KeyProjList.Count.ToString());

            //GetPublicationProjects
            List<project> PubProjList = this.GETRequest<List<project>>(host + Configuration.publicationResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(PubProjList, PubProjList.Count.ToString());

            //GetObjectiveProjects
            List<project> ObjProjList = this.GETRequest<List<project>>(host + Configuration.objectiveResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(ObjProjList, ObjProjList.Count.ToString());

            //GetFreqSiteProjects
            List<project> SiteFreqProjList = this.GETRequest<List<project>>(host + Configuration.frequencyResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(SiteFreqProjList, SiteFreqProjList.Count.ToString());

            //GetLakeSiteProjects
            List<project> siteLakeProjList = this.GETRequest<List<project>>(host + Configuration.lakeResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(siteLakeProjList, siteLakeProjList.Count.ToString());

            //GetMediaSiteProjects
            List<project> siteMedProjList = this.GETRequest<List<project>>(host + Configuration.mediaResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(siteMedProjList, siteMedProjList.Count.ToString());

            //GetParameterSiteProjects
            List<project> siteParamProjList = this.GETRequest<List<project>>(host + Configuration.parameterResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(siteParamProjList, siteParamProjList.Count.ToString());

            //GetResourceSiteProjects
            List<project> siteResProjList = this.GETRequest<List<project>>(host + Configuration.resourceResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(siteResProjList, siteResProjList.Count.ToString());

            //GetSiteStatusProjects
            List<project> siteStatusProjList = this.GETRequest<List<project>>(host + Configuration.statusResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(siteStatusProjList, siteStatusProjList.Count.ToString());

            //GetOrgSysProjects
            List<project> orgSysProjList = this.GETRequest<List<project>>(host + Configuration.orgSystemResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(orgSysProjList, orgSysProjList.Count.ToString());

            //GetProjectDurationProjects
            List<project> projDurProjList = this.GETRequest<List<project>>(host + Configuration.projDurationResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(projDurProjList, projDurProjList.Count.ToString());

            //GetProjectStatusProjects
            List<project> projStatProjList = this.GETRequest<List<project>>(host + Configuration.projStatusResource + "/1/" + Configuration.projectResource);
            Assert.IsNotNull(projStatProjList, projStatProjList.Count.ToString());

            //GetFlaggedProjects
            List<project> flagProjList = this.GETRequest<List<project>>(host + Configuration.projectResource + "?FlaggedProjects=1");
            Assert.IsNotNull(flagProjList, flagProjList.Count.ToString());

            //GetIndexProjects (project_sitecount_view
            List<project_sitecount_view> indexProjectList = this.GETRequest<List<project_sitecount_view>>(host + Configuration.projectResource + "/IndexProjects", basicAuth);
            Assert.IsNotNull(indexProjectList, indexProjectList.Count.ToString());
            //POST
            project postObj;
            postObj = this.POSTRequest<project>(host + Configuration.projectResource, new project() { name = "post-test", proj_duration_id = 1, proj_status_id=1 }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.project_id.ToString());

            //GET POSTed item
            project RequestObj = this.GETRequest<project>(host + Configuration.projectResource + "/" + postObj.project_id);
            Assert.IsNotNull(RequestObj);

            //GetSiteProject
            project siteProjObj = this.GETRequest<project>(host + Configuration.siteResource + "/1301/project");
            Assert.IsNotNull(siteProjObj);

            //GetDataHostProject
            project dhProjObj = this.GETRequest<project>(host + Configuration.dataHostResource + "/1/project");
            Assert.IsNotNull(dhProjObj);

            //PUT POSTed item
            postObj.name = "put-test";
            project putObj = this.PUTRequest<project>(host + Configuration.projectResource + "/" + postObj.project_id, postObj, basicAuth);
            Assert.IsNotNull(putObj);

            //Delete POSTed item
            bool success = this.DELETERequest<project>(host + Configuration.projectResource + "/2084", basicAuth);
            Assert.IsTrue(success);

            //not testing delete all dm projects just yet
        }
        [TestMethod]
        public void PublicationRequest()
        {
            //GET LIST
            List<publication> RequestList = this.GETRequest<List<publication>>(host + Configuration.publicationResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET project publications
            List<publication> projPublicationList = this.GETRequest<List<publication>>(host + Configuration.projectResource + "/1450/" + Configuration.publicationResource);
            Assert.IsNotNull(projPublicationList, projPublicationList.Count.ToString());

            //POST
            publication postObj;
            postObj = this.POSTRequest<publication>(host + Configuration.publicationResource, new publication() { description = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.publication_id.ToString());

            //GET POSTed item
            publication RequestObj = this.GETRequest<publication>(host + Configuration.publicationResource + "/" + postObj.publication_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //POST projPublication
            List<publication> projPubResp;
            projPubResp = this.POSTRequest<publication, List<publication>>(host + Configuration.projectResource + "/1450/addPublication?PublicationId=" + postObj.publication_id, null, basicAuth);
            Assert.IsNotNull(projPubResp, projPubResp.Count.ToString());

            //PUT POSTed item
            RequestObj.description = "put-test";
            publication putObj = this.PUTRequest<publication>(host + Configuration.publicationResource + "/" + RequestObj.publication_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item 
            bool projPubsuccess = this.DELETERequest<publication>(host + Configuration.projectResource + "/1450/removePublication?PublicationId=" + RequestObj.publication_id, basicAuth);
            Assert.IsTrue(projPubsuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<publication>(host + Configuration.publicationResource + "/" + RequestObj.publication_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void ResourceRequest()
        {
            //GET LIST
            List<resource_type> RequestList = this.GETRequest<List<resource_type>>(host + Configuration.resourceResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //GET site Media
            List<resource_type> siteResourceList = this.GETRequest<List<resource_type>>(host + Configuration.siteResource + "/13659/" + Configuration.resourceResource);
            Assert.IsNotNull(siteResourceList, siteResourceList.Count.ToString());

            //POST
            resource_type postObj;
            postObj = this.POSTRequest<resource_type>(host + Configuration.resourceResource, new resource_type() { resource_name = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.resource_type_id.ToString());

            //GET POSTed item
            resource_type RequestObj = this.GETRequest<resource_type>(host + Configuration.resourceResource + "/" + postObj.resource_type_id);
            Assert.IsNotNull(RequestObj);

            //POST siteResource
            List<resource_type> siteResResp;
            siteResResp = this.POSTRequest<resource_type, List<resource_type>>(host + Configuration.siteResource + "/13659/addResource?ResourceTypeId=" + RequestObj.resource_type_id, null, basicAuth);
            Assert.IsNotNull(siteResResp, siteResResp.Count.ToString());

            //PUT POSTed item
            RequestObj.resource_name = "put-test";
            resource_type putObj = this.PUTRequest<resource_type>(host + Configuration.resourceResource + "/" + RequestObj.resource_type_id, RequestObj, basicAuth);
            Assert.IsNotNull(putObj);

            //Delete POSTed item 
            bool siteRessuccess = this.DELETERequest<resource_type>(host + Configuration.siteResource + "/13659/removeResource?ResourceTypeId=" + RequestObj.resource_type_id, basicAuth);
            Assert.IsTrue(siteRessuccess);

            //Delete POSTed item
            bool success = this.DELETERequest<resource_type>(host + Configuration.resourceResource + "/" + RequestObj.resource_type_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void RoleRequest()
        {
            //GET LIST
            List<role> RequestList = this.GETRequest<List<role>>(host + Configuration.roleResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());
                        
            //POST
            role postObj;
            postObj = this.POSTRequest<role>(host + Configuration.roleResource, new role() { role_name = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.role_id.ToString());

            //GET POSTed item
            role RequestObj = this.GETRequest<role>(host + Configuration.roleResource + "/" + postObj.role_id);
            Assert.IsNotNull(RequestObj);

            //GET dm role
            role dmRoleObj = this.GETRequest<role>(host + Configuration.dataManagerResource + "/6/role");
            Assert.IsNotNull(dmRoleObj);
           
            //PUT POSTed item
            RequestObj.role_name = "put-test";
            role putObj = this.PUTRequest<role>(host + Configuration.roleResource + "/" + RequestObj.role_id, RequestObj, basicAuth);
            Assert.IsNotNull(putObj);

            //Delete POSTed item
            bool success = this.DELETERequest<role>(host + Configuration.roleResource + "/" + RequestObj.role_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void SectionRequest()
        {
            //GET LIST
            List<section> RequestList = this.GETRequest<List<section>>(host + Configuration.sectionResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            section postObj;
            postObj = this.POSTRequest<section>(host + Configuration.sectionResource, new section() { section_name = "post-test", div_id = 1 }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.section_id.ToString());

            //GET POSTed item
            section RequestObj = this.GETRequest<section>(host + Configuration.sectionResource + "/" + postObj.section_id, basicAuth);
            Assert.IsNotNull(RequestObj);

            //PUT POSTed item
            postObj.section_name = "put-test";
            section putObj = this.PUTRequest<section>(host + Configuration.sectionResource + "/" + postObj.section_id, postObj, basicAuth);
            Assert.IsNotNull(putObj);

            //Delete POSTed item
            bool success = this.DELETERequest<section>(host + Configuration.sectionResource + "/" + postObj.section_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        [TestMethod]
        public void StatusRequest()
        {
            //GET LIST
            List<status_type> RequestList = this.GETRequest<List<status_type>>(host + Configuration.statusResource);
            Assert.IsNotNull(RequestList, RequestList.Count.ToString());

            //POST
            status_type postObj;
            postObj = this.POSTRequest<status_type>(host + Configuration.statusResource, new status_type() { status = "post-test" }, basicAuth);
            Assert.IsNotNull(postObj, "ID: " + postObj.status_id.ToString());

            //GET POSTed item
            status_type RequestObj = this.GETRequest<status_type>(host + Configuration.statusResource + "/" + postObj.status_id);
            Assert.IsNotNull(RequestObj);

            //GET site status
            status_type siteStatus = this.GETRequest<status_type>(host + Configuration.siteResource + "/1336/status");
            Assert.IsNotNull(siteStatus);

            //PUT POSTed item
            RequestObj.status = "put-test";
            status_type putObj = this.PUTRequest<status_type>(host + Configuration.statusResource + "/" + RequestObj.status_id, RequestObj, basicAuth);
            Assert.IsNotNull(RequestObj);

            //Delete POSTed item
            bool success = this.DELETERequest<status_type>(host + Configuration.statusResource + "/" + RequestObj.status_id, basicAuth);
            Assert.IsTrue(success);
        }//end method
        
        
        
        
        
        
        
        #endregion
    }
}
