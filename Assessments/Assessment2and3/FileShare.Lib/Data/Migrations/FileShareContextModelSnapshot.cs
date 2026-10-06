using System;
using FileShare.Lib.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace FileShare.Lib.Data.Migrations;

[DbContext(typeof(FileShareContext))]
// This class represents a snapshot of the model for the FileShareContext, used by Entity Framework Core to manage migrations and database schema changes.
partial class FileShareContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {

        modelBuilder.HasAnnotation("ProductVersion", "10.0.10");
        // Define the entity for FileEntity with its properties and configurations.
        modelBuilder.Entity("FileShare.Lib.Data.FileEntity", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<DateTime>("AddedDate").HasColumnType("TEXT");
            b.Property<bool>("Completed").HasColumnType("INTEGER");
            b.Property<int>("CompletedBlocks").HasColumnType("INTEGER");
            b.Property<string>("FileName").IsRequired().HasColumnType("TEXT");
            b.Property<long>("FileSize").HasColumnType("INTEGER");
            b.Property<string>("Identifier").IsRequired().HasColumnType("TEXT");
            b.Property<string>("TargetPath").IsRequired().HasColumnType("TEXT");
            b.Property<int>("TotalBlocks").HasColumnType("INTEGER");
            b.HasKey("Id");
            b.ToTable("Files");
        });
        // Define the entity for PeerEntity with its properties and configurations.
        modelBuilder.Entity("FileShare.Lib.Data.PeerEntity", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<bool>("Connected").HasColumnType("INTEGER");
            b.Property<string>("IPAddress").IsRequired().HasColumnType("TEXT");
            b.Property<DateTime>("LastSeen").HasColumnType("TEXT");
            b.Property<string>("Nickname").IsRequired().HasColumnType("TEXT");
            b.Property<int>("Port").HasColumnType("INTEGER");
            b.Property<string>("RuntimeId").IsRequired().HasColumnType("TEXT");
            b.HasKey("Id");
            b.ToTable("Peers");
        });

    }
}
