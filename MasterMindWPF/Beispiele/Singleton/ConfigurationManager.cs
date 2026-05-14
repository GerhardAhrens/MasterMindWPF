namespace MasterMindWPF.Beispiele
{
    using System;
    using System.Diagnostics;
    using System.Windows;

    public class ConfigurationManager : SingletonBase<ConfigurationManager>,
                                        ISingletonInitializable,
                                        ISingletonReloadable
    {
        protected ConfigurationManager()
        {
        }

        public string ApplicationName { get; private set; } = string.Empty;
        public DateTime LastReload { get; private set; }

        public void Initialize()
        {
            Debug.WriteLine("Initialisierung läuft...");

            // Beispielwerte erstellen
            this.LoadConfiguration();

            Debug.WriteLine("Initialisierung abgeschlossen");
        }

        public void ReloadContent()
        {
            Debug.WriteLine("Reload gestartet...");

            this.LoadConfiguration();

            Debug.WriteLine("Reload abgeschlossen");
        }

        public void Print()
        {
            Debug.WriteLine($"App: {this.ApplicationName}");
            Debug.WriteLine($"Startup: {this.LastReload}");
        }

        private void LoadConfiguration()
        {
            // Simuliert Laden aus Datei/DB/API
            ApplicationName = $"App geladen: {DateTime.Now:T}";
            LastReload = DateTime.Now;
        }
    }
}
