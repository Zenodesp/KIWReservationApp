using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KIWReservationApp.Models;

namespace KIWReservationApp.Data
{
    public class KIWReservationAppContext : DbContext
    {
        public KIWReservationAppContext (DbContextOptions<KIWReservationAppContext> options)
            : base(options)
        {
        }


        

        // loads dummy data into the database

        public static async Task DataInitialiser(KIWReservationAppContext context)
        {
            if(!context.Material.Any())
            {
                Material m = new Material
                {
                    Type = "Dummy Item",
                    PickupTime = null,
                    IsPickedUp = false,
                    ReturnTime = null
                };

                context.Material.Add(m);
                   
                await context.SaveChangesAsync();
            }
        }

        public DbSet<KIWReservationApp.Models.Material> Material { get; set; } = default!;

        public DbSet<KIWReservationApp.Models.Student> Student { get; set; } = default!;
    }
}
