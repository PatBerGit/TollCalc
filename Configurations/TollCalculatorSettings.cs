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
        public List<TollFreeVehicles> TollFreeVehicleTypes { get; set; }

        public int MaxDailyFee { get; set; }
        public string? HolidayApiUrl { get; set; }

            public TollCalculatorSettings()
        {
            TollRateOptions = new List<TollRateOptions>();
            TollFreeVehicleTypes = new List<TollFreeVehicles>(); 
        }
    }
}

}
