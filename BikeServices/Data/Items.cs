﻿using System.ComponentModel.DataAnnotations;

namespace BikeServices.Data;
public class Items
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Please provide the task name.")]
    public string ItemName { get; set; }
    public int Quanity { get; set; }
    public string LastTakenOut { get; set; }
}
