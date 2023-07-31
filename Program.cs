namespace IntrumWebApi
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="args"><inheritdoc/></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="args"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}