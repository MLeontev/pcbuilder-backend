using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pcbuilder.Domain.Models.Cases;
using pcbuilder.Domain.Models.Common;
using pcbuilder.Domain.Models.Coolers;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Gpus;
using pcbuilder.Domain.Models.Motherboards;
using pcbuilder.Domain.Models.PowerSupplies;
using pcbuilder.Domain.Models.Ram;
using pcbuilder.Domain.Models.Storage;
using pcbuilder.Infrastructure.Persistence.Configurations;

namespace pcbuilder.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Сборки пользователей
    public DbSet<Brand> Brands { get; set; }
    public DbSet<PcComponent> PcComponents { get; set; }
    public DbSet<Build> Builds { get; set; }
    public DbSet<BuildComponent> BuildComponents { get; set; }

    // Процессоры
    public DbSet<Cpu> Cpus { get; set; }
    public DbSet<CpuSeries> CpuSeries { get; set; }
    public DbSet<Socket> Sockets { get; set; }
    public DbSet<CpuMemory> CpuMemories { get; set; }

    // Память
    public DbSet<MemoryType> MemoryTypes { get; set; }
    public DbSet<Ram> Rams { get; set; }

    // Охлаждение
    public DbSet<Cooler> Coolers { get; set; }
    public DbSet<CoolerSocket> CoolerSockets { get; set; }
    public DbSet<WaterCoolingSize> WaterCoolingSizes { get; set; }

    // Видеокарты
    public DbSet<GpuChipset> GpuChipsets { get; set; }
    public DbSet<Gpu> Gpus { get; set; }
    public DbSet<GpuPowerConnector> GpuPowerConnectors { get; set; }

    // Материнские платы
    public DbSet<MotherboardFormFactor> MotherboardFormFactors { get; set; }
    public DbSet<Motherboard> Motherboards { get; set; }
    public DbSet<MotherboardStorage> MotherboardStorages { get; set; }
    public DbSet<MotherboardPowerConnector> MotherboardPowerConnectors { get; set; }
    public DbSet<MotherboardChipset> MotherboardChipsets { get; set; }
    public DbSet<MotherboardStorageInterface> MotherboardStorageInterfaces { get; set; }
    public DbSet<MotherboardStorageFormFactor> MotherboardStorageFormFactors { get; set; }

    // Блоки питания
    public DbSet<PowerConnector> PowerConnectors { get; set; }
    public DbSet<PowerSupply> PowerSupplies { get; set; }
    public DbSet<PsuPowerConnector> PsuPowerConnectors { get; set; }
    public DbSet<PsuEfficiency> PsuEfficiencies { get; set; }
    public DbSet<PowerConnectorCompatibility> PowerConnectorCompatibilities { get; set; }

    // Накопители
    public DbSet<Storage> Storage { get; set; }
    public DbSet<StorageType> StorageTypes { get; set; }
    public DbSet<StorageInterface> StorageInterfaces { get; set; }
    public DbSet<StorageFormFactor> StorageFormFactors { get; set; }

    // Корпуса
    public DbSet<Case> Cases { get; set; }
    public DbSet<CaseWaterCoolingSize> CaseWaterCoolingSizes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new RoleConfiguration());

        #region Настройка составных ключей

        builder.Entity<BuildComponent>()
            .HasKey(bc => new { bc.BuildId, bc.PcComponentId });

        builder.Entity<CpuMemory>()
            .HasKey(cm => new { cm.CpuId, cm.MemoryTypeId });

        builder.Entity<CoolerSocket>()
            .HasKey(cs => new { cs.CoolerId, cs.SocketId });

        builder.Entity<GpuPowerConnector>()
            .HasKey(gpc => new { gpc.PowerConnectorId, gpc.GpuId });

        builder.Entity<CaseWaterCoolingSize>()
            .HasKey(cw => new { cw.CaseId, cw.WaterCoolingSizeId });

        builder.Entity<PsuPowerConnector>()
            .HasKey(ppc => new { ppc.PowerSupplyId, ppc.PowerConnectorId });
        
        builder.Entity<MotherboardStorageInterface>()
            .HasKey(msi => new { msi.MotherboardStorageId, msi.StorageInterfaceId });
        
        builder.Entity<MotherboardStorageFormFactor>()
            .HasKey(msf => new { msf.MotherboardStorageId, msf.StorageFormFactorId });

        builder.Entity<MotherboardPowerConnector>()
            .HasKey(mpc => new { mpc.MotherboardId, mpc.PowerConnectorId });
        
        builder.Entity<PowerConnectorCompatibility>()
            .HasKey(pcc => new { pcc.SourceConnectorId, pcc.CompatibleConnectorId });

        #endregion

        builder.Entity<PowerConnectorCompatibility>()
            .HasOne(pc => pc.SourceConnector)
            .WithMany(c => c.CompatibleConnectors)
            .HasForeignKey(pc => pc.SourceConnectorId);
        
        base.OnModelCreating(builder);
    }
}