// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

using Xunit;

namespace RefSemantics
{
    public class ReadonlyStruct
    {
        public readonly struct Point
        {
            // Compiler enforces readonly here
            public readonly float X;
            public readonly float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }

            // Compile error - immutable!
//            public void TranslateInPlace(float dx, float dy)
//            {
//                X += dx;
//                Y += dy;
//            }

            private static readonly Point _origin = new Point(0, 0);

            public static ref readonly Point Origin
            {
                get { return ref _origin; }
            }
        }

        [Fact]
        public void Non_ref_assignment_makes_copy()
        {
            // This is a copy, but we can't do anything dangerous anyway
            // It's immutable!
            var origin = Point.Origin;

            // Compile error
            //origin.X = 42;

            Assert.Equal(0, origin.X);
        }

        [Fact]
        public void TestRefReadonlyReturn()
        {
            ref readonly var origin = ref Point.Origin;

            // Compile error
            //origin.X = 42;

            // Method calls are on original value
            // But the method can't do anything dangerous
            // It's immutable!
//            origin.TranslateInPlace(42, 0);

            Assert.Equal(0, origin.X);
        }
    }
}