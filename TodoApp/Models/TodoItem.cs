using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsDone { get; set; } = false;
    }
}