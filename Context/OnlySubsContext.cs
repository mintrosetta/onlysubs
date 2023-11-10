using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OnlySubs.Models.db;

#nullable disable

namespace OnlySubs.Context
{
    public partial class OnlySubsContext : DbContext
    {
        public OnlySubsContext()
        {
        }

        public OnlySubsContext(DbContextOptions<OnlySubsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CodeResource> CodeResources { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostsBookmark> PostsBookmarks { get; set; }
        public virtual DbSet<PostsComment> PostsComments { get; set; }
        public virtual DbSet<PostsImage> PostsImages { get; set; }
        public virtual DbSet<PostsLike> PostsLikes { get; set; }
        public virtual DbSet<PostsPrice> PostsPrices { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersFollow> UsersFollows { get; set; }
        public virtual DbSet<UsersPostsSub> UsersPostsSubs { get; set; }
        public virtual DbSet<UsersResource> UsersResources { get; set; }
        public virtual DbSet<UsersRole> UsersRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<CodeResource>(entity =>
            {
                entity.ToTable("CodeResource");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Resource).HasColumnName("resource");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.IsSub).HasColumnName("isSub");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Posts_Users");
            });

            modelBuilder.Entity<PostsBookmark>(entity =>
            {
                entity.ToTable("PostsBookmark");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("postId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsBookmarks)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsBookmark_Posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostsBookmarks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsBookmark_Users");
            });

            modelBuilder.Entity<PostsComment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("postId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsComments_Posts");
            });

            modelBuilder.Entity<PostsImage>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("imageName");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("postId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsImages)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsImages_Posts");
            });

            modelBuilder.Entity<PostsLike>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("postId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostsLikes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsLikes_Posts");
            });

            modelBuilder.Entity<PostsPrice>(entity =>
            {
                entity.HasKey(e => e.PostId);

                entity.ToTable("PostsPrice");

                entity.Property(e => e.PostId).HasColumnName("postId");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.PostsPrice)
                    .HasForeignKey<PostsPrice>(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PostsPrice_Posts");
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.ToTable("RefreshToken");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RefreshToken1)
                    .IsRequired()
                    .HasColumnName("refreshToken");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("birthDate");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(256)
                    .HasColumnName("email");

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("imageName");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("password");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UsersFollow>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsFollowingUserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("isFollowingUserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersFollows)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersFollows_Users");
            });

            modelBuilder.Entity<UsersPostsSub>(entity =>
            {
                entity.ToTable("UsersPostsSub");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PostId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("postId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.UsersPostsSubs)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersPostsSub_Posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersPostsSubs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersPostsSub_Users");
            });

            modelBuilder.Entity<UsersResource>(entity =>
            {
                entity.ToTable("UsersResource");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Krama).HasColumnName("krama");

                entity.Property(e => e.Money).HasColumnName("money");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersResources)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersMoneysKramas_Users");
            });

            modelBuilder.Entity<UsersRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersRoles_Roles");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsersRoles_Users1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
