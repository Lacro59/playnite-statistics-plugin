using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using PluginCommon;
using Statistics.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Statistics
{
    public class Statistics : Plugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private StatisticsSettings settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("6828cad4-fc82-4828-b02d-4085b1e20327");

        public Statistics(IPlayniteAPI api) : base(api)
        {
            settings = new StatisticsSettings(this);

            // Get plugin's location 
            string pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Add plugin localization in application ressource.
            Localization.SetPluginLanguage(pluginFolder, api.Paths.ConfigurationPath);
        }

        public override IEnumerable<ExtensionFunction> GetFunctions()
        {
            return new List<ExtensionFunction>
            {
                new ExtensionFunction(
                    "Statistics",
                    () =>
                    {
                        // Add code to be execute when user invokes this menu entry.
                        //PlayniteApi.Dialogs.ShowMessage("Code executed from a plugin!");

                        // Show SuccessView
                        new StatisticsView(settings, PlayniteApi.Database, this.GetPluginUserDataPath()).ShowDialog();
                    })
            };
        }

        public override void OnGameInstalled(Game game)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(Game game)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(Game game)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(Game game, long elapsedSeconds)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(Game game)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted()
        {
            // Add code to be executed when Playnite is initialized.
        }

        public override void OnApplicationStopped()
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated()
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        //public override UserControl GetSettingsView(bool firstRunSettings)
        //{
        //    return new StatisticsSettingsView();
        //}
    }
}