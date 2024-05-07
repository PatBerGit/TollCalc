namespace TollCalculator.Configurations
{
    namespace TollCalculator.Configuration
{
    using global::TollCalculator.Model.Enums;
    using System;
    using System.Collections.Generic;

        public class TollCalculatorSettings
    {
        public List<TollRateOptions> TollRateOptions { get; set; }
        public List<DateTime> TollFreeDates { get; set; }
        public List<TollFreeVehicles> TollFreeVehicleTypes { get; set; }

        public int MaxDailyFee { get; set; }

        public TollCalculatorSettings()
        {
            TollRateOptions = new List<TollRateOptions>();
            TollFreeDates = new List<DateTime>();
            TollFreeVehicleTypes = new List<TollFreeVehicles>();
        }
    }
}

}
