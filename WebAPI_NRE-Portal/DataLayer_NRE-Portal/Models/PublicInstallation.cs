namespace DataLayer_NRE_Portal.Models
{
    /// <summary>
    /// Official power plants / public facilities imported from datasets.
    /// </summary>
    public class PublicInstallation : InstallationBase
    {
        public string? OperatorName { get; set; }     // utility/owner
        public string? Municipality { get; set; }     // finer region
        public string? SourceRef { get; set; }        // dataset row id or code
        public bool IsActive { get; set; } = true;
    }
}
