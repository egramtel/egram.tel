namespace Tel.Egram.Persistance
{
    public interface IDatabaseContextFactory
    {
        DatabaseContext CreateDbContext();
    }
}