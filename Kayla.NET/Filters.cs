using System;
using System.Collections.Generic;
using System.Linq;
using Kayla.NET.Models;

namespace Kayla.NET
{
    public static class Filters
    {
        public static List<SubtitleItem> RemoveDuplicateItems(List<SubtitleItem> data)
        {
            var filteredItems = new List<SubtitleItem>();
            var previousItem = new SubtitleItem();

            foreach (var d in data.Where(d =>
                previousItem.StartTime != d.StartTime || previousItem.EndTime != d.EndTime ||
                previousItem.Text != d.Text))
            {
                previousItem = d;
                filteredItems.Add(d);
            }

            return filteredItems;
        }

        public static List<SubtitleItem> AdjustSyncTime(double seconds, List<SubtitleItem> data)
        {
            var fixedItems = new List<SubtitleItem>();

            var convertedSeconds = TimeSpan.FromSeconds(seconds);

            foreach (var f in data)
            {
                f.StartTime = new TimeSpan(f.StartTime * 10000).Add(convertedSeconds).Ticks / 10000;
                f.EndTime = new TimeSpan(f.EndTime * 10000).Add(convertedSeconds).Ticks / 10000;

                fixedItems.Add(f);
            }

            return fixedItems;
        }
    }
}