namespace VerySimpleDashboard.Data
{
    public interface IEntity<TIdentity>
    {
        TIdentity Id { get; set; }
    }
}