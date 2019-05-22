// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NotAccessedField.Global
// ReSharper disable SuggestVarOrType_SimpleTypes

using Xunit;

namespace RefSemantics
{
    public class RefReturnAndRefLocal
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
        }

        public class Enemy
        {
            private Point _location;

            public Enemy(Point location)
            {
                _location = location;
            }

            public Point GetLocation()
            {
                return _location;
            }

            public ref Point GetLocationByRef()
            {
                return ref _location;
            }
        }

        [Fact]
        public void Struct_return_value_is_a_copy()
        {
            var enemy = new Enemy(new Point(10, 10));

            // This is a copy!
            Point location = enemy.GetLocation();
            location.X = 12;

            Assert.Equal(12, location.X);
            Assert.Equal(10, enemy.GetLocation().X);
        }

        [Fact]
        public void Ref_return_value_is_a_copy()
        {
            var enemy = new Enemy(new Point(10, 10));

            // This is a copy, even returned by ref!
            Point copy = enemy.GetLocationByRef();
            copy.X = 12;

            Assert.Equal(12, copy.X);
            Assert.Equal(10, enemy.GetLocationByRef().X);
        }

        [Fact]
        public void Ref_return_to_var_ref_no_copy()
        {
            var enemy = new Enemy(new Point(10, 10));

            // Reference, not copy
            ref var location = ref enemy.GetLocationByRef();
            location.X = 12;

            Assert.Equal(12, enemy.GetLocation().X);
        }

        [Fact]
        public void Use_method_as_left_hand_side()
        {
            var enemy = new Enemy(new Point(10, 10));

            enemy.GetLocationByRef() = new Point(42, 42);

            Assert.Equal(42, enemy.GetLocation().X);
        }
    }
}