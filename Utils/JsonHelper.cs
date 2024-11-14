using Newtonsoft.Json;
using System;
using System.IO;

namespace Graphic3D.Utils
{
    public class JsonHelper
    {
        // Serializes an object of type T into a JSON string.
        public static string SerializeObject<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        //Deserializes a JSON string into an object of type T.
        public static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //Serializes an object of type T and saves it to a JSON file specified by filePath.
        public static void SaveObjectToJsonFile<T>(T obj, string filePath)
        {
            try
            {
                string json = SerializeObject(obj);

                /*string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
                }*/

                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving object to JSON file: {ex.Message}");
            }
        }

        public static T LoadObjectFromJsonFile<T>(string filePath)
        {
            try
            {
                string json;

                using (StreamReader sr = new StreamReader(filePath))
                {
                    json = sr.ReadToEnd();
                }

                return DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading object from JSON file: {ex.Message}");
                return default; 
            }
        }


        public static string GetCurrentDirectory()
        {
            return Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;
        }
    }
}
