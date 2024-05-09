using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using TollCalculator.Configurations.TollCalculator.Configuration;
using TollCalculator.Model;
using TollCalculator.Model.Enums;
using TollCalculator.Responses;

namespace TollCalculator.Services
{
    public class TollCalculatorService
    {
        private readonly HttpClient _httpClient;
        private readonly TollCalculatorSettings _settings;

        public TollCalculatorService(IOptions<TollCalculatorSettings> settings, HttpClient httpClient)
        {
            _settings = settings.Value;

            // Set the base address for HttpClient
            httpClient.BaseAddress = new Uri(_settings.HolidayApiUrl);

            _httpClient = httpClient;
        }

        public async Task<bool> IsTollFreeDateAsync(DateTime date)
        {
            // Send HTTP request to the API endpoint for the given date
            string apiUrl = $"{_settings.HolidayApiUrl}/{date.Year}/{date.Month:00}/{date.Day:00}";
            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Handle error if API request fails
                throw new Exception($"Failed to fetch data from API. Status code: {response.StatusCode}");
            }

            // Parse the JSON response to check if it's a public holiday or a non-working day
            
            var responseData = await response.Content.ReadAsAsync<SholidayApiResponse>();

            // Check if Dagar is null before accessing it
            if (responseData.Dagar == null)
            {
                throw new Exception($"No data found for the specified date: {date}");
            }

            // Use null-conditional operator ?. to avoid NullReferenceException
            var dayInfo = responseData.Dagar.FirstOrDefault(); // Assuming there's only one day in the response

            if (dayInfo == null)
            {
                throw new Exception($"No data found for the specified date: {date}");
            }

            return dayInfo.Holiday == "Ja" || dayInfo.WorkFreeDay == "Ja";
        }


        public int GetTollFee(List<DateTime> timestamps, string vehicleType)
        {
            // Check if any of the timestamps are toll-free using the API
            bool isAnyTollFree = timestamps.Any(timestamp => IsTollFreeDateAsync(timestamp).Result);

            if (isAnyTollFree || IsTollFreeVehicle(vehicleType))
            {
                return 0;
            }

            // Initialize maxTollRate with a default value
            int maxTollRate = 0;

            // Loop through each timestamp
            foreach (var timestamp in timestamps)
            {
                int hour = timestamp.Hour;
                int minute = timestamp.Minute;

                // Find all applicable toll rates for the current hour and minute
                var applicableTollRates = _settings.TollRateOptions
                    .Where(tollRate =>
                        tollRate.Hour == hour &&
                        minute >= tollRate.MinuteStart &&
                        minute <= tollRate.MinuteEnd)
                    .ToList();

                if (applicableTollRates.Count == 0)
                {
                    continue; // No applicable toll rates found for the current timestamp, move to the next timestamp
                }

                // Find the highest toll rate among the applicable rates for the current timestamp
                int maxTollRateForTimestamp = applicableTollRates.Max(tollRate => tollRate.Rate);

                // Update maxTollRate if the current timestamp has a higher toll rate
                if (maxTollRateForTimestamp > maxTollRate)
                {
                    maxTollRate = maxTollRateForTimestamp;
                }
            }

            return maxTollRate;
        }

        private bool IsTollFreeVehicle(string vehicleType)
        {
            if (string.IsNullOrEmpty(vehicleType)) return false;;
            TollFreeVehicles vehicleTypeEnum;

            if (Enum.TryParse(vehicleType, out vehicleTypeEnum))
            {
                return _settings.TollFreeVehicleTypes.Contains(vehicleTypeEnum);
            }
            else
            {
                return false;
            }
        }

    }
}
