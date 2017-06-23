namespace TarrifComparison
{
    public static class DecimalExtensions
    {
        public static decimal ApplyVAT(this decimal value)
        {
            return value + (Constants.VatPercent * value);
        }
    }
}
