using Newtonsoft.Json;

namespace FitiChanBot
{
    public static class FitiUtilities
    {
        public static TimeSpan UTCOffsetCalculate(DateTime local)
        {
            if (DateTime.Compare(DateTime.UtcNow, local) > 0)
            {
                TimeSpan utcOffset = DateTime.UtcNow - local;
                //Text = $"{DateTime.UtcNow.ToString("H:mm")} (UTC) / {(DateTime.UtcNow - utcOffset).ToString("H:mm")} (Local, UTC -{utcOffset.ToString("hh\\:mm")})"
                return new TimeSpan(0, 0, 0, 0, 0) - utcOffset;
            }
            else if (DateTime.Compare(DateTime.UtcNow, local) < 0)
            {
                TimeSpan utcOffset = local - DateTime.UtcNow;
                //Text = $"{DateTime.UtcNow.ToString("H:mm")} (UTC) / {(DateTime.UtcNow + utcOffset).ToString("H:mm")} (Local, UTC +{utcOffset.ToString("hh\\:mm")})"
                return new TimeSpan(0, 0, 0, 0, 0) + utcOffset;
            }
            else
            {
                return new TimeSpan(0, 0, 0, 0, 0);
            }
        }


        public static T ReadJsonRelative<T>(string relativePath) where T : new()
        {
            string jsonFileAbsolutePath = Directory.GetCurrentDirectory() + "/" + relativePath;

            //AdvConsole.WriteLine("<<<------- Reading a settings file ------->>>", 0, ConsoleColor.DarkBlue);
            //Console.WriteLine($"Relative path to file - {settingsRelativePath}");
            //Console.WriteLine($"Absolute path to file - {settingsFileAbsolutePath}");

            T? jsonCSharpObject = new T();

            try
            {
                if (!File.Exists(jsonFileAbsolutePath)) throw new Exception($"File on path <{jsonFileAbsolutePath}> does not exist. Unable to load settings.");

                using (StreamReader sr = new StreamReader(jsonFileAbsolutePath))
                {
                    string json = sr.ReadToEnd();
                    jsonCSharpObject = JsonConvert.DeserializeObject<T>(json);
                }

                if (jsonCSharpObject == null) throw new Exception($"Deserialized {typeof(T)} object from file <{jsonFileAbsolutePath}> is null. (Maybe something is wrong with the file syntax or path to it?)");
            }
            catch
            {
                //AdvConsole.WriteLine("<<<--!!!-- ERROR --!!!-->>>", 0, ConsoleColor.Red);
                //AdvConsole.WriteLine($"{ex.Message}", 0, ConsoleColor.White);
                throw;
            }

            return jsonCSharpObject;
        }
    }
}

