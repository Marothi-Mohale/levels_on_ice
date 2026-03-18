using System.ComponentModel.DataAnnotations;

namespace LevelsOnIceSalon.Web.Options;

public class DataBackupOptions
{
    public const string SectionName = "DataBackups";

    public bool Enabled { get; set; } = true;

    [Required]
    public string DirectoryPath { get; set; } = "App_Data/Backups";

    [Range(1, 168)]
    public int IntervalHours { get; set; } = 24;

    [Range(1, 365)]
    public int RetentionDays { get; set; } = 30;

    public bool RunOnStartup { get; set; } = true;
}
