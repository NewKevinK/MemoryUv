using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Data
{
    public partial class MemoryModel : DbContext
    {
        public MemoryModel()
            : base("name=MemoryModel")
        {
        }

        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<FriendRequest> FriendRequests { get; set; }
        public virtual DbSet<StatisticUser> StatisticsUser { get; set; }
        public virtual DbSet<UserGame> UsersGame { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FriendRequest>()
                .Property(e => e.requestStatus)
                .IsUnicode(false);

            modelBuilder.Entity<StatisticUser>()
                .Property(e => e.nameTag)
                .IsFixedLength();

            modelBuilder.Entity<UserGame>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<UserGame>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<UserGame>()
                .Property(e => e.nametag)
                .IsUnicode(false);

            modelBuilder.Entity<UserGame>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<UserGame>()
                .HasMany(e => e.Friend)
                .WithRequired(e => e.UserGame)
                .HasForeignKey(e => e.idFriend)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGame>()
                .HasMany(e => e.FriendRequest)
                .WithRequired(e => e.UserGame)
                .HasForeignKey(e => e.idApplicant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserGame>()
                .HasMany(e => e.StatisticUser)
                .WithRequired(e => e.UserGame)
                .HasForeignKey(e => e.idUser)
                .WillCascadeOnDelete(false);
        }
    }
}
