using System;
using System.Collections.Generic;
using System.Linq;

namespace GearBox
{
    public class GearBox
    {
        private int _gear = 0;
        private int _rpm = 0;
        private readonly IDictionary<int, Gear> _rpmDictionary;

        public GearBox()
        {
            //default constructor (if nothing else is passed in, this will be used.)
            _rpmDictionary = new DefaultDictionary().RpmDictionary;
        }
        public GearBox(IDictionary<int,Gear> incomingDictionary)
        {
            _rpmDictionary = incomingDictionary;
        }

        //deprecated
        public void DoIt(int rpm) {
            ShiftGears(rpm);
        }
        
        private void ShiftGears(int rpm)
        {
            
            if (ShouldShiftUp(rpm))
            {
                _gear++;
            }

            if (ShouldShiftDown(rpm))
            {
                _gear--;
            }
            
            _rpm = rpm;
        }

        private Gear GetCurrentGear(IDictionary<int,Gear> incomingDictionary)
        {
            if (incomingDictionary.ContainsKey(_gear))
            {
                return incomingDictionary[_gear];
            }

            return null;
        }
        
        private bool ShouldShiftUp(int rpm)
        {
            const int neutral = 0;
            if (_gear == neutral)
                return true;

            var gearObj = GetCurrentGear(_rpmDictionary);

            if (gearObj?.ShiftUp == null)
            {
                return false;
            }
            
            return rpm > gearObj.ShiftUp.Value;
        }

        private bool ShouldShiftDown(int rpm)
        {
            if (_gear < 2)
            {
                return false;
            }
            
            var gearObj = GetCurrentGear(_rpmDictionary);
            
            if (gearObj?.ShiftDown == null)
            {
                return false;
            }
            
            return rpm < gearObj.ShiftDown.Value;
        }


        public int S() => _gear;
        public int E() => _rpm;
    }
}

