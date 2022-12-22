using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace FidoDidoGame.Common.Search
{
    public static class SearchingBase
    {
        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> sourc, string infoSearchFromQuery)
        {
            if (!sourc.Any())
                return sourc;

            if (string.IsNullOrWhiteSpace(infoSearchFromQuery))
                return sourc;

            string[] paramFromQuery = infoSearchFromQuery.Trim().Split(',');
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            StringBuilder querySearchBuilder = new();

            foreach (string param in paramFromQuery)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                string[] propertyFromQuery = param.Trim().Split(' ');
                PropertyInfo propertyFromSourc = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyFromQuery[0],
                    StringComparison.OrdinalIgnoreCase))!;

                if (propertyFromSourc is null)
                    continue;

                if (propertyFromSourc.PropertyType != typeof(string))
                    continue;

                querySearchBuilder = querySearchBuilder.Append($"{propertyFromSourc.Name.ToString()}.Contains(\"{propertyFromQuery[1]}\")||");
            }

            string querySearch = querySearchBuilder.ToString().TrimEnd('&');

            if (string.IsNullOrEmpty(querySearch))
                return sourc;

            return sourc.Where(querySearch);
        }
    }
}