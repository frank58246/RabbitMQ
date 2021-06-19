using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RabbitMQ.Common.Helpers
{
    public static class FormatHelper
    {
        public static T ToObject<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var bf = new BinaryFormatter();
                var obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        public static byte[] ToByteArray<T>(T data)
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, data);
                return ms.ToArray();
            }
        }
    }
}