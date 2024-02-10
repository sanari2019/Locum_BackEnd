namespace Locum_Backend
{
    public class Setting
    {
        public string? ConnectionStrings { get; set; }

        [Obsolete]
        public void InitializeRepoDb()
        {
            // Assuming RepoDb.SqlServerBootstrap.Initialize requires connection string as a parameter
            RepoDb.SqlServerBootstrap.Initialize();
        }
    }
}