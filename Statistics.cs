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

        private StatisticsSettings settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("6828cad4-fc82-4828-b02d-4085b1e20327");

        public Statistics(IPlayniteAPI api) : base(api)
        {
            settings = new StatisticsSettings(this);

            // Get plugin's location 
            string pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Add plugin localization in application ressource.
            PluginCommon.Localization.SetPluginLanguage(pluginFolder, api.Paths.ConfigurationPath);
            // Add common in application ressource.
            PluginCommon.Common.Load(pluginFolder);
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

            if (settings.EnableIntegrationButton)
            {
                foreach (Button bt in Tools.FindVisualChildren<Button>(Application.Current.MainWindow))
                {
                    if (bt.Name == "PART_ButtonSteamFriends")
                    {
                        logger.Debug("Statistics - PART_ButtonSteamFriends find");

                        Button sBt = new StatisticsButton(TransformIcon.Get("statistics"));
                        sBt.Click += sBt_ClickEvent;
                        sBt.Width = bt.ActualWidth;
                        DockPanel.SetDock(sBt, Dock.Right);

                        DockPanel ControlParent = ((DockPanel)bt.Parent);
                        for (int i = 0; i < ControlParent.Children.Count; i++)
                        {
                            if (((FrameworkElement)ControlParent.Children[i]).Name == "PART_ButtonSteamFriends")
                            {
                                logger.Debug("Statistics - sBt add");
                                ControlParent.Children.Insert((i - 1), sBt);
                                i = 200;
                            }
                        }

                        break;
                    }
                    else
                    {
                        logger.Debug("Statistics - PART_ButtonSteamFriends not find");
                    }
                }
            }
        }

        private void sBt_ClickEvent(object sender, RoutedEventArgs e)
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