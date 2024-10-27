namespace Local_Canteen_Optimizer.Ultis
{
    internal class AppSettings
    {
        private static readonly AppSettings _instance = new AppSettings();
        public static AppSettings Instance => _instance;

        public string BaseUrl { get; set; } = "https://damp-wand-7w45w44w9573rg5g-8080.app.github.dev/";

        private AppSettings() { } // Private constructor to prevent external instantiation
    }
}
