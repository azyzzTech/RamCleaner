namespace RamCleaner.WinForms.Core.Models;

internal class ProcessModel
{
    internal int Id { get; set; }
    internal string Name { get; set; }
    internal string MemoryUsage { get; set; }
    internal bool IsSelected { get; set; }
    internal long MemoryUsageBytes { get; set; }
    internal string MemoryUsageDisplay { get; set; }
}