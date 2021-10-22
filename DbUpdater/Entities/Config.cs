namespace DbUpdater.Entities
{
    public class Config
    {
        /// <summary>
        /// Connection string for the DB
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Runs the script for the initial creation of the DB
        /// </summary>
        public bool InitialCreation { get; set; } = false;
    }
}