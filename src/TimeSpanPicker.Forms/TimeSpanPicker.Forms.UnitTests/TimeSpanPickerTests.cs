using System;
using Xunit;

namespace TimeSpanPicker.Forms.UnitTests
{
    public class TimeSpanPickerTests
    {
        [Fact]
        public void Constructor()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();

            Assert.Equal(TimeSpan.Zero, tsp.Time);
            Assert.Equal(TimeSpan.Zero, tsp.MinTime);
            Assert.Equal(new TimeSpan(0, 23, 59, 59, 999), tsp.MaxTime);
        }

        [Fact]
        public void MinTimeWithInvalidValues()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();

            Assert.ThrowsAny<Exception>(() => tsp.MinTime = TimeSpan.FromSeconds(-1));
            Assert.ThrowsAny<Exception>(() => tsp.MinTime = TimeSpan.FromDays(1));
        }

        [Fact]
        public void MaxTimeWithInvalidValues()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();

            Assert.ThrowsAny<Exception>(() => tsp.MaxTime = TimeSpan.FromSeconds(-1));
            Assert.ThrowsAny<Exception>(() => tsp.MaxTime = TimeSpan.FromDays(1));
        }

        [Fact]
        public void MinTimeGreaterThanMaxTime()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();
            tsp.MaxTime = TimeSpan.FromMinutes(5);

            Assert.ThrowsAny<Exception>(() => tsp.MinTime = TimeSpan.FromMinutes(6));
        }

        [Fact]
        public void MaxTimeLessThanMinTime()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();
            tsp.MinTime = TimeSpan.FromMinutes(6);

            Assert.ThrowsAny<Exception>(() => tsp.MaxTime = TimeSpan.FromMinutes(5));
        }

        [Fact]
        public void CoerceOnMinTimeChanged()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();
            tsp.Time = TimeSpan.FromMinutes(5);
            tsp.MinTime = TimeSpan.FromMinutes(6);

            Assert.Equal(tsp.MinTime, tsp.Time);
        }

        [Fact]
        public void CoerceOnMaxTimeChanged()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();
            tsp.Time = TimeSpan.FromMinutes(6);
            tsp.MaxTime = TimeSpan.FromMinutes(5);

            Assert.Equal(tsp.MaxTime, tsp.Time);
        }

        [Fact]
        public void CoerceOnTimeChanged()
        {
            var tsp = new Rotorsoft.Forms.TimeSpanPicker();
            tsp.Time = TimeSpan.FromMinutes(10);
            tsp.MinTime = TimeSpan.FromMinutes(5);
            tsp.MaxTime = TimeSpan.FromMinutes(15);
            tsp.Time = TimeSpan.FromMinutes(3);

            Assert.Equal(tsp.MinTime, tsp.Time);

            tsp.Time = TimeSpan.FromMinutes(16);

            Assert.Equal(tsp.MaxTime, tsp.Time);
        }
    }
}
