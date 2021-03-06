﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPExtended.Services.MediaAccessService.Interfaces.Picture
{
    public class WebPictureBasic : WebMediaItem, ITitleSortable, IDateAddedSortable, IPictureDateTakenSortable, ICategorySortable
    {
        public WebPictureBasic()
        {
            DateTaken = new DateTime(1970, 1, 1);
            Categories = new List<WebCategory>();
        }

        public IList<WebCategory> Categories { get; set; }
        public string Title { get; set; }
        public DateTime DateTaken { get; set; }

        public override WebMediaType Type
        {
            get
            {
                return WebMediaType.Picture;
            }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}