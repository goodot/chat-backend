using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Domain.Extensions.Paging
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IEnumerable<T> Results { get; set; }

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
