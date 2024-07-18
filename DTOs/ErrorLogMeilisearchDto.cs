namespace OPL_grafana_meilisearch.DTOs
{
    public class ErrorLogMeilisearchDto
    {
        public string Id { get; set; }
        public int CodeError { get; set; }
        
        public string Api { get; set; }
        public string Message { get; set; }

        public string dateTime { get; set; }
    }
}