using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OrganizationWaterSupplySol;

public partial class OrganizationsWaterSupplyContext : DbContext
{
    public OrganizationsWaterSupplyContext()
    {
    }

    public OrganizationsWaterSupplyContext(DbContextOptions<OrganizationsWaterSupplyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Counter> Counters { get; set; }

    public virtual DbSet<CounterModel> CounterModels { get; set; }

    public virtual DbSet<CountersCheck> CountersChecks { get; set; }

    public virtual DbSet<CountersDatum> CountersData { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<RateOrg> RateOrgs { get; set; }

    public virtual DbSet<ViewOrganizationsCounter> ViewOrganizationsCounters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TRATSEVSKIY\\SQLEXPRESS01;Database=OrganizationsWaterSupply;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Counter>(entity =>
        {
            entity.HasKey(e => e.RegistrationNumber).HasName("PK__Counters__E8864603567FE66E");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.TimeOfInstallation).HasColumnType("date");

            entity.HasOne(d => d.Model).WithMany(p => p.Counters)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Counters_CounterModels");

            entity.HasOne(d => d.Organization).WithMany(p => p.Counters)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Counters_Organizations");
        });

        modelBuilder.Entity<CounterModel>(entity =>
        {
            entity.HasKey(e => e.ModelId).HasName("PK__CounterM__E8D7A1CC12D7DCC6");

            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.Manufacturer).HasMaxLength(50);
            entity.Property(e => e.ModelName).HasMaxLength(50);
        });

        modelBuilder.Entity<CountersCheck>(entity =>
        {
            entity.HasKey(e => e.CountersCheckId).HasName("PK__Counters__49850B5DA52FAC99");

            entity.Property(e => e.CountersCheckId).HasColumnName("CountersCheckID");
            entity.Property(e => e.CheckDate).HasColumnType("date");
            entity.Property(e => e.CheckResult).HasMaxLength(50);

            entity.HasOne(d => d.RegistrationNumberNavigation).WithMany(p => p.CountersChecks)
                .HasForeignKey(d => d.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CountersChecks_Counters");
        });

        modelBuilder.Entity<CountersDatum>(entity =>
        {
            entity.HasKey(e => e.CountersDataId).HasName("PK__Counters__60A1F09B7549BC10");

            entity.Property(e => e.CountersDataId).HasColumnName("CountersDataID");
            entity.Property(e => e.DataCheckDate).HasColumnType("date");
            entity.Property(e => e.RateId).HasColumnName("RateID");

            entity.HasOne(d => d.Rate).WithMany(p => p.CountersData)
                .HasForeignKey(d => d.RateId)
                .HasConstraintName("FK_CountersData_RateOrg");

            entity.HasOne(d => d.RegistrationNumberNavigation).WithMany(p => p.CountersData)
                .HasForeignKey(d => d.RegistrationNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CountersData_Counters");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("PK__Organiza__CADB0B72595BF4CA");

            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.DirectorFullname).HasMaxLength(50);
            entity.Property(e => e.DirectorPhone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.OrgName).HasMaxLength(50);
            entity.Property(e => e.OwnershipType).HasMaxLength(50);
            entity.Property(e => e.ResponsibleFullname).HasMaxLength(50);
            entity.Property(e => e.ResponsiblePhone)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Rate>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__Rates__58A7CCBC0190846D");

            entity.Property(e => e.RateId).HasColumnName("RateID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RateName).HasMaxLength(50);
        });

        modelBuilder.Entity<RateOrg>(entity =>
        {
            entity.HasKey(e => e.RateId).HasName("PK__RateOrg__58A7CCBCB3D0241A");

            entity.ToTable("RateOrg");

            entity.Property(e => e.RateId)
                .ValueGeneratedNever()
                .HasColumnName("RateID");
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");

            entity.HasOne(d => d.Organization).WithMany(p => p.RateOrgs)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_RateOrg_Organizations");

            entity.HasOne(d => d.Rate).WithOne(p => p.RateOrg)
                .HasForeignKey<RateOrg>(d => d.RateId)
                .HasConstraintName("FK_RateOrg_Rates");
        });

        modelBuilder.Entity<ViewOrganizationsCounter>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("View_OrganizationsCounters");

            entity.Property(e => e.Adress).HasMaxLength(50);
            entity.Property(e => e.DirectorFullname).HasMaxLength(50);
            entity.Property(e => e.DirectorPhone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer).HasMaxLength(50);
            entity.Property(e => e.ModelId).HasColumnName("ModelID");
            entity.Property(e => e.ModelName).HasMaxLength(50);
            entity.Property(e => e.OrgName).HasMaxLength(50);
            entity.Property(e => e.OrganizationId).HasColumnName("OrganizationID");
            entity.Property(e => e.OwnershipType).HasMaxLength(50);
            entity.Property(e => e.ResponsibleFullname).HasMaxLength(50);
            entity.Property(e => e.ResponsiblePhone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.TimeOfInstallation).HasColumnType("date");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
