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
        public ConcurrentDictionary<Guid, StatisticsClass> StatisticsSourceDatabase { get; set; }

        public IGameDatabaseAPI PlayniteApiDatabase;

        public void Initialize(IGameDatabaseAPI PlayniteApiDatabase, StatisticsSettings settings)
        {
            this.PlayniteApiDatabase = PlayniteApiDatabase;

            Statistics = new StatisticsClass
            {
                Name = "All",
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
            StatisticsSourceDatabase = new ConcurrentDictionary<Guid, StatisticsClass>();

            foreach (var Game in PlayniteApiDatabase.Games)
            {
                if (!Game.Hidden)
                {
                    Add(Game);
                    Add(Game, Game.SourceId);
                }
                else
                {
                    if (settings.IncludeHiddenGames)
                    {
                        Add(Game);
                        Add(Game, Game.SourceId);
                    }
                }
            }
        }


        public StatisticsClass Get(Guid id)
        {
            if (StatisticsSourceDatabase.TryGetValue(id, out var item))
            {
                return item;
            }
            else
            {
                return null;
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

            // Initialization variables
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
                    GameGenres = item.GameGenres;
                    GameSource = item.GameSource;
                    GameFavorite = item.GameFavorite;
                    GameIsInstalled = item.GameIsInstalled;
                    GameIsNotLaunching = item.GameIsNotLaunching;
                    GameHidden = item.GameHidden;
                    GameCompletionStatus = item.GameCompletionStatus;

                    Playtime = item.Playtime;
                }
                else
                {
                    string SourceName = "";
                    if (SourceId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                    {
                        SourceName = "Playnite";
                    }
                    else
                    {
                        SourceName = Game.Source.Name;
                    }

                    StatisticsClass StatisticsSource = new StatisticsClass
                    {
                        Name = SourceName,
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
                    StatisticsSourceDatabase.TryAdd((Guid)SourceId, StatisticsSource);

                    GameGenres = new List<Counter>();
                    GameSource = new List<Counter>();
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

            if (SourceId == null)
            {
                IsFind = false;
                string SourceName = "";
                if (Game.SourceId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    SourceName = "Playnite";
                }
                else
                {
                    SourceName = Game.Source.Name;
                }

                for (int i = 0; i < GameSource.Count; i++)
                {
                    if (SourceName == GameSource[i].Name)
                    {
                        GameSource[i].Count += 1;
                        IsFind = true;
                    }
                }
                if (IsFind == false)
                {
                    GameSource.Add(new Counter { Id = Game.SourceId, Name = SourceName, Count = 1 });
                }
            }
            else
            {
                string GameName = Game.Name;
                GameSource.Add(new Counter { Id = Game.Id, Name = GameName, Count = Game.Playtime });
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
            {
                GameCompletionStatus.Add(new Counter { Name = "" + Game.CompletionStatus, Count = 1 });
            }

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
                StatisticsSourceDatabase[(Guid)SourceId].GameGenres = GameGenres;
                StatisticsSourceDatabase[(Guid)SourceId].GameSource = GameSource;
                StatisticsSourceDatabase[(Guid)SourceId].GameFavorite = GameFavorite;
                StatisticsSourceDatabase[(Guid)SourceId].GameIsInstalled = GameIsInstalled;
                StatisticsSourceDatabase[(Guid)SourceId].GameIsNotLaunching = GameIsNotLaunching;
                StatisticsSourceDatabase[(Guid)SourceId].GameHidden = GameHidden;
                StatisticsSourceDatabase[(Guid)SourceId].GameCompletionStatus = GameCompletionStatus;
                StatisticsSourceDatabase[(Guid)SourceId].Playtime = Playtime;
                StatisticsSourceDatabase[(Guid)SourceId].Total += 1;
            }
        }


        public bool HaveGame(Guid SourceId)
        {
            foreach(Game game in PlayniteApiDatabase.Games)
            {
                if (SourceId == game.SourceId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
