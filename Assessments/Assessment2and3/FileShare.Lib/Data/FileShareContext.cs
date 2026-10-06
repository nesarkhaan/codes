using Microsoft.EntityFrameworkCore;

namespace FileShare.Lib.Data;

// The context belongs in Lib because both Service and Web use its definitions.
public class FileShareContext : DbContext
{
    // Constructor that initializes the FileShareContext with the specified options.
    public FileShareContext(DbContextOptions<FileShareContext> options)
        : base(options)
    {
    }
    // DbSet representing the collection of FileEntity objects in the database.
    public DbSet<FileEntity> Files => Set<FileEntity>();
    // DbSet representing the collection of PeerEntity objects in the database.
    public DbSet<PeerEntity> Peers => Set<PeerEntity>();
}
