using System.Linq;

namespace TarrifComparison
{
    public static class CostLogic
    {
        private static int gas;
        private static int power;
        
        //not a pure function as it accesses data which can vary results
        public static string[] CalculateCost(string[] args)
        {
            if (args.Length < 3) return null;

            int.TryParse(args[1], out power);
            int.TryParse(args[2], out gas);

            var data = Data.Load();
            //use LINQ functions to iterate our dataset and generate a new array of strings
            //correctly formatted to display the result
            var result = 
                data.Select(x =>
                    $"{x.tariff} £{SumGasAndPower(x, power, gas)}").ToArray();
                                
            return result;
        }

        //Pure function
        private static string SumGasAndPower(TarrifEntry tarrif, int power, int gas)
        {
            return (
                ((power * tarrif.rates.power) + (gas * tarrif.rates.gas) + tarrif.standing_charge) 
                    * Constants.VAT)
                .ToString("#.##");
        }
    }
}
