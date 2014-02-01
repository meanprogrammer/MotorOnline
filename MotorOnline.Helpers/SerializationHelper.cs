using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace MotorOnline.Helpers
{
    public static class SerializationHelper
    {
        private static JavaScriptSerializer singletonSerializer = new JavaScriptSerializer();

        public static T Deserialize<T>(string json) {
            return singletonSerializer.Deserialize<T>(json);
        }

        //public static Dictionary<string, string> DeserializeAsOne(string json)
        //{ 
        //    List<Dictionary<string, string>> 
        //}
    }

    public class JsonKeyValuePair
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
