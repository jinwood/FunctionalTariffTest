using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TarrifComparison
{
    public static class Data
    {
        public static List<TarrifEntry> Load()
        {
            using (StreamReader reader = new StreamReader(File.OpenText("prices.json").BaseStream))
            {
                var json = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<TarrifEntry>>(json);

                return data;
            }
        }
    }

    public class Rates
    {
        public decimal power { get; set; }
        public decimal gas { get; set; }
    }

    public class TarrifEntry
    {
        public string tariff { get; set; }
        public Rates rates { get; set; }
        public decimal standing_charge { get; set; }
    }
}
