using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DirectMessage.Helper
{
    public static class DirectMessageConfig
    {
        static string path = FileSystem.Current.AppDataDirectory + "\\config.json";

        // public string IpAdress { get { return IpAdress; } set { } }
        public static Config _conf = null;


        static private void init()
        {
            if (!File.Exists(path))
            {
                _conf = new Config();
                _conf.CountryCode = "+389  North Macedonia";
                _conf.PrevNumbers = new List<string>();
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(_conf));

            }
        }
        static private void read()
        {
            init();
            _conf = JsonSerializer.Deserialize<Config>(File.ReadAllText(path));

        }
        static private void write()
        {
            init();
            File.WriteAllText(path, JsonSerializer.Serialize(_conf));
        }
        static public string GetCountryCode()
        {
            read();
            return _conf.CountryCode;

        }
        public static void SetCountryCode(string key)
        {
            _conf.CountryCode = key;
            write();
        }

        static public List<string> GetPrevNumbers()
        {
            read();
            return _conf.PrevNumbers;
        }

        public static void SetPrevNumbers(List<string> c)
        {
            _conf.PrevNumbers = c;
            write();
        }
        public static void AddPrevNumbers(string c)
        {

            _conf.PrevNumbers.Add(c);
            write();
        }

    }



    public class Config
    {
        public string CountryCode { get; set; }
        public List<string> PrevNumbers { get; set; }

    }
}
