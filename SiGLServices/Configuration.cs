//------------------------------------------------------------------------------
//----- Configuration ----------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2016 WiM - USGS

//    authors:  Jeremy K. Newson USGS Wisconsin Internet Mapping
//              Tonia Roddick USGS Wisconsin Internet Mapping
//  
//   purpose:   Registration of resources using ResourceSpace.Has, specify specific 
//              representations with corresponding handlers and codecs.
//
//discussion:   https://github.com/openrasta/openrasta/wiki/Configuration
//
//     

using System;
using System.Collections.Generic;
using WiM.Codecs.json;
using WiM.Codecs.xml;
using WiM.Codecs.csv;
using WiM.PipeLineContributors;

using SiGLDB;
using SiGLServices;
using SiGLServices.Handlers;
using SiGLServices.PipeLineContributors;
using SiGLServices.Security;
using SiGLServices.Resources;
using SiGLServices.Codecs.json;


using OpenRasta.Configuration;
using OpenRasta.Web.UriDecorators;
using OpenRasta.Security;
using OpenRasta.Codecs;
using OpenRasta.IO;
using OpenRasta.DI;
using OpenRasta.Pipeline.Contributors;
using OpenRasta.Pipeline;

namespace SiGLServices
{
    public class Configuration : IConfigurationSource
    {
        #region Private Field Properties
        public static string contactResource = "Contacts";
        public static string dataHostResource = "DataHosts";
        public static string dataManagerResource = "DataManagers";
        public static string divisionResource = "Divisions";
        public static string frequencyResource = "Frequencies";
        public static string keywordResource = "Keywords";
        public static string lakeResource = "Lakes";
        public static string mediaResource = "Media";
        public static string objectiveResource = "Objectives";
        public static string organizationResource = "Organizations";
        public static string organizationSystemResource = "OrganizationSystems";
        public static string parameterResource = "Parameters";
        public static string projStatusResource = "ProjectStatus";
        public static string projDurationResource = "ProjectDuration";
        public static string projectResource = "Projects";
        public static string publicationResource = "Publications";

        public static string orgSystemResource = "OrganizationSystems";
        public static string siteResource = "Sites";
        
        #endregion
        
        public void Configure()
        {
            using (OpenRastaConfiguration.Manual)
            {
                // specify the authentication scheme you want to use, you can register multiple ones
                ResourceSpace.Uses.CustomDependency<IAuthenticationProvider, SiGLAuthenticationProvider>(DependencyLifetime.Singleton);
                ResourceSpace.Uses.PipelineContributor<BasicAuthorizerContributor>();

                // Allow codec choice by extension 
                ResourceSpace.Uses.UriDecorator<ContentTypeExtensionUriDecorator>();
                ResourceSpace.Uses.PipelineContributor<ErrorCheckingContributor>();
                ResourceSpace.Uses.PipelineContributor<CrossDomainPipelineContributor>();
                ResourceSpace.Uses.PipelineContributor<MessagePipelineContributor>();
                ResourceSpace.Uses.PipelineContributor<SiGLHyperMediaPipelineContributor>();

                AddCONTACT_Resources();
                AddDATAHOST_Resources();
                AddDATA_MANAGER_Resources();
                AddDIVISION_Resources();
                AddFREQUENCY_Resources();
                AddKEYWORD_Resources();
                AddLAKE_Resources();
                AddMEDIA_Resources();
                AddOBJECTIVE_Resources();
                AddORGANIZATION_Resources();
                AddORGANIZATION_SYSTEM_Resources();
                AddPARAMETER_Resources();
                AddPROJ_STATUS_Resources();
                AddPROJ_DURATION_Resources();
                AddPROJECT_Resources();
                AddPUBLICATION_Resources();
            }//End using OpenRastaConfiguration.Manual
        }

        #region Helper methods

        private void AddCONTACT_Resources() //(1)
        {
            ResourceSpace.Has.ResourcesOfType<List<contact>>()
            .AtUri(contactResource)
            .And.AtUri(projectResource + "/{projectId}/addContact?ContactId={contactId}").Named("AddProjectContact")
            .And.AtUri(projectResource + "/{projectId}/" + contactResource).Named("GetProjectContacts")
            .And.AtUri(orgSystemResource + "/{orgSysId}/" + contactResource).Named("GetOrgSysContacts")
            .HandledBy<ContactHandler>()
            .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
            .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
            .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<contact>()
            .AtUri(contactResource + "/{entityId}")
            .And.AtUri(projectResource + "/{projectId}/removeContact?ContactId={contactId}").Named("RemoveProjectContact")            
            .HandledBy<ContactHandler>()
            .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
            .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
            .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

        }//end AddCONTACT_Resources()
        private void AddDATAHOST_Resources() //(2)
        {
            ResourceSpace.Has.ResourcesOfType<List<data_host>>()
                .AtUri(dataHostResource)
                //.And.AtUri(projectResource + "/projects/{projectId}/addDataHost").Named("AddProjectDataHost")
                .And.AtUri(projectResource + "/{projectId}/" + dataHostResource).Named("GetProjectDataHosts")
                .HandledBy<DataHostHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<data_host>()
                .AtUri(dataHostResource + "/{entityId}")
               // .And.AtUri(projectResource + "/{projectId}/removeDataHost?DataHostId={dataHostId}").Named("RemoveProjectDataHost")
                .HandledBy<DataHostHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddDATA_MANAGER_Resources() //(3)
        {
            ResourceSpace.Has.ResourcesOfType<List<data_manager>>()
                .AtUri(dataManagerResource)                
                .HandledBy<DataManagerHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<data_manager>()
                .AtUri(dataManagerResource + "/{entityId}")
                .And.AtUri("/login").Named("GetLoggedUser")
                .And.AtUri(projectResource + "/{projectId}/dataManager").Named("GetProjectDataManager")
                .HandledBy<DataManagerHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            //DM_LIST_VIEW
            ResourceSpace.Has.ResourcesOfType<List<dm_list_view>>()
                .AtUri(dataManagerResource + "/DMList").Named("GetDMListModel")
                .HandledBy<DataManagerHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

        }//end AddDATA_MANAGER_Resources()
        private void AddDIVISION_Resources() //(4)
        {
            ResourceSpace.Has.ResourcesOfType<List<division>>()
                .AtUri(divisionResource)
                .HandledBy<DivisionHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<division>()
                .AtUri(divisionResource + "/{entityId}")
                .HandledBy<DivisionHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

        }//end AddDIVISION_Resources()
        private void AddFREQUENCY_Resources() //(5)
        {
            ResourceSpace.Has.ResourcesOfType<List<frequency_type>>()
                .AtUri(frequencyResource)
                .And.AtUri(siteResource + "/{siteId}/addFrequency?FrequencyTypeId={frequencyTypeId}").Named("AddSiteFrequency")
                .And.AtUri(siteResource + "/{siteId}/" + frequencyResource).Named("GetSiteFrequencies")
                .HandledBy<FrequencyHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<frequency_type>()
                .AtUri(frequencyResource + "/{entityId}")
                .And.AtUri(siteResource + "/{siteId}/removeFrequencyType?FrequencyTypeId={frequencyTypeId}").Named("removeSiteFrequency")
                .HandledBy<FrequencyHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddKEYWORD_Resources() //(6)
        {
            ResourceSpace.Has.ResourcesOfType<List<keyword>>()
                .AtUri(keywordResource)
                .And.AtUri(projectResource + "/{projectId}/addKeyword?Word={term}").Named("AddProjectKeyword")
                .And.AtUri(projectResource + "/{projectId}/" + keywordResource).Named("GetProjectKeyword")
                .HandledBy<KeywordHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<keyword>()
                .AtUri(keywordResource + "/{entityId}")
                .And.AtUri(projectResource + "/{projectId}/removeKeyword?KeywordId={keywordId}").Named("RemoveProjectKeyword")
                .And.AtUri(keywordResource + "?term={term}").Named("GetKeywordByTerm")
                .HandledBy<KeywordHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddLAKE_Resources() //(7)
        {
            ResourceSpace.Has.ResourcesOfType<List<lake_type>>()
                .AtUri(lakeResource)                
                .HandledBy<LakeHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<lake_type>()
                .AtUri(lakeResource + "/{entityId}")
                .And.AtUri(siteResource + "/{siteId}/lake").Named("GetSiteLake")
                .HandledBy<LakeHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddMEDIA_Resources() //(8)
        {
            ResourceSpace.Has.ResourcesOfType<List<media_type>>()
                .AtUri(mediaResource)
                .And.AtUri(siteResource + "/{siteId}/addMedia?MediaTypeId={mediaTypeId}").Named("AddSiteMedia")
                .And.AtUri(siteResource + "/{siteId}/" + mediaResource).Named("GetSiteMedia")
                .HandledBy<MediaHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<media_type>()
                .AtUri(mediaResource + "/{entityId}")
                .And.AtUri(siteResource + "/{siteId}/removeMediaType?MediaTypeId={mediaTypeId}").Named("removeSiteMedia")
                .HandledBy<MediaHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddOBJECTIVE_Resources() //(9)
        {
            ResourceSpace.Has.ResourcesOfType<List<objective_type>>()
                .AtUri(objectiveResource)
                .And.AtUri(projectResource + "/{projectId}/addObjective?ObjectiveTypeId={objectiveTypeId}").Named("AddProjectObjective")
                .And.AtUri(projectResource + "/{projectId}/" + objectiveResource).Named("GetProjectObjectives")
                .HandledBy<ObjectiveHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<objective_type>()
                .AtUri(objectiveResource + "/{entityId}")
                .And.AtUri(projectResource + "/{projectId}/removeObjective?ObjectiveTypeId={objectiveTypeId}").Named("RemoveProjectObjective")
                .HandledBy<ObjectiveHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddORGANIZATION_Resources() //(10)
        {
            ResourceSpace.Has.ResourcesOfType<List<organization>>()
                .AtUri(organizationResource)                
                .HandledBy<OrganizationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<organization>()
                .AtUri(organizationResource + "/{entityId}")                
                .HandledBy<OrganizationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddORGANIZATION_SYSTEM_Resources() //(11)
        {
            ResourceSpace.Has.ResourcesOfType<List<organization_system>>()
                .AtUri(organizationSystemResource)
                .And.AtUri(organizationSystemResource + "/OrgResources").Named("AllOrgResources")
                .And.AtUri(projectResource + "/{projectId}/OrganizationResources").Named("GetProjectOrganizations")
                .And.AtUri(projectResource + "/{projectId}/addOrganization?OrganizationId={orgId}&DivisionId={divId}&SectionId={secId}").Named("AddProjectOrganization")
                .HandledBy<OrganizationSystemHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<organization_system>()
                .AtUri(organizationSystemResource + "/{entityId}")
                .And.AtUri(organizationSystemResource + "/OrgResources/{orgSystemId}").Named("GetAnOrgSystemResource")
                .And.AtUri(projectResource + "/{projectId}/removeOrganization?OrgSystemId={orgSystemId}").Named("RemoveProjectOrganization")
                .HandledBy<OrganizationSystemHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

        }
        private void AddPARAMETER_Resources() //(12)
        {
            ResourceSpace.Has.ResourcesOfType<List<parameter_type>>()
                .AtUri(parameterResource)
                .And.AtUri(siteResource + "/{siteId}/addParameter?ParameterTypeId={parameterTypeId}").Named("AddSiteParameter")
                .And.AtUri(siteResource + "/{siteId}/" + parameterResource).Named("GetSiteParameters")
                .HandledBy<ParameterHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<ParameterGroups>()
                .AtUri(parameterResource + "?GroupNames={groupNames}").Named("GetParametersByGroupName")               
                .HandledBy<ParameterHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<parameter_type>()
                .AtUri(parameterResource + "/{entityId}")
                .And.AtUri(siteResource + "/{siteId}/removeParameterType?ParameterTypeId={parameterTypeId}").Named("RemoveSiteParameter")
                .HandledBy<ParameterHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddPROJ_STATUS_Resources() //(13)
        {
            ResourceSpace.Has.ResourcesOfType<List<proj_status>>()
                .AtUri(projStatusResource)
                .HandledBy<ProjStatusHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<proj_status>()
                .AtUri(projStatusResource + "/{entityId}")
                .And.AtUri(projectResource + "/{projectId}/projStatus").Named("GetProjectStatus")
                .HandledBy<ProjStatusHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

        }
        private void AddPROJ_DURATION_Resources() //(13)
        {
            ResourceSpace.Has.ResourcesOfType<List<proj_duration>>()
                .AtUri(projDurationResource)
                .HandledBy<ProjDurationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<proj_duration>()
                .AtUri(projDurationResource + "/{entityId}")
                .And.AtUri(projectResource + "/{projectId}/projDuration").Named("GetProjectDuration")
                .HandledBy<ProjDurationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }
        private void AddPROJECT_Resources() //(14)
        {
            //DO MONDAY
        }
        private void AddPUBLICATION_Resources() //(15)
        {
            ResourceSpace.Has.ResourcesOfType<List<publication>>()
                .AtUri(publicationResource)
                .And.AtUri(projectResource + "/{projectId}/addPublication?PublicationId={publicationId}").Named("AddProjectPublication")
                .And.AtUri(projectResource + "/{projectId}/" + publicationResource).Named("GetProjectPublications")
                .HandledBy<PublicationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");

            ResourceSpace.Has.ResourcesOfType<publication>()
                .AtUri(publicationResource + "/{entityId}")
                .And.AtUri(projectResource + "/{projectId}/removePublication?PublicationId={publicationId}").Named("RemoveProjectPublication")
                .HandledBy<PublicationHandler>()
                .TranscodedBy<UTF8EntityXmlSerializerCodec>(null).ForMediaType("application/xml;q=1").ForExtension("xml")
                .And.TranscodedBy<JsonEntityDotNetCodec>(null).ForMediaType("application/json;q=0.9").ForExtension("json")
                .And.TranscodedBy<CsvDotNetCodec>(null).ForMediaType("text/csv").ForExtension("csv");
        }

        #endregion

    }//End class Configuration
}//End namespace

