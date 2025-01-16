using pcbuilder.Domain.Errors;
using pcbuilder.Domain.Models.Cpus;
using pcbuilder.Domain.Models.Motherboards;

namespace pcbuilder.Domain.Services;

public class CompatibilityChecker
{
    private readonly List<CompatibilityError> _errors = [];

    public List<CompatibilityError> GetErrors() => _errors;
    
    public bool CheckProcessorAndMotherboardCompatibility(Cpu cpu, Motherboard motherboard)
    {
        if (cpu.SocketId != motherboard.SocketId)
        {
            _errors.Add(CompatibilityErrors.SocketMismatch(cpu, motherboard));
            return false;
        }
        
        return true;
    }
}