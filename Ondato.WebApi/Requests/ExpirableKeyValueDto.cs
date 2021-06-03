namespace ondato.Requests
{
    public class ExpirableKeyValueDto : KeyValueDto
    {
        public int? ExpireInSeconds { get; set; }
    }
}