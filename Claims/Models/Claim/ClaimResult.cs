namespace Claims.Models.Claims
{
    public class ClaimResult
    {
        public ClaimResponse? NewClaim { get; set; }
        public bool IsSuccess => Errors.Count == 0;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
