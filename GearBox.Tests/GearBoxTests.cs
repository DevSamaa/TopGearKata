using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GearBox.Tests
{
    public class GearBoxTests
    {
        private  GearBox _gearbox;

        public IDictionary<int, Gear> _rpmDictionary = new Dictionary<int, Gear>()
        {
            {1, new Gear { ShiftUp = 1300, ShiftDown = null }},
            {2, new Gear { ShiftUp = 2500, ShiftDown = 530 }},
            {3, new Gear { ShiftUp = 2800, ShiftDown = 550 }},
            {4, new Gear { ShiftUp = 2200, ShiftDown = 580 }},
            {5, new Gear { ShiftUp = 1000, ShiftDown = 600 }},
            {6, new Gear { ShiftUp = null, ShiftDown = 580 }},
        };

        public GearBoxTests()
        {
           _gearbox = new GearBox(_rpmDictionary);
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
          var boundaryForGear1 = _rpmDictionary[1].ShiftUp.Value;
          
          _gearbox.DoIt(boundaryForGear1 + 1);
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
            var boundaryForGear1 = _rpmDictionary[1].ShiftUp.Value;
            
            _gearbox.DoIt(100);
            _gearbox.DoIt(boundaryForGear1 + 1); 
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
            var maxGear = _rpmDictionary.Keys.Max();
            ShiftUpGears(amountOfGearChanges,_gearbox);
            var result = _gearbox.S();
            
            Assert.Equal(maxGear, result);
        }

        
        //Test Shifting Down
        private static IDictionary<int, Gear> ThreeGearsDown()
        {
            var dictionary = new Dictionary<int, Gear>()
            {
                {1, new Gear { ShiftDown = null, ShiftUp = 1300 }},
                {2, new Gear { ShiftDown = 430, ShiftUp = 2500 }},
                {3, new Gear { ShiftDown = 450, ShiftUp = null }},
            };
            return dictionary;
        }
        
        private static IDictionary<int, Gear> SixGearsDown()
        {
            var dictionary = new Dictionary<int, Gear>()
            {
                {1, new Gear { FuelUsage = 40, ShiftDown = null, ShiftUp = 1300 }},
                {2, new Gear { FuelUsage = 30, ShiftDown = 430, ShiftUp = 2500 }},
                {3, new Gear { FuelUsage = 50, ShiftDown = 450, ShiftUp = 2800 }},
                {4, new Gear { FuelUsage = 70, ShiftDown = 480, ShiftUp = 2200 }},
                {5, new Gear { FuelUsage = 80, ShiftDown = 400, ShiftUp = 1000 }},
                {6, new Gear { FuelUsage = 30, ShiftDown = 480, ShiftUp = null }},
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
        
        private void ShiftsDownWithRpmLowerThanThreshold(IDictionary<int,Gear> rpmDictionary, int startingGear)
        {
            //arrange
            var gearBox = new GearBox(rpmDictionary);
            ShiftUpGears(startingGear,gearBox); 
            
            //action
            gearBox.DoIt(rpmDictionary[startingGear].ShiftDown.Value - 1);
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


