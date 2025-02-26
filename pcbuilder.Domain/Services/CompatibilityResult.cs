namespace pcbuilder.Domain.Services;

public class CompatibilityResult
{
    public CompatibilityStatus Status { get; private set; } = CompatibilityStatus.Compatible;

    public List<CompatibilityError> Errors { get; private set; } = [];

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

    public void AddErrors(IEnumerable<CompatibilityError> errors)
    {
        foreach (var error in errors) AddError(error);
    }

    public bool HasErrors()
    {
        return Errors.Any(error => error.Status == CompatibilityErrorStatus.Problem);
    }
}

public enum CompatibilityStatus
{
    Compatible = 0,
    CompatibleWithLimitations = 1,
    Incompatible = 2
}