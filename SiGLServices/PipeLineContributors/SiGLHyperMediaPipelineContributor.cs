using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiM.Hypermedia;
using WiM.PipeLineContributors;
using SiGLDB;
using WiM.Resources;

namespace SiGLServices.PipeLineContributors
{
    public class SiGLHyperMediaPipelineContributor : HypermediaPipelineContributor
    {
        protected override List<Link> GetReflectedHypermedia(IHypermedia entity)
        {
            List<Link> results = null;
            switch (entity.GetType().Name)
            {
                case "contact":
                    results = new List<Link>();
                    results.Add(new Link(BaseURI, "projects", String.Format(Configuration.projectResource + "/{0}/" + Configuration.contactResource, ((contact)entity).contact_id), refType.GET));
                    results.Add(new Link(BaseURI, "organizationSystems", String.Format(Configuration.orgSystemResource + "/{0}/" + Configuration.contactResource, ((contact)entity).contact_id), refType.GET));

                    break;


                default:
                    break;
            }

            return results;
        }
        protected override List<Link> GetEnumeratedHypermedia(IHypermedia entity)
        {
            List<Link> results = null;
            switch (entity.GetType().Name)
            {
                case "contact":
                    results = new List<Link>();
                    results.Add(new Link(BaseURI, "self", Configuration.contactResource + "/" + ((contact)entity).contact_id, refType.GET));
                    results.Add(new Link(BaseURI, "edit", Configuration.contactResource + "/" + ((contact)entity).contact_id, refType.PUT));
                    results.Add(new Link(BaseURI, "delete", Configuration.contactResource + "/" + ((contact)entity).contact_id, refType.DELETE));
                    break;

                default:
                    break;
            }

            return results;
        }

    }//end class
}//end namespace
