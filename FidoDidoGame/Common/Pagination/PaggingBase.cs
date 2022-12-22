namespace FidoDidoGame.Common.Pagination
{
    public static class PaggingBase
    {
        public static PaggingResponse<T> ApplyPagging<T>(this IQueryable<T> source, int current, int pageSize)
        {
            int count = source.Count();
            PageInfo pageInfo = new PageInfo(count, pageSize, current);
            List<T> items = source.Skip((pageInfo.Current - 1) * pageSize).Take(pageSize).ToList();
            return new PaggingResponse<T>(items, pageInfo);
        }
    }

    public class PageInfo
    {
        public PageInfo(int totalCount, int pageSize, int current)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Current = current > TotalPages ? TotalPages : current;
        }

        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; set; }
        public int Current { get; set; }
        public int? NextPage => Current >= TotalPages ? null : Current + 1;
        public int? PreviousPage => Current <= 1 ? null : Current - 1;
        public bool HasNext => NextPage != null;
        public bool HasPrevious => PreviousPage != null;
    }

    public class PaggingResponse<T>
    {
        public PaggingResponse(List<T> data, PageInfo pageInfo)
        {
            Data = data;
            PageInfo = pageInfo;
        }

        public List<T> Data { get; set; } = default!;
        public PageInfo PageInfo { get; set; } = default!;
    }
}