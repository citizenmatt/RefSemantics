// ReSharper disable SuggestVarOrType_Elsewhere

using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace RefSemantics
{
    public class Span
    {
        private const string MyString = "Hello world";
        private static readonly int[] Numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

        [Fact]
        public void String_as_read_only_span()
        {
            ReadOnlySpan<char> span = MyString.AsSpan();
            AssertEqual(span.Length, "Hello world".Length);
            AssertTrue(span.Equals("Hello world", StringComparison.InvariantCultureIgnoreCase));
            AssertTrue(span.StartsWith("Hello"));
        }

        [Fact]
        public void Substring_as_read_only_slice()
        {
            ReadOnlySpan<char> span = MyString.AsSpan();
            ReadOnlySpan<char> slice = span.Slice(6);
            AssertEqual("world".Length, slice.Length);
            AssertTrue(slice.Equals("world", StringComparison.InvariantCultureIgnoreCase));
        }

        [Fact]
        public void Array_as_mutable_span_and_slice()
        {
            Span<int> numberSpan = Numbers;
            Span<int> numberSlice = numberSpan.Slice(3, 5);
            numberSlice[2] = 100;

            AssertEqual(1, Numbers[0]);
            AssertEqual(2, Numbers[1]);
            AssertEqual(3, Numbers[2]);
            AssertEqual(4, Numbers[3]);
            AssertEqual(5, Numbers[4]);
            AssertEqual(100, Numbers[5]);
            AssertEqual(7, Numbers[6]);
            AssertEqual(8, Numbers[7]);
            AssertEqual(9, Numbers[8]);
            AssertEqual(10, Numbers[9]);
        }

        [Fact]
        public void Allocation_free()
        {
            var totalMemory = GC.GetTotalMemory(true);
            AssertTrue(GC.TryStartNoGCRegion(1000));

            String_as_read_only_span();
            Substring_as_read_only_slice();
            Array_as_mutable_span_and_slice();

            // Crude, not a guarantee that nothing was allocated
            GC.EndNoGCRegion();
            Assert.Equal(0, GC.GetTotalMemory(false) - totalMemory);
        }

        #region AssertEqual implementation details
        private static readonly IEqualityComparer<int> IntEqualityComparer = EqualityComparer<int>.Default;

        static Span()
        {
            // We need to call these once, or we get allocations in the test. I don't really know why, but I'm guessing
            // it's something to do with JIT?
            AssertEqual(1, 1);
            AssertTrue(true);
            var xx = new Span();
            xx.String_as_read_only_span();
            xx.Substring_as_read_only_slice();
            xx.Array_as_mutable_span_and_slice();
        }

        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
        private static void AssertEqual(int expected, int actual)
        {
            Assert.Equal(expected, actual, IntEqualityComparer);
        }

        private static void AssertTrue(bool condition)
        {
            if (!condition)
                throw new TrueException(null, false);
        }
        #endregion
    }
}