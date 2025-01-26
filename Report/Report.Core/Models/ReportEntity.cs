namespace Report.Core.Models;

public class ReportEntity
{
    public int Id { get; set; }
    
    /// <summary>
    /// Наименование
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Дата с
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Дата по
    /// </summary>
    public DateTime EndDate { get; set; }
}