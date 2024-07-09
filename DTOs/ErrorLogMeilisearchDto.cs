namespace OPL_grafana_meilisearch.DTOs
{
    public class ErrorLogMeilisearchDto
    {
        public string CodeId { get; set; }
        
        public string Api { get; set; }
        public string Message { get; set; }

        public string dateTime { get; set; }
    }
}