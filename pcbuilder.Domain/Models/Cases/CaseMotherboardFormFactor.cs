using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Models.Cases;

public class CaseMotherboardFormFactor
{
    public int CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public int MotherboardFormFactorId { get; set; }
    public MotherboardFormFactor MotherboardFormFactor { get; set; } = null!;
}