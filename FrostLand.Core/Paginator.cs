using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public readonly struct Paginator : IEquatable<Paginator>
    {
        public readonly int Total { get;  }
        public readonly int Size { get;  }
        public readonly int Length { get;  }

        public Paginator(int total, int size, int length)
        {
            Total = total;
            Size = size;
            Length = length;
        }

        public override bool Equals(object obj) 
            => obj is Paginator paginator && Equals(paginator);
        public bool Equals(Paginator other) 
            => Total == other.Total && Size == other.Size && Length == other.Length;
        public override int GetHashCode() 
            => HashCode.Combine(Total, Size, Length);

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

        public static bool operator ==(Paginator left, Paginator right) 
            => left.Equals(right);
        public static bool operator !=(Paginator left, Paginator right) 
            => !(left == right);
    }
}
