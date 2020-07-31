using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using PluginCommon;
using Statistics.Views;
using Statistics.Views.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;


namespace Statistics
{
    public class Statistics : Plugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private static IResourceProvider resources = new ResourceProvider();
        private StatisticsSettings settings { get; set; }
        public override Guid Id { get; } = Guid.Parse("6828cad4-fc82-4828-b02d-4085b1e20327");

        private readonly IntegrationUI ui = new IntegrationUI();


        public Statistics(IPlayniteAPI api) : base(api)
        {
            settings = new StatisticsSettings(this);

            // Get plugin's location 
            string pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Add plugin localization in application ressource.
            PluginCommon.Localization.SetPluginLanguage(pluginFolder, api.Paths.ConfigurationPath);
            // Add common in application ressource.
            PluginCommon.Common.Load(pluginFolder);

            // Check version
            if (settings.EnableCheckVersion)
            {
                CheckVersion cv = new CheckVersion();

                if (cv.Check("Statistics", pluginFolder))
                {
                    cv.ShowNotification(api, "Statistics - " + resources.GetString("LOCUpdaterWindowTitle"));
                }
            }
        }

        public override IEnumerable<ExtensionFunction> GetFunctions()
        {
            return new List<ExtensionFunction>
            {
                new ExtensionFunction(
                    resources.GetString("LOCStatistics"),
                    () =>
                    {
                        // Add code to be execute when user invokes this menu entry.

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

            if (settings.EnableIntegrationButtonHeader)
            {
                Button btHeader = new StatisticsButtonHeader(TransformIcon.Get("Statistics"));
                btHeader.Click += OnBtHeaderClick;
                ui.AddButtonInWindowsHeader(btHeader);
            }
        }

        /// <summary>
        /// Event for the header button for show plugin view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBtHeaderClick(object sender, RoutedEventArgs e)
        {
            new StatisticsView(settings, PlayniteApi.Database, this.GetPluginUserDataPath()).ShowDialog();
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

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new StatisticsSettingsView();
        }
    }
}
