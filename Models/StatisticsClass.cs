using System;
using System.Collections.Generic;

namespace Statistics.Models
{
    //Game.Added
    //Game.PlayCount;
    class StatisticsClass
    {
        public List<Counter> GameGenres { get; set;  }
        public List<Counter> GameSource { get; set;  }
        public List<Counter> GameFavorite { get; set;  }
        public List<Counter> GameIsInstalled { get; set;  }
        public List<Counter> GameIsNotLaunching { get; set;  }
        public List<Counter> GameHidden { get; set;  }
        public List<Counter> GameCompletionStatus { get; set;  }

        public long Playtime { get; set; }
        public int Total { get; set; }
    }

    class Counter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    class StatisticsSource
    {
        public string Name { get; set; }

        public List<Counter> GameFavorite { get; set; }
        public List<Counter> GameIsInstalled { get; set; }
        public List<Counter> GameIsNotLaunching { get; set; }
        public List<Counter> GameHidden { get; set; }
        public List<Counter> GameCompletionStatus { get; set; }

        public long Playtime { get; set; }
        public int Total { get; set; }
    }
}
