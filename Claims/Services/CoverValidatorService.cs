using Claims.Models.Cover;

namespace Claims.Services
{
    public class CoverValidatorService
    {
        public List<string> ValidateRequest(CoverRequest coverRequest)
        {
            return ValidateRequestedDates(coverRequest.StartDate, coverRequest.EndDate);
        }

        public List<string> ValidateRequestedDates(DateTime startDate, DateTime endDate)
        {
            List<string> errors = new List<string>();

            if (IsEndDateBeforeStartDate(startDate, endDate))
                errors.Add("End date must be later than the start date");

            if (StartDateInThePast(startDate, endDate))
                errors.Add("StartDate cannot be in the past");

            if (ExceedsOneYear(startDate, endDate))
                errors.Add("Total insurance period cannot exceed 1 year");

            return errors;
        }

        private bool IsEndDateBeforeStartDate(DateTime startDate, DateTime endDate)
        {
            return endDate < startDate;
        }

        private bool StartDateInThePast(DateTime startDate, DateTime endDate)
        {
            return startDate.Date < DateTime.Now.Date;
        }

        private bool ExceedsOneYear(DateTime startDate, DateTime endDate)
        {
            return endDate > startDate.AddYears(1);
        }
    }
}
