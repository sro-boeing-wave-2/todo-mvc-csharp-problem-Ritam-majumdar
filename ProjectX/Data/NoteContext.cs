using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Models
{
    public class NoteContext : DbContext
    {
        public NoteContext (DbContextOptions<NoteContext> options)
            : base(options)
        {
        }

        public DbSet<ProjectX.Models.Note> Note { get; set; }
        public DbSet<ProjectX.Models.CheckList> CheckList { get; set; }
        public DbSet<ProjectX.Models.Label> Label { get; set; }
    }
}
