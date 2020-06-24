using System;
using System.Collections.Generic;
using System.Linq;
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

        //new GearBox() without passing a dictionary can be created
        [Fact]
        public void GearBoxCreatedWithoutDictionaryCanShiftUp()
        {
            var gearBox = new GearBox();
            gearBox.DoIt(0);
            var result = gearBox.S();
            Assert.Equal(1,result);
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
        public void ShiftFrom1To2WithSpecificRpm()
        {
          ShiftUpGears(1, _gearbox);
          var boundaryForGear1 = _rpmUpDictionary[1];
          
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
        public void ShiftFrom0To1StGearWithAnyRpm(int rpm)
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
        
        // _rpmDownDictionary indicates the highest gear, the max gear should never be > highest number in that dictionary
        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(20)]
        public void ShouldNotShiftUpPastMaxGear(int amountOfGearChanges)
        {
            var maxGear = _rpmDownDictionary.Keys.Max();
            ShiftUpGears(amountOfGearChanges,_gearbox);
            var result = _gearbox.S();
            
            Assert.Equal(maxGear, result);
        }

        
        //Test Shifting Down
        private static IDictionary<int, int> ThreeGearsDown()
        {
            var dictionary = new Dictionary<int, int>()
            {
                {2,430},
                {3,450},
            };
            return dictionary;
        }
        
        private static IDictionary<int, int> SixGearsDown()
        {
            var dictionary = new Dictionary<int, int>()
            {
                {2,430},
                {3,450},
                {4,480},
                {5,400},
                {6,480}
            };
            return dictionary;
        }

        private IDictionary<int, int> DefaultUpDict()
        {
            var dictionary = new Dictionary<int, int>()
            {
                {1, 1300},
                {2, 2500},
                {3, 2800},
                {4, 2200},
                {5, 1000}
            };
            return dictionary;
        }
        
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] {SixGearsDown(),6};
            yield return new object[] {SixGearsDown(),5};
            yield return new object[] {SixGearsDown(),3};
            yield return new object[] {ThreeGearsDown(),3};
            yield return new object[] {ThreeGearsDown(),2};
        }

        
       [Theory]
       [MemberData(nameof(TestData))]
        
        private void ShiftsDownWithRpmLowerThanThreshold(IDictionary<int,int>downDictionary, int startingGear)
        {
            //arrange
            var upDictionary = DefaultUpDict();
            var gearBox = new GearBox(upDictionary,downDictionary);
            ShiftUpGears(startingGear,gearBox); 
            
            //action
            gearBox.DoIt(downDictionary[startingGear]-1);
            var result = gearBox.S();
            var expectedGear = startingGear - 1;
            Assert.Equal(expectedGear, result);
        }
        
        private void ShiftUpGears(int gearShifts, GearBox gearBox)
        {
            gearBox.DoIt(0); //from neutral to first gear
            for (int i = 1; i < gearShifts; i++)
            {
                gearBox.DoIt(int.MaxValue);
            }
        }
        
        [Theory]
        [InlineData(529,2,1)]
        [InlineData(530,2,2)]
        [InlineData(549,3,2)]

        private void OnlyShiftsDownWithRPMLowerThanThreshold(int rpm, int startingGear,int expectedNewGear)
        {
            //arrange
            ShiftUpGears(startingGear, _gearbox); 
            
            //action
             _gearbox.DoIt(rpm);
             var result = _gearbox.S(); 
             
             //assert
             Assert.Equal(expectedNewGear, result);
        }
        
        
        [Theory]
        [InlineData(499,1,1)]
        [InlineData(0,1,1)]
        [InlineData(-5,1,1)]
        private void ShouldNotShiftDownFrom1stTo0Ever(int rpm, int startingGear,int expectedNewGear)
        {
            ShiftUpGears(startingGear, _gearbox); 
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


