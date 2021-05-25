using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrostLand.Core
{
    public struct Page<T>
    {
        public readonly int Index { get; }
        public readonly int Size { get; }

        public IReadOnlyList<T> Items => pageData;

        private readonly T[] pageData;

        public Page(int index, Span<T> pageData)
        {
            Index = index;
            this.pageData = pageData.ToArray();
            Size = pageData.Length;
        }
        public Page(int index, IEnumerable<T> pageData)
        {
            Index = index;
            this.pageData = pageData.ToArray();
            Size = this.pageData.Length;
        }
    }
}
