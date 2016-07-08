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
        
        #endregion
    }
}
