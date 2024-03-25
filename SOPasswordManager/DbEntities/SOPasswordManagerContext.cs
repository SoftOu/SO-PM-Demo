using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SOPasswordManager.DbEntities
{
    public partial class SOPasswordManagerContext : DbContext
    {
        public SOPasswordManagerContext()
        {
        }

        public SOPasswordManagerContext(DbContextOptions<SOPasswordManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<ClientContacts> ClientContacts { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<ClientUser> ClientUser { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<ProjectData> ProjectData { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUser { get; set; }
        public virtual DbSet<ProviderContact> ProviderContact { get; set; }
        public virtual DbSet<ProviderContactDetail> ProviderContactDetail { get; set; }
        public virtual DbSet<Providers> Providers { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=163.172.202.4\\MSSQLSERVER2014;Database=elaunch_password_manager;user id=lawyeruser;password=lawyer!@#789");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "lawyeruser");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City", "SO-PM");

                entity.Property(e => e.CityId).HasColumnName("City_ID");

                entity.Property(e => e.CityName)
                    .HasColumnName("City_Name")
                    .HasMaxLength(50);

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.City)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_City_Country");
            });

            modelBuilder.Entity<ClientContacts>(entity =>
            {
                entity.HasKey(e => e.ClientContactId);

                entity.ToTable("ClientContacts", "SO-PM");

                entity.Property(e => e.ClientContactId).HasColumnName("ClientContactID");

                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.ContactId).HasColumnName("ContactID");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.ClientContacts)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_ClientContacts_Clients");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ClientContacts)
                    .HasForeignKey(d => d.ContactId)
                    .HasConstraintName("FK_ClientContacts_Contacts");
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.HasKey(e => e.ClientId);

                entity.ToTable("Clients", "SO-PM");

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.CityId).HasColumnName("City_ID");

                entity.Property(e => e.ClientName).HasMaxLength(250);

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("Date_Updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_Clients_City");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Clients_County");
            });

            modelBuilder.Entity<ClientUser>(entity =>
            {
                entity.ToTable("ClientUser", "SO-PM");

                entity.Property(e => e.ClientUserId).HasColumnName("ClientUser_ID");

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("Date_Updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(320);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.ClientUser)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_ClientUser_UserRole");
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.HasKey(e => e.ContactId);

                entity.ToTable("Contacts", "SO-PM");

                entity.Property(e => e.ContactId).HasColumnName("Contact_ID");

                entity.Property(e => e.Email1).HasMaxLength(320);

                entity.Property(e => e.Email2).HasMaxLength(320);

                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.PhoneNumber1).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber2).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("County", "SO-PM");

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.CountyName)
                    .HasColumnName("County_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ProjectData>(entity =>
            {
                entity.HasKey(e => e.ProjectUserId);

                entity.ToTable("ProjectData", "SO-PM");

                entity.Property(e => e.ProjectUserId).HasColumnName("ProjectUser_ID");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("Date_Updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(320);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.ProjectId).HasColumnName("Project_ID");

                entity.Property(e => e.Url).HasColumnName("URL");

                entity.Property(e => e.UserName).HasColumnName("User_Name");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectData)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectUser_Projects");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.ToTable("Projects", "SO-PM");

                entity.Property(e => e.ProjectId).HasColumnName("Project_ID");

                entity.Property(e => e.ClientId).HasColumnName("Client_ID");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("Date_Updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProjectName).HasColumnName("Project_Name");

                entity.Property(e => e.ProviderId).HasColumnName("Provider_ID");

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            });

            modelBuilder.Entity<ProjectUser>(entity =>
            {
                entity.ToTable("ProjectUser", "SO-PM");

                entity.Property(e => e.ProjectUserId).HasColumnName("ProjectUser_ID");

                entity.Property(e => e.ProjectId).HasColumnName("Project_ID");

                entity.Property(e => e.SytemUserId).HasColumnName("SytemUser_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectUser)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectUser_Projects1");

                entity.HasOne(d => d.SytemUser)
                    .WithMany(p => p.ProjectUser)
                    .HasForeignKey(d => d.SytemUserId)
                    .HasConstraintName("FK_ProjectUser_SystemUser");
            });

            modelBuilder.Entity<ProviderContact>(entity =>
            {
                entity.ToTable("ProviderContact", "SO-PM");

                entity.Property(e => e.ProviderContactId).HasColumnName("ProviderContact_ID");

                entity.Property(e => e.ProviderContactDetailId).HasColumnName("ProviderContactDetail_ID");

                entity.Property(e => e.ProviderId).HasColumnName("Provider_ID");

                entity.HasOne(d => d.ProviderContactDetail)
                    .WithMany(p => p.ProviderContact)
                    .HasForeignKey(d => d.ProviderContactDetailId)
                    .HasConstraintName("FK_ProviderContact_ProviderContactDetail");

                entity.HasOne(d => d.Provider)
                    .WithMany(p => p.ProviderContact)
                    .HasForeignKey(d => d.ProviderId)
                    .HasConstraintName("FK_ProviderContact_Providers");
            });

            modelBuilder.Entity<ProviderContactDetail>(entity =>
            {
                entity.ToTable("ProviderContactDetail", "SO-PM");

                entity.Property(e => e.ProviderContactDetailId).HasColumnName("ProviderContactDetail_ID");

                entity.Property(e => e.Email1).HasMaxLength(320);

                entity.Property(e => e.Email2).HasMaxLength(320);

                entity.Property(e => e.Name).HasMaxLength(10);

                entity.Property(e => e.PhoneNumber1).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber2).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Surname).HasMaxLength(50);
            });

            modelBuilder.Entity<Providers>(entity =>
            {
                entity.HasKey(e => e.ProviderId);

                entity.ToTable("Providers", "SO-PM");

                entity.Property(e => e.ProviderId).HasColumnName("Provider_ID");

                entity.Property(e => e.CityId).HasColumnName("City_ID");

                entity.Property(e => e.CountryId).HasColumnName("Country_ID");

                entity.Property(e => e.CreatedBy).HasColumnName("Created_By");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("Date_Created")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("Date_Updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.ProviderName).HasMaxLength(250);
                entity.Property(e => e.BillingFullName).HasMaxLength(250);
                entity.Property(e => e.IdCard).HasMaxLength(50);
                entity.Property(e => e.FullAddress).HasMaxLength(2000);
                entity.Property(e => e.PostalCode).HasMaxLength(20);

                entity.Property(e => e.UpdatedBy).HasColumnName("Updated_By");
            });

            modelBuilder.Entity<SystemUser>(entity =>
            {
                entity.HasKey(e => e.SytemUserId);

                entity.ToTable("SystemUser", "SO-PM");

                entity.Property(e => e.SytemUserId).HasColumnName("SytemUser_ID");

                entity.Property(e => e.Email).HasMaxLength(320);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.SystemUser)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_SystemUser_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("UserRole", "SO-PM");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.RoleName)
                    .HasColumnName("Role_Name")
                    .HasMaxLength(50);
            });
        }
    }
}
