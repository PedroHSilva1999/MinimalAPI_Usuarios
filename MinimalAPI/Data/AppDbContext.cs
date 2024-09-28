using Microsoft.EntityFrameworkCore;
using MinimalAPI.Models;
using System.Data;

namespace MinimalAPI.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) { }
           
        public DbSet<UsuariosModel> Usuarios { get; set; }
    }
}
