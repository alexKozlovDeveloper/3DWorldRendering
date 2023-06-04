using Assets.Scripts.Storing.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Assets.Scripts.Storing
{
    public static class ModelStoringExtensions
    {
        //public static void SaveAsJson<T>(this T model, string filePath)
        //{
        //    string json = JsonConvert.SerializeObject(model);

        //    string fileName = Path.GetFileNameWithoutExtension(filePath) + "_" + DateTime.Now.ToString("[yyyy-MM-dd]_(HH-mm-ss)") + Path.GetExtension(filePath);
        //    string filePathWithTime = Path.Combine(Path.GetDirectoryName(filePath), fileName);

        //    File.WriteAllText(filePathWithTime, json);
        //}

        //public static T LoadFromJson<T>(string filePath)
        //{
        //    var json = File.ReadAllText(filePath);

        //    var model = JsonConvert.DeserializeObject<T>(json);

        //    return model;
        //}

        //public static void SerializeModelToFile<T>(this T model, string filePath)
        //{
        //    // Создаем объект DataContractJsonSerializer для выполнения сериализации
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

        //    // Открываем поток для записи в файл
        //    using (FileStream stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        // Сериализуем модель и записываем ее в поток
        //        serializer.WriteObject(stream, model);
        //    }
        //}

        //public static T DeserializeModelFromFile<T>(string filePath)
        //{
        //    // Создаем объект DataContractJsonSerializer для выполнения десериализации
        //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

        //    // Открываем поток для чтения из файла
        //    using (FileStream stream = new FileStream(filePath, FileMode.Open))
        //    {
        //        // Десериализуем модель из потока и приводим ее к типу T
        //        return (T)serializer.ReadObject(stream);
        //    }
        //}

        public static void SerializeModelToXml<T>(this T model, string filePath)
        {
            // Создаем экземпляр класса XmlSerializer для заданного типа модели
            var serializer = new XmlSerializer(typeof(T));

            // Открываем поток для записи в файл по указанному пути
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Сериализуем модель в XML и записываем результат в файл
                serializer.Serialize(fileStream, model);
            }
        }

        public static T DeserializeModelFromXml<T>(string filePath)
        {
            // Создаем экземпляр класса XmlSerializer для заданного типа модели
            var serializer = new XmlSerializer(typeof(T));

            // Открываем поток для чтения файла по указанному пути
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                // Десериализуем данные из файла в модель и возвращаем результат
                return (T)serializer.Deserialize(fileStream);
            }
        }

        public static Type[] FindCellLayerTypes()
        {
            var typesInNamespace = Assembly.GetAssembly(typeof(ILayer))
                .GetTypes()
                .Where(t => String.Equals(t.Namespace, "Assets.Scripts.Storing.Models.Layers", StringComparison.Ordinal));

            var cellLayerTypes = typesInNamespace.Where(t => typeof(ILayer).IsAssignableFrom(t)).ToArray();

            return cellLayerTypes;
        }
    }
}
