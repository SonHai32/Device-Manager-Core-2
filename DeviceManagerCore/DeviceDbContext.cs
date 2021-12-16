using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DeviceManagerCore.Models;
using System.Reflection;

#nullable disable

namespace DeviceManagerCore
{
    public partial class DeviceDbContext : DbContext
    {

        public string DbPath { get; }

        public DeviceDbContext()
        {
            var path = System.IO.Directory.GetCurrentDirectory();
            DbPath = System.IO.Path.Join(path, "devices.db");

        }



        public DeviceDbContext(DbContextOptions<DeviceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Devices { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Server=DESKTOP-7AKNHIG\\SQLEXPRESS;Database=db_device;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("devices");

                entity.HasIndex(e => e.DeviceName, "UQ__devices__947F6E926B8BA0CB")
                    .IsUnique();

                entity.Property(e => e.DeviceId).HasColumnName("deviceId");

                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("deviceName");

                entity.Property(e => e.DevicePrice).HasColumnName("devicePrice");

                entity.Property(e => e.DeviceQuantity).HasColumnName("deviceQuantity");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
