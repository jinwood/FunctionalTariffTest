using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace TarrifComparison
{
    public static class Data
    {
        public static List<TarrifEntry> Load()
        {
            //read the json file and deserialize into the type defined below
            using (StreamReader reader = new StreamReader(File.OpenText("prices.json").BaseStream))
            {
                return ((List<TarrifEntry>)new JsonSerializer()
                    .Deserialize(reader, typeof(List<TarrifEntry>)));
            }
        }
    }

    //used structs to enforce the immutable nature
    //of this application
    public struct Rates
    {
        public decimal power { get; set; }
        public decimal gas { get; set; }
    }

    public struct TarrifEntry 
    {
        public string tariff { get; set; }
        public Rates rates { get; set; }
        public decimal standing_charge { get; set; }
    }

    public struct EnergyRate
    {
        public string Name { get; set; }
        public decimal Cost { get; set; }
    }
}
