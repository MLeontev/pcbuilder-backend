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
    // Сборки пользователей
    public DbSet<PcComponent> PcComponents { get; set; }
    public DbSet<Build> Builds { get; set; }
    public DbSet<BuildComponent> BuildComponents { get; set; }
    public DbSet<Brand> Brands { get; set; }
    
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
    public DbSet<PciSlot> PciSlots { get; set; }
    public DbSet<MotherboardFormFactor> MotherboardFormFactors { get; set; }
    public DbSet<Motherboard> Motherboards { get; set; }
    public DbSet<MotherboardPciSlot> MotherboardPciSlots { get; set; }
    public DbSet<MotherboardStorage> MotherboardStorages { get; set; }
    public DbSet<MotherboardPowerConnector> MotherboardPowerConnectors { get; set; }
    public DbSet<MotherboardChipset> MotherboardChipsets { get; set; }
    
    // Блоки питания
    public DbSet<PowerConnector> PowerConnectors { get; set; }
    public DbSet<PowerSupply> PowerSupplies { get; set; }
    public DbSet<PsuPowerConnector> PsuPowerConnectors { get; set; }
    public DbSet<PsuEfficiency> PsuEfficiencies { get; set; }
    
    // Накопители
    public DbSet<Storage> Storage { get; set; }
    public DbSet<StorageType> StorageTypes { get; set; }
    public DbSet<StorageInterface> StorageInterfaces { get; set; }
    public DbSet<StorageFormFactor> StorageFormFactors { get; set; }
    
    // Корпуса
    public DbSet<Case> Cases { get; set; }
    public DbSet<CaseWaterCoolingSize> CaseWaterCoolingSizes { get; set; }
    public DbSet<CaseStorageFormFactor> CaseStorageFormFactors { get; set; }
    public DbSet<CaseMotherboardFormFactor> CaseMotherboardFormFactors { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
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
        
        builder.Entity<CaseStorageFormFactor>()
            .HasKey(cs => new { cs.CaseId, cs.StorageFormFactorId });
        
        builder.Entity<CaseMotherboardFormFactor>()
            .HasKey(cm => new { cm.CaseId, cm.MotherboardFormFactorId });
        
        builder.Entity<PsuPowerConnector>()
            .HasKey(ppc => new { ppc.PsuId, ppc.PowerConnectorId });
        
        builder.Entity<MotherboardPciSlot>()
            .HasKey(mps => new { mps.MotherboardId, mps.PciSlotId });

        builder.Entity<MotherboardStorage>()
            .HasKey(ms => new { ms.MotherboardId, ms.StorageInterfaceId });

        builder.Entity<MotherboardPowerConnector>()
            .HasKey(mpc => new { mpc.MotherboardId, mpc.PowerConnectorId });
        
        #endregion

        #region Заполнение справочных таблиц

        builder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Intel" },
            new Brand { Id = 2, Name = "AMD" },
            new Brand { Id = 3, Name = "MSI" }
        );
        
        builder.Entity<MemoryType>().HasData(
            new MemoryType { Id = 1, Name = "DDR3" },
            new MemoryType { Id = 2, Name = "DDR4" },
            new MemoryType { Id = 3, Name = "DDR5" }
        );
        
        builder.Entity<Socket>().HasData(
            new Socket { Id = 1, Name = "LGA1700" },
            new Socket { Id = 2, Name = "AM4" },
            new Socket { Id = 3, Name = "AM5" }
        );
        
        builder.Entity<CpuSeries>().HasData(
            new CpuSeries { Id = 1, BrandId = 1, Name = "Core i5" },
            new CpuSeries { Id = 2, BrandId = 2, Name = "Ryzen 5" }
        );
        
        builder.Entity<PciSlot>().HasData(
            new PciSlot { Id = 1, Version = "4.0", Bandwidth = 16 }
        );
        
        builder.Entity<MotherboardChipset>().HasData(
            new MotherboardChipset { Id = 1, BrandId = 1, Name = "H610" }
        );
        
        builder.Entity<MotherboardFormFactor>().HasData(
            new MotherboardFormFactor { Id = 1, Name = "Micro-ATX" },
            new MotherboardFormFactor { Id = 2, Name = "ATX" }
        );
        
        #endregion

        #region Заполнение процессоров
        
        builder.Entity<Cpu>().HasData(
            new Cpu
            {
                Id = 2,
                BrandId = 2,
                Name = "Ryzen 5 5600X 3.7 GHz 6-Core Processor",
                SeriesId = 2,
                SocketId = 2,
                Cores = 6,
                Threads = 12,
                BaseClock = 3.7M,
                BoostClock = 4.6M,
                Tdp = 65,
                IntegratedGpu = false,
                MaxMemoryCapacity = 128
            }
        );
        builder.Entity<CpuMemory>().HasData(
            new CpuMemory { CpuId = 2, MemoryTypeId = 2, MaxMemorySpeed = 3200}
        );
        
        builder.Entity<Cpu>().HasData(
            new Cpu
            {
                Id = 3,
                BrandId = 2,
                Name = "Ryzen 5 7500F 3.7 GHz 6-Core Processor",
                SeriesId = 2,
                SocketId = 3,
                Cores = 6,
                Threads = 12,
                BaseClock = 3.7M,
                BoostClock = 5M,
                Tdp = 65,
                IntegratedGpu = false,
                MaxMemoryCapacity = 128
            }
        );
        builder.Entity<CpuMemory>().HasData(
            new CpuMemory { CpuId = 3, MemoryTypeId = 3, MaxMemorySpeed = 5200}
        );
        
        builder.Entity<Cpu>().HasData(
            new Cpu
            {
                Id = 4,
                BrandId = 1,
                Name = "Core i5-12400F 2.5 GHz 6-Core Processor",
                SeriesId = 1,
                SocketId = 1,
                Cores = 6,
                Threads = 12,
                BaseClock = 2.5M,
                BoostClock = 4.4M,
                Tdp = 65,
                IntegratedGpu = false,
                MaxMemoryCapacity = 128
            }
        );
        builder.Entity<CpuMemory>().HasData(
            new CpuMemory { CpuId = 4, MemoryTypeId = 2, MaxMemorySpeed = 3200},
            new CpuMemory { CpuId = 4, MemoryTypeId = 3, MaxMemorySpeed = 4800}
        );

        #endregion

        #region Заполнение материнских плат

        builder.Entity<Motherboard>().HasData(
            new Motherboard
            {
                Id = 2,
                BrandId = 3,
                Name = "PRO H610M-E DDR4",
                MotherboardChipsetId = 1,
                SocketId = 1,
                FormFactorId = 1,
                MemoryTypeId = 2,
                MemorySlots = 2,
                MaxMemoryCapacity = 64,
                MaxMemorySpeed = 3200
            }
        );
        
        builder.Entity<Motherboard>().HasData(
            new Motherboard
            {
                Id = 2,
                BrandId = 3,
                Name = "PRO H610M-E DDR4",
                MotherboardChipsetId = 1,
                SocketId = 1,
                FormFactorId = 1,
                MemoryTypeId = 2,
                MemorySlots = 2,
                MaxMemoryCapacity = 64,
                MaxMemorySpeed = 3200
            }
        );

        #endregion
        
        base.OnModelCreating(builder);
    }
}