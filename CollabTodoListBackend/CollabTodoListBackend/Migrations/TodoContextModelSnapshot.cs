﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebEngineering01_ASP.NetCore.Models;

namespace CollabTodoListBackend.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<Guid>("ListID");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("Until");

                    b.HasKey("Id");

                    b.HasIndex("ListID");

                    b.ToTable("TodoItem");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<Guid>("OwnerID");

                    b.HasKey("Id");

                    b.HasIndex("OwnerID");

                    b.ToTable("TodoList");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoListUser", b =>
                {
                    b.Property<Guid>("CollaboratorID");

                    b.Property<Guid>("TodoListID");

                    b.HasKey("CollaboratorID", "TodoListID");

                    b.HasIndex("TodoListID");

                    b.ToTable("TodoListUser");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("MailAdress")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoItem", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.TodoList", "List")
                        .WithMany("TodoItems")
                        .HasForeignKey("ListID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoList", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebEngineering01_ASP.NetCore.Models.TodoListUser", b =>
                {
                    b.HasOne("WebEngineering01_ASP.NetCore.Models.User", "Collaborator")
                        .WithMany("Lists")
                        .HasForeignKey("CollaboratorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebEngineering01_ASP.NetCore.Models.TodoList", "Todolist")
                        .WithMany("Collaborators")
                        .HasForeignKey("TodoListID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
