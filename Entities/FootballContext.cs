using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace football.Entities;

public partial class FootballContext : DbContext
{
    public FootballContext()
    {
    }

    public FootballContext(DbContextOptions<FootballContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Info> Infos { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Nation> Nations { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Train> Trains { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=football;uid=root;pwd=hinihao123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_bin")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PRIMARY");

            entity.ToTable("chat");

            entity.HasIndex(e => e._1Id, "1_id");

            entity.HasIndex(e => e._2Id, "2_id");

            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e._1Id).HasColumnName("1_id");
            entity.Property(e => e._2Id).HasColumnName("2_id");

            entity.HasOne(d => d._1).WithMany(p => p.Chat_1s)
                .HasForeignKey(d => d._1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chat_ibfk_1");

            entity.HasOne(d => d._2).WithMany(p => p.Chat_2s)
                .HasForeignKey(d => d._2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chat_ibfk_2");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PRIMARY");

            entity.ToTable("club");

            entity.Property(e => e.ClubId).HasColumnName("club_id");
            entity.Property(e => e.AvatarId)
                .HasMaxLength(256)
                .HasColumnName("avatar_id");
            entity.Property(e => e.ClubName)
                .HasMaxLength(255)
                .HasColumnName("club_name");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasMany(d => d.Players).WithMany(p => p.Clubs)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerClub",
                    r => r.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_club_ibfk_2"),
                    l => l.HasOne<Club>().WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_club_ibfk_1"),
                    j =>
                    {
                        j.HasKey("ClubId", "PlayerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("player_club");
                        j.HasIndex(new[] { "PlayerId" }, "player_id");
                        j.IndexerProperty<int>("ClubId").HasColumnName("club_id");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("player_id");
                    });

            entity.HasMany(d => d.Users).WithMany(p => p.Clubs)
                .UsingEntity<Dictionary<string, object>>(
                    "UserClub",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_club_ibfk_1"),
                    l => l.HasOne<Club>().WithMany()
                        .HasForeignKey("ClubId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("user_club_ibfk_2"),
                    j =>
                    {
                        j.HasKey("ClubId", "UserId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("user_club");
                        j.HasIndex(new[] { "UserId" }, "user_id");
                        j.IndexerProperty<int>("ClubId").HasColumnName("club_id");
                        j.IndexerProperty<int>("UserId").HasColumnName("user_id");
                    });
        });

        modelBuilder.Entity<Info>(entity =>
        {
            entity.HasKey(e => e.InfoId).HasName("PRIMARY");

            entity.ToTable("info");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.InfoId).HasColumnName("info_id");
            entity.Property(e => e.Addition)
                .HasMaxLength(255)
                .HasColumnName("addition");
            entity.Property(e => e.EndYear).HasColumnName("end_year");
            entity.Property(e => e.StartYear).HasColumnName("start_year");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Infos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("info_ibfk_1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("message");

            entity.HasIndex(e => e.ChatId, "chat_id");

            entity.HasIndex(e => e.From, "from");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.From).HasColumnName("from");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .HasConstraintName("message_ibfk_2");

            entity.HasOne(d => d.FromNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.From)
                .HasConstraintName("message_ibfk_1");
        });

        modelBuilder.Entity<Nation>(entity =>
        {
            entity.HasKey(e => e.NationId).HasName("PRIMARY");

            entity.ToTable("nation");

            entity.Property(e => e.NationId).HasColumnName("nation_id");
            entity.Property(e => e.AvatarId)
                .HasMaxLength(256)
                .HasColumnName("avatar_id");
            entity.Property(e => e.NationName)
                .HasMaxLength(255)
                .HasColumnName("nation_name");
            entity.Property(e => e.Score).HasColumnName("score");

            entity.HasMany(d => d.Players).WithMany(p => p.Nations)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerNation",
                    r => r.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_nation_ibfk_2"),
                    l => l.HasOne<Nation>().WithMany()
                        .HasForeignKey("NationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_nation_ibfk_1"),
                    j =>
                    {
                        j.HasKey("NationId", "PlayerId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("player_nation");
                        j.HasIndex(new[] { "PlayerId" }, "player_id");
                        j.IndexerProperty<int>("NationId").HasColumnName("nation_id");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("player_id");
                    });
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PRIMARY");

            entity.ToTable("player");

            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.AvatarId)
                .HasMaxLength(256)
                .HasColumnName("avatar_id");
            entity.Property(e => e.Defence).HasColumnName("defence");
            entity.Property(e => e.Dribble).HasColumnName("dribble");
            entity.Property(e => e.Energy).HasColumnName("energy");
            entity.Property(e => e.ExpireDate).HasColumnName("expire_date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pass).HasColumnName("pass");
            entity.Property(e => e.Position)
                .HasMaxLength(255)
                .HasColumnName("position");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.Shot).HasColumnName("shot");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.Vel).HasColumnName("vel");

            entity.HasMany(d => d.Trains).WithMany(p => p.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerTrain",
                    r => r.HasOne<Train>().WithMany()
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_train_ibfk_2"),
                    l => l.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("player_train_ibfk_1"),
                    j =>
                    {
                        j.HasKey("PlayerId", "TrainId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("player_train");
                        j.HasIndex(new[] { "TrainId" }, "train_id");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("player_id");
                        j.IndexerProperty<int>("TrainId").HasColumnName("train_id");
                    });
        });

        modelBuilder.Entity<Train>(entity =>
        {
            entity.HasKey(e => e.TrainId).HasName("PRIMARY");

            entity.ToTable("train");

            entity.Property(e => e.TrainId).HasColumnName("train_id");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TrainDate).HasColumnName("train_date");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transfer");

            entity.HasIndex(e => e.From, "from");

            entity.HasIndex(e => e.To, "to");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.From).HasColumnName("from");
            entity.Property(e => e.PlayerId).HasColumnName("player_id");
            entity.Property(e => e.To).HasColumnName("to");

            entity.HasOne(d => d.FromNavigation).WithMany(p => p.TransferFromNavigations)
                .HasForeignKey(d => d.From)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transfer_ibfk_1");

            entity.HasOne(d => d.ToNavigation).WithMany(p => p.TransferToNavigations)
                .HasForeignKey(d => d.To)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transfer_ibfk_2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.AvatarId)
                .HasMaxLength(256)
                .HasColumnName("avatar_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Pwd)
                .HasMaxLength(255)
                .HasColumnName("pwd");
            entity.Property(e => e.TeleNumber)
                .HasMaxLength(255)
                .HasColumnName("tele_number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
