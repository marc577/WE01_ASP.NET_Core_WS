﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebEngineering01_ASP.NetCore.Models;

namespace WebEngineering01_ASP.NetCore.Migrations
{
    [DbContext(typeof(TodoContext))]
    [Migration("20180222101508_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<long>("ListId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("Until");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("TodoItem");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long>("OwnerId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("TodoList");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MailAdress")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("SurName")
                        .IsRequired();

                    b.Property<long?>("TodoListId");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoItem", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.TodoList", "List")
                        .WithMany("TodoItems")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoList", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.User", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.TodoList")
                        .WithMany("Collaborators")
                        .HasForeignKey("TodoListId");
                });
#pragma warning restore 612, 618
        }
    }
}
