using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MVVMToolKit.Helper
{
    /// <summary>
    /// The serialize helper class.
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// Reads the data from xml file using the specified file name.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="fileName">The file name.</param>
        /// <param name="useDataContractSerialize">The use data contract serialize.</param>
        /// <returns>The xmlFile.</returns>
        public static T? ReadDataFromXmlFile<T>(
            string fileName,
            bool useDataContractSerialize = false)
            where T : class
        {
            try
            {
                using StreamReader streamReader = new StreamReader(fileName);
                string xmlData = streamReader.ReadToEnd();
                T? result =
                    useDataContractSerialize
                        ? DataContractSerializeDeserialize<T>(xmlData)
                        : XmlSerializerDeserialize<T>(xmlData);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Data the contract serialize deserialize using the specified xml data.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="xmlData">The xml data.</param>
        /// <returns>The Deserialize Data.</returns>
        public static T? DataContractSerializeDeserialize<T>(string xmlData)
            where T : class
        {
            try
            {
                using StringReader reader = new StringReader(xmlData);
                XmlReader xmlReader = XmlReader.Create(reader);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return serializer.ReadObject(xmlReader) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Xml the serializer deserialize using the specified xml data.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="xmlData">The xml data.</param>
        /// <returns>The Deserialize data.</returns>
        public static T? XmlSerializerDeserialize<T>(string xmlData)
            where T : class
        {
            try
            {
                using StringReader stringReader = new StringReader(xmlData);
                using XmlTextReader xmlReader = new XmlTextReader(stringReader);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return serializer.Deserialize(xmlReader) as T;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Saves the data to xml using the specified file name.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="fileName">The file name.</param>
        /// <param name="target">The target.</param>
        /// <param name="useDataContractSerialize">The use data contract serialize.</param>
        public static void SaveDataToXml<T>(
            string fileName,
            T target,
            bool useDataContractSerialize = false)
        {
            try
            {
                using (TextWriter streamWriter = new StreamWriter(fileName, false, Encoding.UTF8))
                {
                    string xmlData =
                        useDataContractSerialize
                            ? DataContractSerializerSerialize(target)
                            : XmlSerializerSerialize(target);

                    streamWriter.Write(xmlData);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// Datas the contract serializer serialize using the specified obj.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>The string.</returns>
        public static string DataContractSerializerSerialize<T>(T obj)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                };

                using XmlWriter xmlWriter = XmlWriter.Create(sb, settings);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(xmlWriter, obj);
                xmlWriter.Flush();

                return sb.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Xmls the serializer serialize using the specified obj.
        /// </summary>
        /// <typeparam name="T">The T.</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>The string.</returns>
        public static string XmlSerializerSerialize<T>(T obj)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                string xmlData;
                using (MemoryStream memStream = new MemoryStream())
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = new string(' ', 4),
                        NewLineOnAttributes = false,
                        Encoding = Encoding.UTF8,
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(memStream, settings))
                    {
                        serializer.Serialize(xmlWriter, obj);
                    }

                    xmlData = Encoding.UTF8.GetString(memStream.GetBuffer());
                    xmlData = xmlData.Substring(xmlData.IndexOf('<'));
                    xmlData = xmlData.Substring(0, xmlData.LastIndexOf('>') + 1);
                }

                return xmlData;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
