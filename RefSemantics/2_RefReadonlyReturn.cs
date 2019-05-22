// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

using Xunit;

namespace RefSemantics
{
    public class RefReadonlyReturn
    {
        public struct Point
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }

            public void TranslateInPlace(float dx, float dy)
            {
                X += dx;
                Y += dy;
            }


            public static ref Point Origin => ref _origin;
            private static Point _origin = new Point(0, 0);



            public static ref readonly Point ReadonlyOrigin
            {
                get { return ref _readonlyOrigin; }
            }
            private static readonly Point _readonlyOrigin = new Point(0, 0);
        }

        [Fact]
        public void Non_ref_variable_receives_copy()
        {
            // Receives copy, not reference. This won't modify Point.Origin
            var copy = Point.Origin;
            copy.X = 42;

            Assert.Equal(0, Point.Origin.X);
            Assert.Equal(42, copy.X);
        }

        [Fact]
        public void Non_readonly_ref_variable_modifies_shared_state()
        {
            // Non readonly reference! Modifies shared state
            ref var origin = ref Point.Origin;
            origin.X = 42;

            Assert.Equal(42, Point.Origin.X);
        }

        [Fact]
        public void Ref_readonly_cannot_be_modified()
        {
            ref readonly var origin = ref Point.ReadonlyOrigin;

            // Compile error!
//            origin.X = 42;

            Assert.Equal(0, origin.X);
            Assert.Equal(0, Point.ReadonlyOrigin.X);
        }

        [Fact]
        public void Ref_readonly_makes_copy_for_method_call()
        {
            ref readonly var origin = ref Point.ReadonlyOrigin;

            // Method call happens on defensive copy
            origin.TranslateInPlace(42, 0);

            Assert.Equal(0, Point.ReadonlyOrigin.X);
            Assert.Equal(0, origin.X);
        }

        public RefReadonlyReturn()
        {
            // Reset everything between tests. Ignore me! ☺️
            Point.Origin.X = 0;
        }
    }
}