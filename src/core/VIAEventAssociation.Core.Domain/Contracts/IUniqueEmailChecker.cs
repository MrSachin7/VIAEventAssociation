namespace VIAEventAssociation.Core.Domain.Contracts;

public interface IUniqueEmailChecker {
    
    Task<bool> IsUnique(string email);
}