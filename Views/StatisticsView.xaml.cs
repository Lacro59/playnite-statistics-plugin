using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Models;
using Statistics.Database;
using Statistics.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Statistics.Views
{
    /// <summary>
    /// Logique d'interaction pour StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : Window
    {
        private static readonly ILogger logger = LogManager.GetLogger();


        public StatisticsView(StatisticsSettings settings, IGameDatabaseAPI PlayniteApiDatabase, string PluginUserDataPath)
        {
            StatisticsDatabase StatisticsDatabase = new StatisticsDatabase();
            StatisticsDatabase.Initialize(PlayniteApiDatabase);


            InitializeComponent();


            labelInformations.Content = "Global informations";
            labelTotalGames.Content = "Total games";
            countTotalGames.Content = StatisticsDatabase.Statistics.Total;
            labelTotalInstalled.Content = "Is installed";
            countTotalInstalled.Content = StatisticsDatabase.Statistics.GameIsInstalled.Count;
            labelTotalNotLaunching.Content = "Not Launching";
            countTotalNotLaunching.Content = StatisticsDatabase.Statistics.GameIsNotLaunching.Count;
            labelTotalFavorite.Content = "Favorite";
            countTotalFavorite.Content = StatisticsDatabase.Statistics.GameFavorite.Count;
            labelTotalPlaytime.Content = "Playtime";
            countTotalPlaytime.Content = (int)TimeSpan.FromSeconds(StatisticsDatabase.Statistics.Playtime).TotalHours + "h " 
                + TimeSpan.FromSeconds(StatisticsDatabase.Statistics.Playtime).ToString(@"mm") + "min";

            /*
            NotPlayed = 0,
            Played = 1,
            Beaten = 2,
            Completed = 3,
            Playing = 4,
            Abandoned = 5,
            OnHold = 6,
            PlanToPlay = 7
            */

            List<Counter> GameCompletionStatus = StatisticsDatabase.Statistics.GameCompletionStatus;
            int NotPlayed = 0;
            int Played = 0;
            int Beaten = 0;
            int Completed = 0;
            int Playing = 0;
            int Abandoned = 0;
            int OnHold = 0;
            int PlanToPlay = 0;

            for (int i = 0; i < GameCompletionStatus.Count; i++)
            {
                if (GameCompletionStatus[i].Name == "" + CompletionStatus.NotPlayed)
                    NotPlayed = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.Played)
                    Played = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.Beaten)
                    Beaten = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.Completed)
                    Completed = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.Playing)
                    Playing = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.Abandoned)
                    Abandoned = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.OnHold)
                    OnHold = GameCompletionStatus[i].Count;

                if (GameCompletionStatus[i].Name == "" + CompletionStatus.PlanToPlay)
                    PlanToPlay = GameCompletionStatus[i].Count;
            }

            labelGameCompletation.Content = "Game completation";
            labelNotPlayed.Content = "Not played";
            countNotPlayed.Value = NotPlayed;
            countNotPlayed.Maximum = StatisticsDatabase.Statistics.Total;
            labelPlayed.Content = "Played";
            countPlayed.Value = Played;
            countPlayed.Maximum = StatisticsDatabase.Statistics.Total;
            labelBeaten.Content = "Beaten";
            countBeaten.Value = Beaten;
            countBeaten.Maximum = StatisticsDatabase.Statistics.Total;
            labelCompleted.Content = "Completed";
            countCompleted.Value = Completed;
            countCompleted.Maximum = StatisticsDatabase.Statistics.Total;
            labelPlaying.Content = "Playing";
            countPlaying.Value = Playing;
            countPlaying.Maximum = StatisticsDatabase.Statistics.Total;
            labelAbandoned.Content = "Abandoned";
            countAbandoned.Value = Abandoned;
            countAbandoned.Maximum = StatisticsDatabase.Statistics.Total;
            labelOnHold.Content = "On hold";
            countOnHold.Value = OnHold;
            countOnHold.Maximum = StatisticsDatabase.Statistics.Total;
            labelPlanToPlay.Content = "Plan to play";
            countPlanToPlay.Value = PlanToPlay;
            countPlanToPlay.Maximum = StatisticsDatabase.Statistics.Total;


            
            ConcurrentDictionary<Guid, StatisticsSource> StatisticsSourceDatabase = StatisticsDatabase.StatisticsSourceDatabase;

            SeriesCollection SourceTotal = new SeriesCollection();

            SeriesCollection SourcePlaytime = new SeriesCollection();
            ChartValues<double> SourcePlaytimeSeries = new ChartValues<double>();
            string[] SourcePlaytimeLabels = new string[StatisticsSourceDatabase.Count];
            int counter = 0;
            foreach (var item in StatisticsSourceDatabase)
            {
                SourceTotal.Add(new PieSeries
                {
                    Title = item.Value.Name,
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.Value.Total) },
                    DataLabels = true
                });


                SourcePlaytimeSeries.Add(item.Value.Playtime);
                SourcePlaytimeLabels[counter] = item.Value.Name;
                counter += 1;
            }

            SourcePlaytime.Add(new RowSeries
            {
                Title = "Playtime",
                Values = SourcePlaytimeSeries
            });


            labelStatsSourceTotal.Content = "Total Games by source";
            StatsSourceTotal.Series = SourceTotal;

            labelStatsSourcePlaytime.Content = "Total playtime by source";
            StatsSourcePlaytime.Series = SourcePlaytime;
            StatsSourcePlaytimeX.LabelFormatter = value => (int)TimeSpan.FromSeconds(value).TotalHours + "h " + TimeSpan.FromSeconds(value).ToString(@"mm") + "min";
            StatsSourcePlaytimeY.Labels = SourcePlaytimeLabels;



        }
    }
}
