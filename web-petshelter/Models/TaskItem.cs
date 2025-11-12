using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_petshelter.Models;

public enum TaskStatus { Todo = 0, Doing = 1, Done = 2 }

public class TaskItem
{
    public int Id { get; set; }

    [Required, StringLength(160)]
    public string Title { get; set; } = "";

    [StringLength(2000)]
    public string? Description { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    public DateTime? DueAt { get; set; }

    // Проста пріоритизація: 1(високий)–5(низький)
    [Range(1, 5)]
    public int Priority { get; set; } = 3;

    // Призначено (якщо є модель користувача — підставиш FK; поки просто string)
    [StringLength(100)]
    public string? AssigneeUserId { get; set; }

    // Прив'язки (опційні)
    public int? AnimalId { get; set; }
    public int? ShelterId { get; set; }
    public int? AdoptionRequestId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // навігаційні (необов'язково, якщо моделі вже є)
    public Animal? Animal { get; set; }
    public Shelter? Shelter { get; set; }
    // public AdoptionRequest? AdoptionRequest { get; set; }
}
