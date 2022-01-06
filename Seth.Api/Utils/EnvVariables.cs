namespace Seth.Api.Utils
{
    public static class EnvVariables
    {
        public static string SethConfigPathEnv = Environment.GetEnvironmentVariable("SETH_CONFIG_PATH") ?? "";

        public static string SethRootPathEnv = Environment.GetEnvironmentVariable("SETH_ROOT_PATH") ?? "";
    }
}