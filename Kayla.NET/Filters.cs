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

        public static List<SubtitleItem> IncreaseSyncTime(double seconds, List<SubtitleItem> data)
        {
            var fixedItems = new List<SubtitleItem>();

            var convertedSeconds = TimeSpan.FromSeconds(seconds);

            foreach (var f in data)
            {
                var newStartTime = new TimeSpan(f.StartTime * 10000).Add(convertedSeconds).Ticks / 10000;
                var newEndTime = new TimeSpan(f.EndTime * 10000).Add(convertedSeconds).Ticks / 10000;

                f.StartTime = newStartTime;
                f.EndTime = newEndTime;

                fixedItems.Add(f);
            }

            return fixedItems;
        }

        public static List<SubtitleItem> DecreaseSyncTime(double seconds, List<SubtitleItem> data)
        {
            var fixedItems = new List<SubtitleItem>();

            var convertedSeconds = TimeSpan.FromSeconds(seconds);

            foreach (var f in data)
            {
                var newStartTime = new TimeSpan(f.StartTime * 10000).Subtract(convertedSeconds).Ticks / 10000;
                var newEndTime = new TimeSpan(f.EndTime * 10000).Subtract(convertedSeconds).Ticks / 10000;

                f.StartTime = newStartTime;
                f.EndTime = newEndTime;

                fixedItems.Add(f);
            }

            return fixedItems;
        }
    }
}