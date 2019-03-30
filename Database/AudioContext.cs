using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class AudioContext : DbContext
    {
        public AudioContext(DbContextOptions<AudioContext> options) : base(options)
        {
 
        }
        
        public DbSet<AudioFile> AudioFiles { get; set; }
    }
}