namespace AppPipe
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class SerializeHelper
    {
        /// <summary>
        /// XmlSerialize 를 이용해 현재 타입의 인스턴스를 xml (file) 로 저장한다.
        /// </summary>
        /// <param name="fileName">
        /// 저장될 파일명을 지정.
        /// </param>
        /// <returns>
        /// 저장 성공 유무를 반환.
        /// </returns>
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
                    useDataContractSerialize ?
                    DataContractSerializerSerialize(target) : XmlSerializerSerialize(target);

                    streamWriter.Write(xmlData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[void SaveDataToXml<T> Error] {0} : ", ex));
            }
        }

        /// <summary>
        /// XmlSerializer 를 이용해 현재 타입의 인스턴스를 xml (string) 으로 반환한다.
        /// </summary>
        /// <param name="obj">대상 인스턴스의 오브젝트.</param>
        /// <returns>
        /// Serialize 결과 XML ( string ).
        /// </returns>
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
                        Encoding = Encoding.UTF8
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[SaveDataToXml<T> Error] {0} : ", ex));
                return string.Empty;
            }
        }

        /// <summary>
        /// DataContractSerialize를 이용해 대상 인스턴스를 Serialize 한다.
        /// </summary>
        /// <param name="obj">대상 인스턴스 오브젝트.</param>
        /// <returns>
        /// Serialize 결과 XML ( string ).
        /// </returns>
        public static string DataContractSerializerSerialize<T>(T obj)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    // settings.Encoding = Encoding.UTF8;
                };

                using (XmlWriter xmlWriter = XmlWriter.Create(sb, settings))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(xmlWriter, obj);
                    xmlWriter.Flush();

                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("[DataContractSerialize<T> Error] {0} : ", ex));
                return string.Empty;
            }
        }

        /// <summary>
        /// 파일을 객체로 변환한다.
        /// </summary>
        /// <param name="fileName">
        /// 읽어들일 xml 파일.
        /// </param>
        /// <param name="useDataContractSerialize">
        /// DataContractSerialize 사용 여부.
        /// 여기서는 DataContractSerialize 아니면 XmlSerialize 를 사용한다.
        /// </param>
        /// <returns>
        /// 결과 오브젝트.
        /// </returns>
        public static T ReadDataFromXmlFile<T>(
            string fileName,
            bool useDataContractSerialize = false) where T : class
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(fileName))
                {
                    string xmlData = streamReader.ReadToEnd();
                    T result =
                    useDataContractSerialize ?
                    DataContractSerializeDeserialize<T>(xmlData) : XmlSerializerDeserialize<T>(xmlData);

                    return result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("[ReadDataFromXml<T> Error] {0} : ", ex));
                return null;
            }
        }

        public static T DeserializeByDataContractSerializer<T>(string xmlData) where T : class
        {
            try
            {
                using (var reader = new StringReader(xmlData))
                {
                    var xmlReader = XmlReader.Create(reader);
                    var serializer = new DataContractSerializer(typeof(T));
                    return serializer.ReadObject(xmlReader) as T;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error DeserializeByDataContractSerializer()");
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// DataContractSerialize를 이용해 Xml 문자열을 Deserialize 한다.
        /// </summary>
        /// <param name="xmlData">
        /// DataContractSerialize를 이용해 Serialize된 xml 문자열.
        /// </param>
        /// <returns>
        /// 결과 오브젝트.
        /// </returns>
        public static T DataContractSerializeDeserialize<T>(string xmlData) where T : class
        {
            try
            {
                using (StringReader reader = new StringReader(xmlData))
                {
                    XmlReader xmlReader = XmlReader.Create(reader);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return serializer.ReadObject(xmlReader) as T;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("[DataDeserialize<T> Error] {0} : ", ex));
                return null;
            }
        }

        /// <summary>
        /// 문자열을 객체로 변환한다.
        /// </summary>
        /// <param name="xmlData">
        /// xml 문자열 데이터.
        /// </param>
        /// <returns>
        /// 결과 오브젝트.
        /// </returns>
        public static T XmlSerializerDeserialize<T>(string xmlData) where T : class
        {
            try
            {
                using (StringReader stringReader = new StringReader(xmlData))
                {
                    using (XmlTextReader xmlReader = new XmlTextReader(stringReader))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        return serializer.Deserialize(xmlReader) as T;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    string.Format("[ReadDataFromXml Error] {0} : ", ex));
                return null;
            }
        }

        public static string SerializeByDataContractSerializer<T>(T obj)
        {
            try
            {
                if (obj is DataSet)
                {
                    var dataSet = obj as DataSet;
                    var convertedDataSet = DateTimeModeConverter.ConvertTo(dataSet);
                    return InnerSerializeByDataContractSerializer(convertedDataSet);
                }

                if (obj is DataTable)
                {
                    var dataTable = obj as DataTable;
                    var convertedDataTable = DateTimeModeConverter.ConvertTo(dataTable);
                    return InnerSerializeByDataContractSerializer(convertedDataTable);
                }

                return InnerSerializeByDataContractSerializer(obj);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error SerializeByDataContractSerializer()");
                Debug.WriteLine(ex);
                return string.Empty;
            }
        }

        private static string InnerSerializeByDataContractSerializer<T>(T obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    ConformanceLevel = ConformanceLevel.Document,
                    Encoding = new UTF8Encoding(false)
                };

                using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
                {
                    var serializer = new DataContractSerializer(typeof(T));

                    serializer.WriteObject(xmlWriter, obj);

                    xmlWriter.Flush();

                    if (xmlWriter.Settings == null)
                    {
                        throw new Exception("XmlWriterSettings is not created.");
                    }

                    memoryStream.Position = 0;
                    return GetStringFromWrittenBuffer(xmlWriter.Settings.Encoding, memoryStream);
                }
            }
        }

        private static string GetStringFromWrittenBuffer(Encoding encoding, MemoryStream memoryStream)
        {
            return encoding.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
    }
}
