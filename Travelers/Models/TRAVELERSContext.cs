using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Travelers.Models
{
    public partial class TRAVELERSContext : DbContext
    {
        public TRAVELERSContext()
        {
        }

        public TRAVELERSContext(DbContextOptions<TRAVELERSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Destino> Destinos { get; set; }
        public virtual DbSet<MedioPago> MedioPagos { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<Viaje> Viajes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("data source=ANTONIO; initial catalog=Travelers; Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.ToTable("Cliente");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("apellido")
                    .IsFixedLength(true);

                entity.Property(e => e.Dni).HasColumnName("dni");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("nombre")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Destino>(entity =>
            {
                entity.HasKey(e => e.IdDestino);

                entity.ToTable("Destino");

                entity.Property(e => e.IdDestino).HasColumnName("idDestino");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("descripcion")
                    .IsFixedLength(true);

                entity.Property(e => e.NombrePais)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("nombrePais")
                    .IsFixedLength(true);

                entity.Property(e => e.NombreProvincia)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("nombreProvincia")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<MedioPago>(entity =>
            {
                entity.HasKey(e => e.IdMedioPago);

                entity.ToTable("MedioPago");

                entity.Property(e => e.IdMedioPago).HasColumnName("idMedioPago");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("tipo")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.IdReserva);

                entity.ToTable("Reserva");

                entity.Property(e => e.IdReserva).HasColumnName("idReserva");

                entity.Property(e => e.CostoTotal)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("costoTotal");

                entity.Property(e => e.FechaReserva)
                    .HasColumnType("date")
                    .HasColumnName("fechaReserva");

                entity.Property(e => e.IdCliente).HasColumnName("idCliente");

                entity.Property(e => e.IdMedioPago).HasColumnName("idMedioPago");

                entity.Property(e => e.IdViaje).HasColumnName("idViaje");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Cliente");

                entity.HasOne(d => d.IdMedioPagoNavigation)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.IdMedioPago)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_MedioPago");

                entity.HasOne(d => d.IdViajeNavigation)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.IdViaje)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Viaje");
            });

            modelBuilder.Entity<Viaje>(entity =>
            {
                entity.HasKey(e => e.IdViaje);

                entity.ToTable("Viaje");

                entity.Property(e => e.IdViaje).HasColumnName("idViaje");

                entity.Property(e => e.Aerolinas)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("aerolinas")
                    .IsFixedLength(true);

                entity.Property(e => e.CapacidadMax).HasColumnName("capacidadMax");

                entity.Property(e => e.IdDestino).HasColumnName("idDestino");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("precio");

                entity.HasOne(d => d.IdDestinoNavigation)
                    .WithMany(p => p.Viajes)
                    .HasForeignKey(d => d.IdDestino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Viaje_Destino");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
