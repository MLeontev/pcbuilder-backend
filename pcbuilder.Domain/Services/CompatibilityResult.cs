namespace pcbuilder.Domain.Services;

public class CompatibilityResult
{
    public CompatibilityStatus Status { get; private set; } = CompatibilityStatus.Compatible;
    
    public List<CompatibilityError> Errors { get; } = [];
    
    public void AddError(CompatibilityError error)
    {
        Errors.Add(error);

        Status = error.Status switch
        {
            CompatibilityErrorStatus.Problem => CompatibilityStatus.Incompatible,
            
            CompatibilityErrorStatus.Warning when Status == CompatibilityStatus.Compatible =>
                CompatibilityStatus.CompatibleWithLimitations,
            
            _ => Status
        };
    }
    
    public bool HasErrors() => Errors.Any(error => error.Status == CompatibilityErrorStatus.Problem);
}

public enum CompatibilityStatus
{
    Compatible = 0,
    CompatibleWithLimitations = 1,
    Incompatible = 2
}