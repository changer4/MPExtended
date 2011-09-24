﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPExtended.Services.MediaAccessService.Interfaces.Shared;

namespace MPExtended.Services.MediaAccessService.Interfaces.TVShow
{
    public class WebTVEpisodeBasic : WebMediaItem, ITitleSortable, IDateAddedSortable, IRatingSortable, ITVEpisodeNumberSortable
    {
        public WebTVEpisodeBasic()
        {
            DateAdded = new DateTime(1970, 1, 1);
        }

        public string Id { get; set; }
        public string ShowId { get; set; }
        public string Title { get; set; }
        public int EpisodeNumber { get; set; }
        public string SeasonId { get; set; }
        public IList<string> Path { get; set; }
        public bool IsProtected { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Watched { get; set; }
        public float Rating { get; set; }
        public IList<string> BannerPaths { get; set; }
        public DateTime FirstAired { get; set; }

        public WebMediaType Type
        {
            get
            {
                return WebMediaType.TVEpisode;
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}