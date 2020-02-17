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
        public virtual DbSet<DbEditTaskResult> DbEditTaskResult { get; set; }
        public virtual DbSet<DbImg> DbImg { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbEditTaskProgress>(entity =>
            {
                entity.HasKey(e => new { e.TaskId, e.GroupId })
                    .HasName("PK__DbEditTa__CD20E687853450D5");
            });

            modelBuilder.Entity<DbEditTaskResult>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tmp_ms_x__3214EC06C4139393")
                    .IsClustered(false);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.Image).IsRequired();

                entity.HasOne(d => d.DbEditTaskProgress)
                    .WithMany(p => p.DbEditTaskResult)
                    .HasForeignKey(d => new { d.TaskId, d.GroupId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DbEditTaskResult_DbEditTaskProgress");
            });

            modelBuilder.Entity<DbImg>(entity =>
            {
                entity.HasKey(e => e.ImgId)
                    .HasName("PK__DbImg__352F54F3B6D8F838");

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
