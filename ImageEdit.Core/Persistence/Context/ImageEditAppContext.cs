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

        public virtual DbSet<DbImg> DbImg { get; set; }
        public virtual DbSet<DbImgTaskProgress> DbImgTaskProgress { get; set; }
        public virtual DbSet<DbImgTaskResult> DbImgTaskResult { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=ImageEdit.Db;User ID=sa;Password=qwerty");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbImg>(entity =>
            {
                entity.HasKey(e => e.ImgId)
                    .HasName("PK__DbImg__352F54F35B3B777E");

                entity.Property(e => e.ImgId).ValueGeneratedNever();

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Image).IsRequired();
            });

            modelBuilder.Entity<DbImgTaskProgress>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.TaskId })
                    .HasName("PK__DbImgTas__135C67F1A7A8B382");

                entity.HasOne(d => d.DbImgTaskResult)
                    .WithOne(p => p.DbImgTaskProgress)
                    .HasForeignKey<DbImgTaskProgress>(d => new { d.GroupId, d.TaskId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DbImgTaskProgress_DbImgTaskResult");
            });

            modelBuilder.Entity<DbImgTaskResult>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.TaskId })
                    .HasName("PK__DbImgTas__135C67F1B8AF74B3");

                entity.Property(e => e.Extension)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
