namespace Claims.Models.ComputePremium
{
    public class ComputePremiumResult
    {
        public ComputePremiumResponse? ComputePremium { get; set; }
        public bool IsSuccess => Errors.Count() == 0;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
