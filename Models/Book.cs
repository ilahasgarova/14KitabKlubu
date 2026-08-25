using System.ComponentModel.DataAnnotations;

namespace KitabKlubu.Models;

public class Book
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Price { get; set; } = "";
    public string Description { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public string ContactInfo { get; set; } = "";
    public string SubmittedBy { get; set; } = "";
    public bool IsApproved { get; set; }
    public string DateAdded { get; set; } = "";
}