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


        public static T ReadJsonSettings<T>(string settingsRelativePath) where T : new()
        {
            string settingsFileAbsolutePath = Directory.GetCurrentDirectory() + "\\" + settingsRelativePath;

            //AdvConsole.WriteLine("<<<------- Reading a settings file ------->>>", 0, ConsoleColor.DarkBlue);
            //Console.WriteLine($"Relative path to file - {settingsRelativePath}");
            //Console.WriteLine($"Absolute path to file - {settingsFileAbsolutePath}");

            T? settings = new T();

            try
            {
                if (!File.Exists(settingsFileAbsolutePath)) throw new Exception($"File on path <{settingsFileAbsolutePath}> does not exist. Unable to load settings.");

                using (StreamReader sr = new StreamReader(settingsFileAbsolutePath))
                {
                    string json = sr.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<T>(json);
                }

                if (settings == null) throw new Exception($"Deserialized {typeof(T)} object from file <{settingsFileAbsolutePath}> is null. (Maybe something is wrong with the file syntax or path to it?)");
            }
            catch (Exception ex)
            {
                //AdvConsole.WriteLine("<<<--!!!-- ERROR --!!!-->>>", 0, ConsoleColor.Red);
                //AdvConsole.WriteLine($"{ex.Message}", 0, ConsoleColor.White);
                throw;
            }

            return settings;
        }
    }
}

