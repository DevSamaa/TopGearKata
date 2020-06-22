using System;
using System.Collections.Generic;
using Xunit;

namespace GearBox.Tests
{
    public class GearBoxTests
    {
        private  GearBox _gearbox;
        
        private IDictionary<int, int> _rpmUpDictionary = new Dictionary<int, int>()
        {
            {1,1300},
            {2,2500},
            {3,2800},
            {4,2200},
            {5,1000}

        };
        
        private IDictionary<int, int> _rpmDownDictionary = new Dictionary<int, int>()
        {
            {2,530},
            {3,550},
            {4,580},
            {5,600},
            {6,580}
        };

        public GearBoxTests()
        {
           _gearbox = new GearBox(_rpmUpDictionary, _rpmDownDictionary);
        }

        
        // new GearBox always starts with Gear (S) in neutral (0)
        [Fact]
        public void NewGearBoxStartsIn0() {
            var result = _gearbox.S();
            var expected = 0;
            Assert.Equal(expected, result);
        }
        
        //shifting up with specific rpm
        [Fact]
        public void ShiftFrom1to2WithSpecificRPM()
        {
          ShiftUpGears(1);
          var rpms =_rpmUpDictionary;
          var boundaryForGear1 = rpms[1];
          
            _gearbox.DoIt(boundaryForGear1 +1);
            var resultAfterDoIt = _gearbox.S();
            Assert.Equal(2, resultAfterDoIt);
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
        
        //When you're in first gear and call DoIt with an RPM >boundary, the gear should shift up
        [Fact]
        public void Given1stGearAndRPM2001_ShouldReturnS2()
        {
            var boundaryForGear1 = _rpmUpDictionary[1];
            
            _gearbox.DoIt(100);
            _gearbox.DoIt(boundaryForGear1+1); 
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
            _gearbox.DoIt(0); //from neutral to first gear
            for (int i = 1; i < gearShifts; i++)
            {
                if (_rpmUpDictionary.ContainsKey(i))
                {
                    _gearbox.DoIt(_rpmUpDictionary[i] +1);
                }
                else
                {
                        _gearbox.DoIt(int.MaxValue);
                }
            }
        }
        
        
        //Test Shifting Down
        [Theory]
        [InlineData(529,2,1)]
        [InlineData(530,2,2)]
        [InlineData(549,3,2)]
        [InlineData(588,6,6)]
        [InlineData(579,6,5)]

        private void ShiftsDownWithRPMLowerThanThreshold(int rpm, int startingGear,int expectedNewGear)
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


