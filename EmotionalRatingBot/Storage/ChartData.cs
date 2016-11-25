using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmotionalRatingBot.Storage
{
    public class ChartData
    {
        public int PrimaryRating { get; set; }

        public IDictionary<string, int> Emotions { get; set; }

        public float Sex { get; set; }
    }
}