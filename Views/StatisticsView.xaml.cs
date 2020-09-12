﻿using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Playnite.Controls;
using Playnite.Converters;
using Playnite.SDK;
using Playnite.SDK.Models;
using PluginCommon;
using PluginCommon.LiveChartsCommon;
using Statistics.Database;
using Statistics.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Statistics.Views
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> TakeMax<T>(this IEnumerable<T> col, int max)
        {
            var count = 0;
            return col.TakeWhile(x => count++ < max);
        }
    }

    /// <summary>
    /// Logique d'interaction pour StatisticsView.xaml
    /// </summary>
    public partial class StatisticsView : WindowBase
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private static StatisticsDatabase StatisticsDatabase = new StatisticsDatabase();
        private readonly StatisticsSettings _settings;

        private ToggleButton _lastToggleButton = null;
        private bool desactiveToogleCheck = false;

        private string SelectedGenre = "";

        public StatisticsView(StatisticsSettings settings, IGameDatabaseAPI PlayniteApiDatabase, string PluginUserDataPath)
        {
            StatisticsDatabase.Initialize(PlayniteApiDatabase, settings);

            InitializeComponent();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            _settings = settings;

            if (!settings.PreferTopGames)
            {
                SwitchDataGames.IsChecked = false;
                SwitchDataSources.IsChecked = true;
            }

            if (settings.PreferGenresCount)
            {
                SwitchDataGenresTime.IsChecked = false;
                SwitchDataGenresCount.IsChecked = true;
            }

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

            // Playnite
            string SourceName = TransformIcon.Get("Playnite");

            if (SourceName.Length != 1)
            {
                SourceName = "Playnite";
            }
            else
            {
                SourceName = SourceName + " " + "Playnite";
            }

            if (StatisticsDatabase.HaveGame(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                s1.Tag = "00000000-0000-0000-0000-000000000000";
                s1.Content = SourceName;
                s1.Visibility = Visibility.Visible;
                iCount += 1;
            }


            #region Set list sources
            // Set emulators sources (cbEmulators)
            List<dataEmulators> ListEmulators = new List<dataEmulators>();
            foreach (var item in PlayniteApiDatabase.Emulators)
            {
                ListEmulators.Add(new dataEmulators { Id = item.Id, Name = item.Name });
            }
            cbEmulators.ItemsSource = ListEmulators;
            if (ListEmulators.Count == 0)
            {
                spEmulators.Visibility = Visibility.Hidden;
            }
            // Get link source for emulator
            else
            {
                foreach (var game in PlayniteApiDatabase.Games)
                {
                    if (game.PlayAction != null && game.PlayAction.EmulatorId != null)
                    {
                        for (int i = 0; i < ListEmulators.Count; i++)
                        {
                            if (ListEmulators[i].Id == game.PlayAction.EmulatorId)
                            {
                                ListEmulators[i].SourceId = game.SourceId;
                                if (game.SourceId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                                {
                                    ListEmulators[i].SourceName = "";
                                }
                                else
                                {
                                    ListEmulators[i].SourceName = game.Source.Name;
                                }
                                i = ListEmulators.Count;
                            }
                        }
                    }
                }
            }

            // Set pc sources
            foreach (var item in PlayniteApiDatabase.Sources)
            {
                bool IsEmulators = false;
                for (int i = 0; i < ListEmulators.Count; i++)
                {
                    if (ListEmulators[i].SourceId == item.Id)
                    {
                        IsEmulators = true;
                    }
                }

                if (!IsEmulators) 
                {
                    SourceName = TransformIcon.Get(item.Name);

                    if (SourceName.Length != 1)
                    {
                        SourceName = item.Name;
                    }
                    else
                    {
                        SourceName = SourceName + " " + item.Name;
                    }

                    if (StatisticsDatabase.HaveGame(item.Id))
                    {
                        switch (iCount)
                        {
                            case 1:
                                s1.Tag = item.Id.ToString();
                                s1.Content = SourceName;
                                s1.Visibility = Visibility.Visible;
                                break;
                            case 2:
                                s2.Tag = item.Id.ToString();
                                s2.Content = SourceName;
                                s2.Visibility = Visibility.Visible;
                                break;
                            case 3:
                                s3.Tag = item.Id.ToString();
                                s3.Content = SourceName;
                                s3.Visibility = Visibility.Visible;
                                break;
                            case 4:
                                s4.Tag = item.Id.ToString();
                                s4.Content = SourceName;
                                s4.Visibility = Visibility.Visible;
                                break;
                            case 5:
                                s5.Tag = item.Id.ToString();
                                s5.Content = SourceName;
                                s5.Visibility = Visibility.Visible;
                                break;
                            case 6:
                                s6.Tag = item.Id.ToString();
                                s6.Content = SourceName;
                                s6.Visibility = Visibility.Visible;
                                break;
                            case 7:
                                s7.Tag = item.Id.ToString();
                                s7.Content = SourceName;
                                s7.Visibility = Visibility.Visible;
                                break;
                            case 8:
                                s8.Tag = item.Id.ToString();
                                s8.Content = SourceName;
                                s8.Visibility = Visibility.Visible;
                                break;
                            case 9:
                                s9.Tag = item.Id.ToString();
                                s9.Content = SourceName;
                                s9.Visibility = Visibility.Visible;
                                break;
                            case 10:
                                s10.Tag = item.Id.ToString();
                                s10.Content = SourceName;
                                s10.Visibility = Visibility.Visible;
                                break;
                            case 11:
                                s11.Tag = item.Id.ToString();
                                s11.Content = SourceName;
                                s11.Visibility = Visibility.Visible;
                                break;
                            case 12:
                                s12.Tag = item.Id.ToString();
                                s12.Content = SourceName;
                                s12.Visibility = Visibility.Visible;
                                break;
                            case 13:
                                s13.Tag = item.Id.ToString();
                                s13.Content = SourceName;
                                s13.Visibility = Visibility.Visible;
                                break;
                        }

                        iCount += 1;
                    }
                }
            }

            s0.IsChecked = true;
            #endregion  
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var toggleButton = sender as ToggleButton;
            cbEmulators.Text = "";
            cbEmulators.SelectedIndex = -1;

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
                SwitchDataGames.Visibility = Visibility.Visible;
                SwitchDataSources.Visibility = Visibility.Visible;
            }
            else
            {
                stats = StatisticsDatabase.Get(Guid.Parse(SourceID));
                SwitchDataGames.Visibility = Visibility.Hidden;
                SwitchDataSources.Visibility = Visibility.Hidden;
            }

            // Reduce amount of genres
            stats.GameGenres = stats.GameGenres
                .Where(x => x.Count >= _settings.MinGenreCount)
                .OrderByDescending(x => x.Count)
                .TakeMax(_settings.MaxGenres).ToList();

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
            ChartValues<CustomerForTime> SourcePlaytimeSeries = new ChartValues<CustomerForTime>();

            SeriesCollection StatsGraphicsGenrePlaytimeSeries = new SeriesCollection();
            string[] StatsGraphicsGenrePlaytimeLabels = new string[0];
            ChartValues<CustomerForTime> SourceGenrePlaytimeSeries = new ChartValues<CustomerForTime>();

            SeriesCollection SourceGraphicsGenresSeries = new SeriesCollection();

            if (stats != null)
            {
                // Information
                Total = stats.Total;
                TotalInstalled = stats.GameIsInstalled.Count;
                TotalNotLaunching = stats.GameIsNotLaunching.Count;
                TotalFavorite = stats.GameFavorite.Count;

                LongToTimePlayedConverter converter = new LongToTimePlayedConverter();
                TotalPlaytime = (string)converter.Convert(stats.Playtime, null, null, CultureInfo.CurrentCulture);

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
                List<dataTemp> temp = new List<dataTemp>();
                if (SourceID == "null")
                {
                    ConcurrentDictionary<Guid, StatisticsClass> StatisticsSourceDatabase = StatisticsDatabase.StatisticsSourceDatabase;

                    // Playtime
                    StatsGraphicsPlaytimeLabels = new string[StatisticsSourceDatabase.Count];
                    temp = new List<dataTemp>();

                    if (!(bool)SwitchDataGames.IsChecked)
                    {
                        foreach (var item in StatisticsSourceDatabase)
                        {
                            temp.Add(new dataTemp { Name = item.Value.Name, Count = item.Value.Playtime });
                        }
                        temp.Sort((a, b) => a.Count.CompareTo(b.Count));

                        foreach (var item in temp)
                        {
                            SourcePlaytimeSeries.Add(new CustomerForTime
                            {
                                Name = item.Name,
                                Values = item.Count
                            });
                            StatsGraphicsPlaytimeLabels[counter] = item.Name;
                            counter += 1;
                        }
                    }
                    else
                    {
                        foreach (var itemAll in StatisticsSourceDatabase)
                        {
                            foreach (var item in itemAll.Value.GameSource)
                            {
                                temp.Add(new dataTemp { Name = item.Name, Count = item.Count });
                            }
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
                                SourcePlaytimeSeries.Add(new CustomerForTime
                                {
                                    Name = item.Name,
                                    Values = item.Count
                                });
                                StatsGraphicsPlaytimeLabels[counter] = item.Name;
                                counter += 1;
                            }
                        }
                    }

                    // Genre Playtime
                    
                }
                else
                {
                    List<Counter> StatisticsSourceDatabase = StatisticsDatabase.Get(Guid.Parse(SourceID)).GameSource;

                    temp = new List<dataTemp>();
                    foreach (var item in StatisticsSourceDatabase)
                    {
                        temp.Add(new dataTemp { Name = item.Name, Count = item.Count });
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
                            SourcePlaytimeSeries.Add(new CustomerForTime
                            {
                                Name = item.Name,
                                Values = item.Count
                            });
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

                // Genre Playtime
                temp = new List<dataTemp>();
                foreach (var item in stats.GameGenresTime)
                {
                    temp.Add(new dataTemp { Name = item.Name, Count = item.Playtime });
                }
                temp.Sort((a, b) => b.Count.CompareTo(a.Count));
                temp.Reverse();

                // Limit with playtime > 0
                temp = temp.FindAll(x => x.Count > 0);

                counter = 0;
                StatsGraphicsGenrePlaytimeLabels = new string[temp.Count];
                foreach (var item in temp)
                {
                    SourceGenrePlaytimeSeries.Add(new CustomerForTime
                    {
                        Name = item.Name,
                        Values = item.Count
                    });
                    StatsGraphicsGenrePlaytimeLabels[counter] = item.Name;
                    counter += 1;
                }

                StatsGraphicsGenrePlaytimeSeries.Add(new RowSeries
                {
                    Title = "Playtime",
                    Values = SourceGenrePlaytimeSeries
                });


                // Graphics genres
                temp = new List<dataTemp>();
                foreach (var item in stats.GameGenres)
                {
                    temp.Add(new dataTemp { Name = item.Name, Count = item.Count });
                }
                temp.Sort((a, b) => b.Count.CompareTo(a.Count));

                foreach (var item in temp)
                {
                    SourceGraphicsGenresSeries.Add(new PieSeries
                    {
                        Title = item.Name,
                        Values = new ChartValues<CustomerForSingle> { new CustomerForSingle { Name = item.Name, Values = item.Count } },
                        DataLabels = true
                    });
                }
            }


            countTotalGames.Content = Total;
            countTotalInstalled.Content = TotalInstalled;
            countTotalNotLaunching.Content = TotalNotLaunching;
            countTotalFavorite.Content = TotalFavorite;
            countTotalPlaytime.Content = TotalPlaytime;

            countNotPlayed.Value = NotPlayed;
            countNotPlayed.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountNotPlayed.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * NotPlayed) / stats.Total) + "%";
            countPlayed.Value = Played;
            countPlayed.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountPlayed.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * Played) / stats.Total) + "%";
            countBeaten.Value = Beaten;
            countBeaten.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountBeaten.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * Beaten) / stats.Total) + "%";
            countCompleted.Value = Completed;
            countCompleted.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountCompleted.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * Completed) / stats.Total) + "%";
            countPlaying.Value = Playing;
            countPlaying.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountPlaying.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * Playing) / stats.Total) + "%";
            countAbandoned.Value = Abandoned;
            countAbandoned.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountAbandoned.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * Abandoned) / stats.Total) + "%";
            countOnHold.Value = OnHold;
            countOnHold.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountOnHold.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * OnHold) / stats.Total) + "%";
            countPlanToPlay.Value = PlanToPlay;
            countPlanToPlay.Maximum = (stats == null) ? 1 : stats.Total;
            labelCountPlanToPlay.Content = (stats == null) ? "" : (int)Math.Round((double)(100 * PlanToPlay) / stats.Total) + "%";


            //let create a mapper so LiveCharts know how to plot our CustomerViewModel class
            var customerVmMapper = Mappers.Xy<CustomerForTime>()
                .Y((value, index) => index)
                .X(value => value.Values);

            //lets save the mapper globally
            Charting.For<CustomerForTime>(customerVmMapper);


            //let create a mapper so LiveCharts know how to plot our CustomerViewModel class
            var customerVmMapperPie = Mappers.Xy<CustomerForSingle>()
                .X((value, index) => index)
                .Y(value => value.Values);

            //lets save the mapper globally
            Charting.For<CustomerForSingle>(customerVmMapperPie);


            StatsGraphicPlaytime.Series = StatsGraphicsPlaytimeSeries;
            StatsGraphicPlaytimeY.Labels = StatsGraphicsPlaytimeLabels;

            StatsGraphicGenresPlaytime.Series = StatsGraphicsGenrePlaytimeSeries;
            StatsGraphicGenresPlaytimeY.Labels = StatsGraphicsGenrePlaytimeLabels;

            StatsGraphicGenres.Series = SourceGraphicsGenresSeries;
        }

        private void S0_Loaded(object sender, RoutedEventArgs e)
        {
            Tools.DesactivePlayniteWindowControl(this);
        }

        private void SwitchData_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)sender).Name == "SwitchDataGames")
            {
                SwitchDataSources.IsChecked = !((ToggleButton)sender).IsChecked;
            }
            else
            {
                SwitchDataGames.IsChecked = !((ToggleButton)sender).IsChecked;
            }


            SetData("null");
        }

        private void SwitchDataGenres_Click(object sender, RoutedEventArgs e)
        {
            if (((ToggleButton)sender).Name == "SwitchDataGenresTime")
            {
                SwitchDataGenresCount.IsChecked = !((ToggleButton)sender).IsChecked;
            }
            else
            {
                SwitchDataGenresTime.IsChecked = !((ToggleButton)sender).IsChecked;
            }
        }

        #region Tooltip selection
        private void StatsGraphicGenres_DataHover(object sender, ChartPoint chartPoint)
        {
            SelectedGenre = chartPoint.SeriesView.Title;
            SelectedToolTip();
        }

        private void CustomerToolTipForMultipleSingle_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedToolTip();
        }

        private void SelectedToolTip()
        {
            ScrollViewer scrollViewer = new ScrollViewer();
            foreach (ScrollViewer sp in Tools.FindVisualChildren<ScrollViewer>(StatsGraphicGenres.DataTooltip))
            {
                scrollViewer = sp;
            }

            int counter = 0;
            foreach (Grid sp in Tools.FindVisualChildren<Grid>(StatsGraphicGenres.DataTooltip))
            {
                if (SelectedGenre == (string)sp.Tag)
                {
                    sp.Background = Brushes.CadetBlue;
                    if (counter > 10)
                    {
                        scrollViewer.ScrollToVerticalOffset(counter * 15);
                    }
                    else
                    {
                        scrollViewer.ScrollToVerticalOffset(0);
                    }
                }
                else
                {
                    sp.Background = Brushes.Transparent;
                }

                counter += 1;
            }
        }
        #endregion

        private void CbEmulators_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            s0.IsChecked = false;
            s1.IsChecked = false;
            s2.IsChecked = false;
            s3.IsChecked = false;
            s4.IsChecked = false;
            s5.IsChecked = false;
            s6.IsChecked = false;
            s7.IsChecked = false;
            s8.IsChecked = false;
            s9.IsChecked = false;
            s10.IsChecked = false;
            s11.IsChecked = false;
            s12.IsChecked = false;
            s13.IsChecked = false;

            try
            {
                SetData(((dataEmulators)cbEmulators.SelectedItem).Id.ToString());
            }
            catch
            {

            }
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}

public class dataTemp
{
    public string Name { get; set; }
    public long Count { get; set; }
}

public class dataEmulators
{
    public Guid Id { get; set; }
    public Guid SourceId { get; set; }
    public string Name { get; set; }
    public string SourceName { get; set; }

    public override string ToString()
    {
        return Name;
    }
    public string cbName
    {
        get
        {
            if (SourceName.IsNullOrEmpty())
            {
                return Name;
            }
            return SourceName;
        }
    }
}
