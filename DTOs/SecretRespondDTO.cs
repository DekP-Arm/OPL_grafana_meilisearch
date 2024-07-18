using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OPL_grafana_meilisearch.DTOs
{
    public class SecretRespondDTO
    {
        public List<Secret> Secrets { get; set; }
        public List<Import> Imports { get; set; }
    }
    public class Secret
{
    public string Id { get; set; }
    public string _Id { get; set; }
    public string Workspace { get; set; }
    public string Environment { get; set; }
    public int Version { get; set; }
    public string Type { get; set; }
    public string SecretKey { get; set; }
    public string SecretValue { get; set; }
    public string SecretComment { get; set; }
    public string SecretPath { get; set; }
}
    public class Import
{
    public string SecretPath { get; set; }
    public string Environment { get; set; }
    public string FolderId { get; set; }
    public List<Secret> Secrets { get; set; }
}
}