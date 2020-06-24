using System;
using System.Collections.Generic;
using System.Linq;

namespace GearBox
{
    public class GearBox
    {
        private int _gear = 0;
        private int _rpm = 0;
        private readonly IDictionary<int, int> _rpmUpDictionary;
        private readonly IDictionary<int, int> _rpmDownDictionary;

        public GearBox()
        {
            //default constructor (if nothing else is passed in, this will be used.)
             _rpmUpDictionary = new DefaultDictionary().RpmUpDictionary;
            _rpmDownDictionary = new DefaultDictionary().RpmDownDictionary;
        }
        public GearBox(IDictionary<int,int> incomingUpDictionary, IDictionary<int,int> incomingDownDictionary)
        {
            _rpmUpDictionary = incomingUpDictionary;
            _rpmDownDictionary = incomingDownDictionary;
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

        private int? BoundaryForShifting(IDictionary<int,int>incomingDictionary)
        {
            int boundaryForShifting = 0;
            if (incomingDictionary.ContainsKey(_gear))
            {
                boundaryForShifting = incomingDictionary[_gear];
            }
            else
            {
                return null;
            }
            return boundaryForShifting;
        }

        

        private bool ShouldShiftUp(int rpm)
        {
            const int neutral = 0;
            var boundary = BoundaryForShifting(_rpmUpDictionary);
            return _gear > neutral && boundary.HasValue && rpm > boundary|| _gear ==neutral;
        }

        private bool ShouldShiftDown(int rpm)
        {
            return _gear >= 2 && rpm < BoundaryForShifting(_rpmDownDictionary);
        }


        public int S() => _gear;
        public int E() => _rpm;
    }
}

