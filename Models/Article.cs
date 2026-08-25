using System.ComponentModel.DataAnnotations;

namespace KitabKlubu.Models;

public class Article
{
    [Key]
    public int Id { get; set; }
    public string Icon { get; set; } = "";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string Content { get; set; } = "";
    public string Date { get; set; } = "";
}