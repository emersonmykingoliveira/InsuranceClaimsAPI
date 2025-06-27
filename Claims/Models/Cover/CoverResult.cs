namespace Claims.Models.Cover;

public class CoverResult
{
    public CoverResponse? NewCover { get; set; }
    public bool IsSuccess => Errors.Count == 0;
    public List<string> Errors { get; set; } = new List<string>();
}
