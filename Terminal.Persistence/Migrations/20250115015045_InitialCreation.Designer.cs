﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Terminal.Persistence.Data;

#nullable disable

namespace Terminal.Persistence.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250115015045_InitialCreation")]
    partial class InitialCreation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Terminal.Domain.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Definition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<int>("DefinitionState")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DownvoteCount")
                        .HasColumnType("int");

                    b.Property<string>("EnglishContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnglishTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GeorgianContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GeorgianTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReferencesId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("UpvoteCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("DefinitionId");

                    b.HasIndex("ReferencesId");

                    b.ToTable("Definitions");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Reference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EnglishTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GeorgianTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReferenceId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("References");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReportState")
                        .HasColumnType("int");

                    b.Property<int>("RevieverId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Terminal.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserRank")
                        .HasColumnType("int");

                    b.Property<int>("UserStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Terminal.Domain.Utilities.Vote", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("DefinitionId")
                        .HasColumnType("int");

                    b.Property<bool>("Likes")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "DefinitionId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Comment", b =>
                {
                    b.HasOne("Terminal.Domain.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Definition", b =>
                {
                    b.HasOne("Terminal.Domain.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Terminal.Domain.Models.Definition", null)
                        .WithMany("Similars")
                        .HasForeignKey("DefinitionId");

                    b.HasOne("Terminal.Domain.Models.Reference", "References")
                        .WithMany()
                        .HasForeignKey("ReferencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("References");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Report", b =>
                {
                    b.HasOne("Terminal.Domain.Models.User", "Author")
                        .WithMany("Reports")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Terminal.Domain.Models.Definition", b =>
                {
                    b.Navigation("Similars");
                });

            modelBuilder.Entity("Terminal.Domain.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Reports");
                });
#pragma warning restore 612, 618
        }
    }
}
