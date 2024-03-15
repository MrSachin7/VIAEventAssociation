namespace VIAEventAssociation.Core.Domain.temp;

public interface IUniqueEmailChecker {
    
    Task<bool> IsUnique(string email);
}