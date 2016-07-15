//------------------------------------------------------------------------------
//----- JsonDotNetCodec -----------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2016 WiM - USGS

//    authors:  Jeremy Newson USGS Wisconsin Internet Mapping
//              Tonia Roddick USGS Wisconsin Internet Mapping

//   purpose:   Created a JSON Codec that works with EF. JsonDataContractCodec 
//              does not work because IsReference is set
//
//discussion:   A Codec is an enCOder/DECoder for a resources in 
//              this case the resources are POCO classes derived from the EF. 
//              https://github.com/openrasta/openrasta/wiki/Codecs
//
//     

#region Comments
// 06.23.2016 - tr - Created to properly de/serialize GeoJSON
#endregion

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using OpenRasta.TypeSystem;
using OpenRasta.Web;
using OpenRasta.Codecs;

using WiM.Resources.Spatial;
using SiGLServices.Resources;
using Newtonsoft.Json;
using WiM.Codecs.json;

using SiGLDB;

namespace SiGLServices.Codecs.json
{
    [MediaType("application/json;q=0.5", "json")]
    [MediaType("application/geojson;q=0.5", "geojson")]
    public class SiGLGeoJsonDotNetCodec : JsonDotNetCodec
    {
        private Type[] extraTypes = new Type[2] { typeof(site), typeof(FullSite) };

        public override void WriteTo(object entity, IHttpEntity response, string[] parameters)
        {
            FeatureCollection fc = null;
            double lat = -99;
            double longit = -99;
            try
            {
                if (entity == null)
                    return;

                if (entity.GetType().IsGenericType && entity.GetType().GetGenericTypeDefinition() == typeof(List<>) && extraTypes.Contains(entity.GetType().GetGenericArguments()[0]))
                {
                    fc = new FeatureCollection(4326);
                    IEnumerable collection = entity as IEnumerable;
                    foreach (var item in collection)
                    {
                        lat = Convert.ToDouble(item.GetType().GetProperty("latitude").GetValue(item));
                        longit = Convert.ToDouble(item.GetType().GetProperty("longitude").GetValue(item));

                        fc.addFeature(new Feature(item, longit, lat));
                        entity = fc;
                    }//next
                }
                else if (extraTypes.Contains(entity.GetType()))
                {
                    fc = new FeatureCollection(4326);
                    lat = Convert.ToDouble(entity.GetType().GetProperty("latitude").GetValue(entity)); ;
                    longit = Convert.ToDouble(entity.GetType().GetProperty("longitude").GetValue(entity)); ;

                    fc.addFeature(new Feature(entity, longit, lat));
                    entity = fc;
                }//end if

                //reset content type to json
                response.ContentType = MediaType.Json;
                base.WriteTo(entity, response, parameters);

            }
            catch (Exception ex)
            {
                response.ContentType = MediaType.Json;
                base.WriteTo(entity, response, parameters);
            }

        }
    }
}

