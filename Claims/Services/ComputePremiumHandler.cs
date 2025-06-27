using Claims.Models;
using Claims.Models.ComputePremium;

namespace Claims.Services
{
    public static class ComputePremiumHandler
    {
        private const decimal _premiumPerDayWithoutMultiplier = 1250m;

        public static ComputePremiumResult ComputePremiumByDate(DateTime startDate, DateTime endDate, CoverType coverType)
        {
            var multiplier = GetMultiplier(coverType);
            var premiumPerDay = _premiumPerDayWithoutMultiplier * multiplier;
            var insuranceLength = (endDate - startDate).TotalDays;
            var totalPremium = 0m;

            for (int day = 0; day <= insuranceLength; day++)
            {
                totalPremium += CalculateDailyPremium(day, premiumPerDay, coverType);
            }

            return new ComputePremiumResult
            {
                ComputePremium = new ComputePremiumResponse
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Total = totalPremium
                }
            };
        }

        private static decimal GetMultiplier(CoverType coverType)
        {
            return coverType switch
            {
                CoverType.Yacht => 1.10m,
                CoverType.PassengerShip => 1.20m,
                CoverType.Tanker => 1.50m,
                _ => 1.30m
            };
        }

        private static decimal CalculateDailyPremium(int day, decimal basePremium, CoverType coverType)
        {
            if (IsFirst30Days(day))
            {
                return basePremium;
            }
            else if (IsFollowing150Days(day))
            {
                var discount = coverType == CoverType.Yacht ? 0.05m : 0.02m;
                return basePremium * (1 - discount);
            }
            else//Remaining days
            {
                var discount = coverType == CoverType.Yacht ? 0.03m : 0.01m;
                return basePremium * (1 - discount);
            }
        }

        private static bool IsFollowing150Days(int day)
        {
            return day == 30 || day < 180;
        }

        private static bool IsFirst30Days(int day)
        {
            return day == 0 || day < 30;
        }
    }
}
