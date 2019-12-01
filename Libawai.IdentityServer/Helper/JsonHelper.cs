using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Libawai.IdentityServer.Helper
{
    public class JsonHelper
    {
        /// <summary>
        /// 转化为JSON格式数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJson<T>(object obj)
        {
            string result;
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                result = Encoding.UTF8.GetString(ms.ToArray());
            }

            return result;
        }

        /// <summary>
        /// 转化为List的数据为JSON格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public string Json<T>(List<T> values)
        {
            var stringBuilder = new StringBuilder();
            var jsonSerializer = new DataContractJsonSerializer(typeof(T));

            foreach (var value in values)
            {
                using var ms = new MemoryStream();
                jsonSerializer.WriteObject(ms, value);
                stringBuilder.Append(Encoding.UTF8.GetString(ms.ToArray()));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Json格式字符串转换为T类型的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T ParseFormByJson<T>(string jsonStr)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonStr));
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T) serializer.ReadObject(ms);
        }

    }
}
