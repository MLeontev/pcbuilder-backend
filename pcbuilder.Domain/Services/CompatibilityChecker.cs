using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Services;

public class CompatibilityChecker
{
    private readonly List<CompatibilityError> _errors = [];

    public List<CompatibilityError> GetErrors() => _errors;
    
    public bool CheckProcessorAndMotherboardCompatibility(Cpu? cpu, Motherboard? motherboard)
    {
        if (cpu == null || motherboard == null) return true;
        
        if (cpu.SocketId == motherboard.SocketId) return true;
        
        _errors.Add(CompatibilityErrors.SocketMismatch(cpu, motherboard));
        return false;
    }
}