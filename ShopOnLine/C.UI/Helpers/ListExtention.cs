using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace C.UI.Helpers
{
    public static class ListExtention
    {
        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string defaultOption,
            string defaultOptionValue)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = defaultOption,
                Value = defaultOptionValue
            });
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string defaultOption,
            string defaultOptionValue,
            string selectedValue)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f),
                Selected = (value(f).ToString() == selectedValue)
            }).ToList();
            items.Insert(0, new SelectListItem()
            {
                Text = defaultOption,
                Value = defaultOptionValue
            });
            return items;
        }

        public static List<SelectListItem> ToSelectList<T>(
            this IEnumerable<T> enumerable,
            Func<T, string> text,
            Func<T, string> value,
            string selectedValue
            )
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = text(f),
                Value = value(f),
                Selected = (value(f).ToString() == selectedValue)
            }).ToList();
            return items;
        }
    }
}
