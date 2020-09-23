using System.Collections.Generic;
using System.Linq;
using SRTSubtitleConverter.Models;

namespace SRTSubtitleConverter
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
    }
}