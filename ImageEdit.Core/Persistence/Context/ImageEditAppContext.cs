using ImageEdit.Core.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageEdit.Core.Persistence.Context
{
    public partial class ImageEditAppContext : DbContext
    {
        public ImageEditAppContext()
        {
        }

        public ImageEditAppContext(DbContextOptions<ImageEditAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DbEditTaskProgress> DbEditTaskProgress { get; set; }
        public virtual DbSet<DbImg> DbImg { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbEditTaskProgress>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.GroupId })
                    .HasName("PK__DbEditTa__CD20E68775BB7B8E");
            });

            modelBuilder.Entity<DbImg>(entity =>
            {
                entity.HasKey(e => e.ImgId)
                    .HasName("PK__DbImg__352F54F3BDDE343E");

                entity.Property(e => e.ImgId).ValueGeneratedNever();

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.Image).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
