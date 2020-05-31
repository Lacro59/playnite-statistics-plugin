using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Models;
using PluginCommon;
using Statistics.Database;
using Statistics.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Statistics.Views
{
    /// <summary>
    /// Logique d'interaction pour StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : Window
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private static StatisticsDatabase StatisticsDatabase = new StatisticsDatabase();

        private ToggleButton _lastToggleButton = null;
        private bool desactiveToogleCheck = false;

        public StatisticsView(StatisticsSettings settings, IGameDatabaseAPI PlayniteApiDatabase, string PluginUserDataPath)
        {
            StatisticsDatabase.Initialize(PlayniteApiDatabase);

            InitializeComponent();

            SetListSource(PlayniteApiDatabase);
            _lastToggleButton = s0;
            s0.IsChecked = true;
            SetData("null");
        }

        /// <summary>
        /// Set list sources available in database.
        /// </summary>
        /// <param name="PlayniteApiDatabase"></param>
        internal void SetListSource(IGameDatabaseAPI PlayniteApiDatabase)
        {
            s1.Visibility = Visibility.Hidden;
            s2.Visibility = Visibility.Hidden;
            s3.Visibility = Visibility.Hidden;
            s4.Visibility = Visibility.Hidden;
            s5.Visibility = Visibility.Hidden;
            s6.Visibility = Visibility.Hidden;
            s7.Visibility = Visibility.Hidden;
            s8.Visibility = Visibility.Hidden;
            s9.Visibility = Visibility.Hidden;
            s10.Visibility = Visibility.Hidden;
            s11.Visibility = Visibility.Hidden;
            s12.Visibility = Visibility.Hidden;
            s13.Visibility = Visibility.Hidden;

            int iCount = 1;
            foreach (var item in PlayniteApiDatabase.Sources)
            {
                string SourceName = TransformIcon.Get(item.Name);

                if (SourceName.Length != 1)
                {
                    SourceName = item.Name;
                }
                else
                {
                    SourceName = SourceName + " " + item.Name;
                }

                switch (iCount)
                {
                    case 1:
                        s1.Tag = item.Id.ToString();
                        s1.Content = item.Name;
                        s1.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        s2.Tag = item.Id.ToString();
                        s2.Content = item.Name;
                        s2.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        s3.Tag = item.Id.ToString();
                        s3.Content = item.Name;
                        s3.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        s4.Tag = item.Id.ToString();
                        s4.Content = item.Name;
                        s4.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        s5.Tag = item.Id.ToString();
                        s5.Content = item.Name;
                        s5.Visibility = Visibility.Visible;
                        break;
                    case 6:
                        s6.Tag = item.Id.ToString();
                        s6.Content = item.Name;
                        s6.Visibility = Visibility.Visible;
                        break;
                    case 7:
                        s7.Tag = item.Id.ToString();
                        s7.Content = item.Name;
                        s7.Visibility = Visibility.Visible;
                        break;
                    case 8:
                        s8.Tag = item.Id.ToString();
                        s8.Content = item.Name;
                        s8.Visibility = Visibility.Visible;
                        break;
                    case 9:
                        s9.Tag = item.Id.ToString();
                        s9.Content = item.Name;
                        s9.Visibility = Visibility.Visible;
                        break;
                    case 10:
                        s10.Tag = item.Id.ToString();
                        s10.Content = item.Name;
                        s10.Visibility = Visibility.Visible;
                        break;
                    case 11:
                        s11.Tag = item.Id.ToString();
                        s11.Content = item.Name;
                        s11.Visibility = Visibility.Visible;
                        break;
                    case 12:
                        s12.Tag = item.Id.ToString();
                        s12.Content = item.Name;
                        s12.Visibility = Visibility.Visible;
                        break;
                    case 13:
                        s13.Tag = item.Id.ToString();
                        s13.Content = item.Name;
                        s13.Visibility = Visibility.Visible;
                        break;
                }

                iCount += 1;
            }

            s0.IsChecked = true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;

            try
            {
                if (_lastToggleButton == toggleButton && toggleButton.IsChecked == false)
                {
                    toggleButton.IsChecked = true;
                }
                else
                {
                    if (!desactiveToogleCheck)
                    {
                        desactiveToogleCheck = true;

                        if (_lastToggleButton != null)
                        {
                            _lastToggleButton.IsChecked = false;
                        }

                        _lastToggleButton = toggleButton;

                        SetData((string)toggleButton.Tag);
                    }
                }
            }
            catch
            {
                //logger.Error(Ex, "ToggleButton_Checked");
                desactiveToogleCheck = false;
            }

            desactiveToogleCheck = false;
        }

        internal void SetData(string SourceID)
        {
            StatisticsClass stats;
            if (SourceID == "null")
            {
                stats = StatisticsDatabase.Statistics;
            }
            else
            {
                stats = StatisticsDatabase.Get(Guid.Parse(SourceID));
            }

            long Total = 0;
            long TotalInstalled = 0;
            long TotalNotLaunching = 0;
            long TotalFavorite = 0;
            string TotalPlaytime = "0h 00min";

            long NotPlayed = 0;
            long Played = 0;
            long Beaten = 0;
            long Completed = 0;
            long Playing = 0;
            long Abandoned = 0;
            long OnHold = 0;
            long PlanToPlay = 0;

            SeriesCollection StatsGraphicsPlaytimeSeries = new SeriesCollection();
            string[] StatsGraphicsPlaytimeLabels = new string[0];
            ChartValues<double> SourcePlaytimeSeries = new ChartValues<double>();

            SeriesCollection SourceGraphicsGenresSeries = new SeriesCollection();

            if (stats != null)
            {
                // Information
                Total = stats.Total;
                TotalInstalled = stats.GameIsInstalled.Count;
                TotalNotLaunching = stats.GameIsNotLaunching.Count;
                TotalFavorite = stats.GameFavorite.Count;
                TotalPlaytime = (int)TimeSpan.FromSeconds(stats.Playtime).TotalHours + "h "
                    + TimeSpan.FromSeconds(stats.Playtime).ToString(@"mm") + "min";

                // Game completation
                List<Counter> GameCompletionStatus = stats.GameCompletionStatus;

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

                // Graphics playtime
                int counter = 0;
                if (SourceID == "null")
                {
                    ConcurrentDictionary<Guid, StatisticsClass> StatisticsSourceDatabase = StatisticsDatabase.StatisticsSourceDatabase;
                    StatsGraphicsPlaytimeLabels = new string[StatisticsSourceDatabase.Count];

                    foreach (var item in StatisticsSourceDatabase)
                    {
                        SourcePlaytimeSeries.Add(item.Value.Playtime);
                        StatsGraphicsPlaytimeLabels[counter] = item.Value.Name;
                        counter += 1;
                    }
                }
                else
                {
                    List<Counter> StatisticsSourceDatabase = StatisticsDatabase.Get(Guid.Parse(SourceID)).GameSource;

                    List<dataTemp> temp = new List<dataTemp>();

                    foreach (var item in StatisticsSourceDatabase)
                    {
                        temp.Add(new dataTemp() { Name = item.Name, Count = item.Count });
                    }

                    temp.Sort((a, b) => b.Count.CompareTo(a.Count));
                    if (temp.Count > 10)
                    {
                        temp = temp.GetRange(0, 10);
                    }
                    temp.Reverse();

                    StatsGraphicsPlaytimeLabels = new string[10];

                    foreach (var item in temp)
                    {
                        if (counter < 10)
                        {
                            SourcePlaytimeSeries.Add(item.Count);
                            StatsGraphicsPlaytimeLabels[counter] = item.Name;
                            counter += 1;
                        }
                    }
                }

                StatsGraphicsPlaytimeSeries.Add(new RowSeries
                {
                    Title = "Playtime",
                    Values = SourcePlaytimeSeries
                });

                //Graphics genres
                foreach (var item in stats.GameGenres)
                {
                    SourceGraphicsGenresSeries.Add(new PieSeries
                    {
                        Title = item.Name,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(item.Count) },
                        DataLabels = true
                    });
                }
            }
            else
            {

            }


            countTotalGames.Content = Total;
            countTotalInstalled.Content = TotalInstalled;
            countTotalNotLaunching.Content = TotalNotLaunching;
            countTotalFavorite.Content = TotalFavorite;
            countTotalPlaytime.Content = TotalPlaytime;

            countNotPlayed.Value = NotPlayed;
            countNotPlayed.Maximum = stats.Total;
            labelCountNotPlayed.Content = (int)Math.Round((double)(100 * NotPlayed) / stats.Total) + "%";
            countPlayed.Value = Played;
            countPlayed.Maximum = stats.Total;
            labelCountPlayed.Content = (int)Math.Round((double)(100 * Played) / stats.Total) + "%";
            countBeaten.Value = Beaten;
            countBeaten.Maximum = stats.Total;
            labelCountBeaten.Content = (int)Math.Round((double)(100 * Beaten) / stats.Total) + "%";
            countCompleted.Value = Completed;
            countCompleted.Maximum = stats.Total;
            labelCountCompleted.Content = (int)Math.Round((double)(100 * Completed) / stats.Total) + "%";
            countPlaying.Value = Playing;
            countPlaying.Maximum = stats.Total;
            labelCountPlaying.Content = (int)Math.Round((double)(100 * Playing) / stats.Total) + "%";
            countAbandoned.Value = Abandoned;
            countAbandoned.Maximum = stats.Total;
            labelCountAbandoned.Content = (int)Math.Round((double)(100 * Abandoned) / stats.Total) + "%";
            countOnHold.Value = OnHold;
            countOnHold.Maximum = stats.Total;
            labelCountOnHold.Content = (int)Math.Round((double)(100 * OnHold) / stats.Total) + "%";
            countPlanToPlay.Value = PlanToPlay;
            countPlanToPlay.Maximum = stats.Total;
            labelCountPlanToPlay.Content = (int)Math.Round((double)(100 * PlanToPlay) / stats.Total) + "%";

            StatsGraphicPlaytime.Series = StatsGraphicsPlaytimeSeries;
            StatsGraphicPlaytimeX.LabelFormatter = value => (int)TimeSpan.FromSeconds(value).TotalHours + "h " + TimeSpan.FromSeconds(value).ToString(@"mm") + "min";
            StatsGraphicPlaytimeY.Labels = StatsGraphicsPlaytimeLabels;

            StatsGraphicGenres.Series = SourceGraphicsGenresSeries;
        }
    }
}

public class dataTemp
{
    public string Name { get; set; }
    public long Count { get; set; }
}

