﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CourseWork.Migrations
{
    [DbContext(typeof(MyAppDbContext))]
    partial class MyAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CourseWork.Modules.Admin.Entity.AdminEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("CourseWork.Modules.Blogs.Entity.BlogComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BlogEntityId")
                        .HasColumnType("int");

                    b.Property<int?>("DownVote")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UpVote")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlogEntityId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("CourseWork.Modules.Blogs.Entity.BlogEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("DownVote")
                        .HasColumnType("int");

                    b.Property<string>("ImgUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UpVote")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("CourseWork.Modules.User.Entity.UserEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CourseWork.Modules.Votes.Entity.VoteEntity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsUpVote")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("BlogId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("CourseWork.Modules.Blogs.Entity.BlogComment", b =>
                {
                    b.HasOne("CourseWork.Modules.Blogs.Entity.BlogEntity", "BlogEntity")
                        .WithMany("Comments")
                        .HasForeignKey("BlogEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlogEntity");
                });

            modelBuilder.Entity("CourseWork.Modules.Blogs.Entity.BlogEntity", b =>
                {
                    b.OwnsOne("CourseWork.Modules.Blogs.Entity.UserInfo", "PostUser", b1 =>
                        {
                            b1.Property<int>("BlogEntityid")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.HasKey("BlogEntityid");

                            b1.ToTable("Blogs");

                            b1.WithOwner()
                                .HasForeignKey("BlogEntityid");
                        });

                    b.Navigation("PostUser")
                        .IsRequired();
                });

            modelBuilder.Entity("CourseWork.Modules.Votes.Entity.VoteEntity", b =>
                {
                    b.HasOne("CourseWork.Modules.Blogs.Entity.BlogEntity", "Blog")
                        .WithMany("Votes")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CourseWork.Modules.Blogs.Entity.UserInfo", "VoteUser", b1 =>
                        {
                            b1.Property<int>("VoteEntityid")
                                .HasColumnType("int");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.HasKey("VoteEntityid");

                            b1.ToTable("Votes");

                            b1.WithOwner()
                                .HasForeignKey("VoteEntityid");
                        });

                    b.Navigation("Blog");

                    b.Navigation("VoteUser")
                        .IsRequired();
                });

            modelBuilder.Entity("CourseWork.Modules.Blogs.Entity.BlogEntity", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
