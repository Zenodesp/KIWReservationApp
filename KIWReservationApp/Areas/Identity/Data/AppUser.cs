using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KIWReservationApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    [Display(Name = "Voornaam")]
    public string? FirstName { get; set; }
    [Display(Name = "Achternaam")]
    public string? LastName { get; set; }
}

