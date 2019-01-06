namespace Tel.Egram.Services.Persistance
{
    public interface IDatabaseContextFactory
    {
        DatabaseContext CreateDbContext();
    }
}