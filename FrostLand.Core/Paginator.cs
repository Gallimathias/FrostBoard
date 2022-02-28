using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public record struct Paginator (int Total, int Size, int Length)
    {       
        public static Page<T> GetPage<T>(Paginator paginator, int index, Span<T> collection) 
            => new(index, GetSpan(paginator, index, collection));

        public static Span<T> GetSpan<T>(Paginator paginator, int index, Span<T> collection)
        {
            var startIndex = index * paginator.Size;
            var endIndex = (startIndex + paginator.Size);
            endIndex = endIndex >= collection.Length ? collection.Length - 1 : endIndex;
            return collection[startIndex..(startIndex + paginator.Size)];
        }
        
        public static Paginator GetPaginator<T>(Span<T> collection, int size)
            => new(collection.Length / size, size, collection.Length);
    }
}
