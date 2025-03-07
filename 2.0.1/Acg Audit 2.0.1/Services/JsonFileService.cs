using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ACG_AUDIT.Services
{
    public class JsonFileService
    {
        public static void SaveToJson<T>(T data, string filePath, JsonSerializerOptions options)
        {
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(filePath, json);
        }
    }
}
