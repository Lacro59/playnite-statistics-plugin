using Playnite.SDK;
using Playnite.SDK.Models;
using Statistics.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Statistics.Database
{
    class StatisticsDatabase
    {
        // Variable Playnite
        private static ILogger logger = LogManager.GetLogger();


        public StatisticsClass Statistics { get; set; }
        public ConcurrentDictionary<Guid, StatisticsSource> StatisticsSourceDatabase { get; set; }



        public void Initialize(IGameDatabaseAPI PlayniteApiDatabase)
        {
            Statistics = new StatisticsClass
            {
                GameGenres = new List<Counter>(),
                GameSource = new List<Counter>(),
                GameFavorite = new List<Counter>(),
                GameIsInstalled = new List<Counter>(),
                GameIsNotLaunching = new List<Counter>(),
                GameHidden = new List<Counter>(),
                GameCompletionStatus = new List<Counter>(),

                Playtime = 0,
                Total = 0
            };
            StatisticsSourceDatabase = new ConcurrentDictionary<Guid, StatisticsSource>();

            foreach (var Game in PlayniteApiDatabase.Games)
            {
                Add(Game);
                Add(Game, Game.SourceId);
            }

        }


        private void Add(Game Game, Guid? SourceId = null)
        {
            List<Counter> GameGenres = new List<Counter>();
            List<Counter> GameSource = new List<Counter>();
            List<Counter> GameFavorite = new List<Counter>();
            List<Counter> GameIsInstalled = new List<Counter>();
            List<Counter> GameIsNotLaunching = new List<Counter>();
            List<Counter> GameHidden = new List<Counter>();
            List<Counter> GameCompletionStatus = new List<Counter>();

            long Playtime = 0;

            bool IsFind = false;

            if (SourceId == null)
            {
                GameGenres = Statistics.GameGenres;
                GameSource = Statistics.GameSource;
                GameFavorite = Statistics.GameFavorite;
                GameIsInstalled = Statistics.GameIsInstalled;
                GameIsNotLaunching = Statistics.GameIsNotLaunching;
                GameHidden = Statistics.GameHidden;
                GameCompletionStatus = Statistics.GameCompletionStatus;

                Playtime = Statistics.Playtime; ;
            }
            else
            {
                if (StatisticsSourceDatabase.TryGetValue((Guid)SourceId, out var item))
                {
                    GameFavorite = item.GameFavorite;
                    GameIsInstalled = item.GameIsInstalled;
                    GameIsNotLaunching = item.GameIsNotLaunching;
                    GameHidden = item.GameHidden;
                    GameCompletionStatus = item.GameCompletionStatus;

                    Playtime = item.Playtime;
                }
                else
                {
                    StatisticsSource StatisticsSource = new StatisticsSource
                    {
                        GameFavorite = new List<Counter>(),
                        GameIsInstalled = new List<Counter>(),
                        GameIsNotLaunching = new List<Counter>(),
                        GameHidden = new List<Counter>(),
                        GameCompletionStatus = new List<Counter>(),

                        Playtime = 0,
                        Total = 0
                    };
                    StatisticsSourceDatabase.TryAdd((Guid)SourceId, StatisticsSource);

                    GameFavorite = new List<Counter>();
                    GameIsInstalled = new List<Counter>();
                    GameIsNotLaunching = new List<Counter>();
                    GameHidden = new List<Counter>();
                    GameCompletionStatus = new List<Counter>();

                    Playtime = 0;
                }
            }

            if (Game.IsInstalled)
                GameIsInstalled.Add(new Counter { Id = Game.Id, Name = Game.Name });

            if (Game.LastActivity == null)
                GameIsNotLaunching.Add(new Counter { Id = Game.Id, Name = Game.Name });

            if (SourceId == null)
            {
                if (Game.Genres != null)
                {
                    foreach (var item in Game.Genres)
                    {
                        IsFind = false;
                        for (int i = 0; i < GameGenres.Count; i++)
                        {
                            if (item.Name == GameGenres[i].Name)
                            {
                                GameGenres[i].Count += 1;
                                IsFind = true;
                            }
                        }
                        if (IsFind == false)
                            GameGenres.Add(new Counter { Id = item.Id, Name = item.Name, Count = 1 });
                    }
                }

                IsFind = false;
                string SourceName = "";
                if (Game.SourceId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    SourceName = "Playnite";
                else
                    SourceName = Game.Source.Name;

                for (int i = 0; i < GameSource.Count; i++)
                {
                    if (SourceName == GameSource[i].Name)
                    {
                        GameSource[i].Count += 1;
                        IsFind = true;
                    }
                }
                if (IsFind == false)
                    GameSource.Add(new Counter { Id = Game.SourceId, Name = SourceName, Count = 1 });
            }

            if (Game.Favorite)
                GameFavorite.Add(new Counter { Id = Game.Id, Name = Game.Name });

            if (Game.Hidden)
                GameHidden.Add(new Counter { Id = Game.Id, Name = Game.Name });

            IsFind = false;
            for (int i = 0; i < GameCompletionStatus.Count; i++)
            {
                if ("" + Game.CompletionStatus == GameCompletionStatus[i].Name)
                {
                    GameCompletionStatus[i].Count += 1;
                    IsFind = true;
                }
            }
            if (IsFind == false)
                GameCompletionStatus.Add(new Counter { Name = "" + Game.CompletionStatus, Count = 1 });

            Playtime += Game.Playtime;

            if (SourceId == null)
            {
                Statistics.GameGenres = GameGenres;
                Statistics.GameSource = GameSource;
                Statistics.GameFavorite = GameFavorite;
                Statistics.GameIsInstalled = GameIsInstalled;
                Statistics.GameIsNotLaunching = GameIsNotLaunching;
                Statistics.GameHidden = GameHidden;
                Statistics.GameCompletionStatus = GameCompletionStatus;
                Statistics.Playtime = Playtime;
                Statistics.Total += 1;
            }
            else
            {
                string SourceName = "";
                if (SourceId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    SourceName = "Playnite";
                else
                    SourceName = Game.Source.Name;

                StatisticsSourceDatabase[(Guid)SourceId].Name = SourceName;
                StatisticsSourceDatabase[(Guid)SourceId].GameFavorite = GameFavorite;
                StatisticsSourceDatabase[(Guid)SourceId].GameIsInstalled = GameIsInstalled;
                StatisticsSourceDatabase[(Guid)SourceId].GameIsNotLaunching = GameIsNotLaunching;
                StatisticsSourceDatabase[(Guid)SourceId].GameHidden = GameHidden;
                StatisticsSourceDatabase[(Guid)SourceId].GameCompletionStatus = GameCompletionStatus;
                StatisticsSourceDatabase[(Guid)SourceId].Playtime = Playtime;
                StatisticsSourceDatabase[(Guid)SourceId].Total += 1;
            }
        }
    }
}
