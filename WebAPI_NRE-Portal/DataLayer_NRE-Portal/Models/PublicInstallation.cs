namespace DataLayer_NRE_Portal.Models
{
    /// <summary>
    /// Official power plants / public facilities imported from datasets.
    /// </summary>
    public class PublicInstallation : InstallationBase
    {
        public string? OperatorName { get; set; }     
        public string? Municipality { get; set; }     
        public string? SourceRef { get; set; }        // dataset row id or code ( just i case we need to find it in the OG source file )
        public bool IsActive { get; set; } = true;
    }
}
