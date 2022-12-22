using System.Collections;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace FidoDidoGame.Common.Sort;

public static class SortingBase
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> sourc, string? infoSortFromQuery)
    {
        if (!sourc.Any())
            return sourc;

        if (string.IsNullOrWhiteSpace(infoSortFromQuery))
            return sourc;

        string[] sortParam = infoSortFromQuery.Trim().Split(',');
        PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        StringBuilder querySortBuilder = new();

        foreach (string param in sortParam)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            string propertyNameFromQuery = param.Trim().Split(" ")[0];
            PropertyInfo propertyFromSourc = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyNameFromQuery, StringComparison.OrdinalIgnoreCase))!;

            if (propertyFromSourc is null)
                continue;

            if (propertyFromSourc.PropertyType != typeof(string) && propertyFromSourc.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                continue;

            string sortingRule = param.EndsWith(" desc") ? "descending" : "ascending";

            querySortBuilder = querySortBuilder.Append($"{propertyFromSourc.Name.ToString()} {sortingRule}, ");
        }

        string querySort = querySortBuilder.ToString().TrimEnd(' ', ',');


        if (string.IsNullOrWhiteSpace(querySort))
            return sourc;

        return sourc.OrderBy(querySort);
    }
}