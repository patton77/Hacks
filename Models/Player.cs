using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hacks.Models;

public partial class Player
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public DateTime? Dob { get; set; }

    public int? TeamId { get; set; }

    public int? PositionId { get; set; }

    public string? MothersName { get; set; }

    [NotMapped]
    public List<SelectListItem>? teams { get; set; }

    [NotMapped]
    public string? CurrentTeamString { get; set; }


}
