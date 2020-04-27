using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace pastomatai.Models
{
    public partial class pastomataiContext : DbContext
    {
        public pastomataiContext()
        {
        }

        public pastomataiContext(DbContextOptions<pastomataiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<EndUser> EndUser { get; set; }
        public virtual DbSet<LoggedInUser> LoggedInUser { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<PostMachine> PostMachine { get; set; }
        public virtual DbSet<PostMachineBox> PostMachineBox { get; set; }
        public virtual DbSet<Terminal> Terminal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=pastomatai;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IdAddress)
                    .HasName("PK__Address__36F5392F8684E269");

                entity.Property(e => e.IdAddress)
                    .HasColumnName("id_Address")
                    .ValueGeneratedNever();

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HouseNumber)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EndUser>(entity =>
            {
                entity.HasKey(e => e.IdEndUser)
                    .HasName("PK__EndUser__227BA2F8E3CCE4CE");

                entity.Property(e => e.IdEndUser)
                    .HasColumnName("id_EndUser")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkAddressidAddress).HasColumnName("fk_Addressid_Address");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkAddressidAddressNavigation)
                    .WithMany(p => p.EndUser)
                    .HasForeignKey(d => d.FkAddressidAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("determinesThePlace");
            });

            modelBuilder.Entity<LoggedInUser>(entity =>
            {
                entity.HasKey(e => e.IdEndUser)
                    .HasName("PK__LoggedIn__227BA2F855F11392");

                entity.Property(e => e.IdEndUser)
                    .HasColumnName("id_EndUser")
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdEndUserNavigation)
                    .WithOne(p => p.LoggedInUser)
                    .HasForeignKey<LoggedInUser>(d => d.IdEndUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LoggedInU__id_En__2D27B809");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.IdMessage)
                    .HasName("PK__Message__D7379C213DF74146");

                entity.Property(e => e.IdMessage)
                    .HasColumnName("id_Message")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkLoggedInUseridEndUser).HasColumnName("fk_LoggedInUserid_EndUser");

                entity.Property(e => e.FkPackageidPackage).HasColumnName("fk_Packageid_Package");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkLoggedInUseridEndUserNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.FkLoggedInUseridEndUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("gets");

                entity.HasOne(d => d.FkPackageidPackageNavigation)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.FkPackageidPackage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("notifies");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.IdPackage)
                    .HasName("PK__Package__F2E83A6565F86461");

                entity.Property(e => e.IdPackage)
                    .HasColumnName("id_Package")
                    .ValueGeneratedNever();

                entity.Property(e => e.CollectionTime).HasColumnType("date");

                entity.Property(e => e.FkEndUseridEndUser).HasColumnName("fk_EndUserid_EndUser");

                entity.Property(e => e.FkLoggedInUseridEndUser).HasColumnName("fk_LoggedInUserid_EndUser");

                entity.Property(e => e.FkTerminalidTerminal).HasColumnName("fk_Terminalid_Terminal");

                entity.Property(e => e.PackageState)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.PutInTime).HasColumnType("date");

                entity.Property(e => e.Size)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.FkEndUseridEndUserNavigation)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.FkEndUseridEndUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("receives");

                entity.HasOne(d => d.FkLoggedInUseridEndUserNavigation)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.FkLoggedInUseridEndUser)
                    .HasConstraintName("sends");

                entity.HasOne(d => d.FkTerminalidTerminalNavigation)
                    .WithMany(p => p.Package)
                    .HasForeignKey(d => d.FkTerminalidTerminal)
                    .HasConstraintName("has");
            });

            modelBuilder.Entity<PostMachine>(entity =>
            {
                entity.HasKey(e => e.IdPostMachine)
                    .HasName("PK__PostMach__FBBEC8B5ECE04C21");

                entity.Property(e => e.IdPostMachine)
                    .HasColumnName("id_PostMachine")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkAddressidAddress).HasColumnName("fk_Addressid_Address");

                entity.Property(e => e.FkLoggedInUseridEndUser).HasColumnName("fk_LoggedInUserid_EndUser");

                entity.Property(e => e.FkLoggedInUseridEndUser1).HasColumnName("fk_LoggedInUserid_EndUser1");

                entity.Property(e => e.PostMachineState)
                    .IsRequired()
                    .HasMaxLength(19)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.FkAddressidAddressNavigation)
                    .WithMany(p => p.PostMachine)
                    .HasForeignKey(d => d.FkAddressidAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("determinesPlace");

                entity.HasOne(d => d.FkLoggedInUseridEndUserNavigation)
                    .WithMany(p => p.PostMachineFkLoggedInUseridEndUserNavigation)
                    .HasForeignKey(d => d.FkLoggedInUseridEndUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("overseesPackages");

                entity.HasOne(d => d.FkLoggedInUseridEndUser1Navigation)
                    .WithMany(p => p.PostMachineFkLoggedInUseridEndUser1Navigation)
                    .HasForeignKey(d => d.FkLoggedInUseridEndUser1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("oversees");
            });

            modelBuilder.Entity<PostMachineBox>(entity =>
            {
                entity.HasKey(e => e.IdPostMachineBox)
                    .HasName("PK__PostMach__5303F898674EF06C");

                entity.HasIndex(e => e.FkPackageidPackage)
                    .HasName("UQ__PostMach__4641E4F2177DDB8D")
                    .IsUnique();

                entity.Property(e => e.IdPostMachineBox)
                    .HasColumnName("id_PostMachineBox")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkPackageidPackage).HasColumnName("fk_Packageid_Package");

                entity.Property(e => e.FkPostMachineidPostMachine).HasColumnName("fk_PostMachineid_PostMachine");

                entity.Property(e => e.Pin)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Size)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.FkPackageidPackageNavigation)
                    .WithOne(p => p.PostMachineBox)
                    .HasForeignKey<PostMachineBox>(d => d.FkPackageidPackage)
                    .HasConstraintName("holds");

                entity.HasOne(d => d.FkPostMachineidPostMachineNavigation)
                    .WithMany(p => p.PostMachineBox)
                    .HasForeignKey(d => d.FkPostMachineidPostMachine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("belongs");
            });

            modelBuilder.Entity<Terminal>(entity =>
            {
                entity.HasKey(e => e.IdTerminal)
                    .HasName("PK__Terminal__66DF19975AD98934");

                entity.HasIndex(e => e.FkAddressidAddress)
                    .HasName("UQ__Terminal__BC966318A440C12D")
                    .IsUnique();

                entity.Property(e => e.IdTerminal)
                    .HasColumnName("id_Terminal")
                    .ValueGeneratedNever();

                entity.Property(e => e.FkAddressidAddress).HasColumnName("fk_Addressid_Address");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkAddressidAddressNavigation)
                    .WithOne(p => p.Terminal)
                    .HasForeignKey<Terminal>(d => d.FkAddressidAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("determinesLocation");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
