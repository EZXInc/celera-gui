using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;

namespace EZXWPFLibrary.Helpers
{
    //[DebuggerStepThrough()]
    //[DebuggerNonUserCode()]
    public static class XmlHelper
    {
        public static T ReadFromString<T>(string objectString) where T : class
        {
            T returnObj = default(T);
            using (StringReader stream = new StringReader(objectString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                returnObj = serializer.Deserialize(stream) as T;
                //assume that the parameter less constructor sets the new values as default
            }
            return returnObj;
        }

        public static T ReadFromFile<T>(string filename) where T : class
        {
            T returnObj = default(T);
            if (File.Exists(filename))
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    returnObj = serializer.Deserialize(stream) as T;
                    //assume that the parameter less constructor sets the new values as default
                }
            }
            return returnObj;
        }

        public static void WriteToFile<T>(T objToWrite, string filename) where T : class
        {
            string dirName = Path.GetDirectoryName(filename);
            if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, objToWrite);
            }
        }


        
    }
}
