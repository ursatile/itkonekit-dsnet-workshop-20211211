using System;
using System.Collections.Generic;

namespace Autobarn.Website.Controllers.api {
    public static class Hal {
        public static Dictionary<string, object> Paginate(
            string baseUrl,
            int index,
            int count,
            int total
        ) {
            var links = new Dictionary<string, object>();
            links.Add("self", new { href = $"{baseUrl}?index={index}&count={count}" });
            if (index + count < total) {
                links.Add("next", new { href = $"{baseUrl}?index={index + count}&count={count}" });
            }
            if (index > 0) {
                links.Add("prev", new { href = $"{baseUrl}?index={Math.Max(0, index - count)}&count={count}" });
            }
            links.Add("first", new { href = $"{baseUrl}?index=0&count={count}" });
            links.Add("final", new { href = $"{baseUrl}?index={total - count}&count={count}" });
            return links;
        }
    }
}