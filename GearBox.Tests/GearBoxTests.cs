using Xunit;

namespace GearBox.Tests
{
    public class GearBoxTests
    {
        private  GearBox _gearbox;
        public GearBoxTests()
        {
           _gearbox = new GearBox();
        }
        
        // new GearBox always starts with Gear (S) in neutral (0)
        [Fact]
        public void NewGearBoxStartsIn0() {
            var result = _gearbox.S();
            var expected = 0;
            Assert.Equal(expected, result);
        }

        //Shifting UP
        // Calling DoIt for the first time shifts the gear (S) from neutral (0) to 1st
        [Fact]
        public void FirstDoItChangesSFrom0To1()
        {
            var resultBeforeDoIt = _gearbox.S();
            Assert.Equal(0,resultBeforeDoIt);
            
            _gearbox.DoIt(100);
            var resultAfterDoIt = _gearbox.S();
            Assert.Equal(1, resultAfterDoIt);
        }
        
        
        //Doit will shift from neutral to first gear with ANY rpm
        [Theory]
        [InlineData(100)]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(7)]
        [InlineData(2000)]
        [InlineData(2001)]
        public void ShiftFrom0To1stGearWithAnyRPM(int rpm)
        {
            _gearbox.DoIt(rpm);
            var result = _gearbox.S();
            Assert.Equal(1, result);
        }
        
        //When you're in first gear and call DoIt with an RPM >2000, the gear should shift up
        [Fact]
        public void Given1stGearAndRPM2001_ShouldReturnS2()
        {
            _gearbox.DoIt(100);
            _gearbox.DoIt(2001);
            var result = _gearbox.S();
            Assert.Equal(2, result);
        }
        
        // 6 is the maximum, gear (S) is never larger than 6
        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(20)]
        public void MaximumGearIs6(int amountOfGearChanges)
        {
            ShiftUpGears(amountOfGearChanges);
            var result = _gearbox.S();
            
            Assert.Equal(6, result);
        }

        private void ShiftUpGears(int gearShifts)
        {
            for (int i = 0; i < gearShifts; i++)
            {
                _gearbox.DoIt(2001);
            }
        }
        
        
        //Test Shifting Down
        [Theory]
        [InlineData(499,6,5)]
        [InlineData(500,6,6)]
        [InlineData(499,2,1)]
        [InlineData(501,2,2)]
        private void ShiftsDownWithRPMLowerThan500(int rpm, int startingGear,int expectedNewGear)
        {
            ShiftUpGears(startingGear); 
             _gearbox.DoIt(rpm);
             var result = _gearbox.S(); 
             Assert.Equal(expectedNewGear, result);
        }
        
        [Theory]
        [InlineData(499,1,1)]
        [InlineData(0,1,1)]
        [InlineData(-5,1,1)]
        
        private void ShouldNotShiftDownFrom1stTo0Ever(int rpm, int startingGear,int expectedNewGear)
        {
            ShiftUpGears(startingGear); 
            _gearbox.DoIt(rpm);
            var result = _gearbox.S(); 
            Assert.Equal(expectedNewGear, result);
        }

        [Fact]
        private void EShouldReturnTheLastRPM()
        {
            var inputRPM = 999;
            _gearbox.DoIt(inputRPM);
            var result =_gearbox.E();
            Assert.Equal(inputRPM,result);

        }
    }    
}
