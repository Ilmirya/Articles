using Microsoft.EntityFrameworkCore;

namespace Articles.Infrastructure.DataAccess.DbContext;

internal class ArticleDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Section> Sections { get; set; }
    
    public ArticleDbContext(DbContextOptions<ArticleDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(a => a.Name).HasMaxLength(256).IsRequired();
            entity.Property(a => a.CreatedAt).IsRequired();
            
            entity.HasOne(a => a.Section)
                .WithMany(s => s.Articles)
                .HasForeignKey(a => a.SectionId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasIndex(a => a.SectionId);
        });
        

        modelBuilder.Entity<Section>(entity =>
        {
            entity.Property(s => s.Name).HasMaxLength(256).IsRequired();
        });
    }

}