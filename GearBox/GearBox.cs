using System;
using System.Collections.Generic;

namespace GearBox
{
    public class GearBox
    {
        private int _gear = 0;
        private int _rpm = 0;
        private IDictionary<int, int> _rpmUpDictionary;
        private IDictionary<int, int> _rpmDownDictionary;


        public GearBox(IDictionary<int,int> incomingUpDictionary, IDictionary<int,int> incomingDownDictionary)
        {
            _rpmUpDictionary = incomingUpDictionary;
            _rpmDownDictionary = incomingDownDictionary;
        }

        //deprecated
        public void DoIt(int rpm) {
            ShiftGears(rpm);
        }

        private int BoundaryForShifting(IDictionary<int,int>incomingDictionary)
        {
            int boundaryForShifting = 0;
            if (incomingDictionary.ContainsKey(_gear))
            {
                boundaryForShifting = incomingDictionary[_gear];
            }
            else
            {
                throw new Exception("invalid gear");
            }

            return boundaryForShifting;
        }
        public void ShiftGears(int rpm)
        {
            const int boundaryForShiftingDown = 500;
            const int neutral = 0;
            const int maxGear = 6;
            
            if (_gear > neutral &&_gear < maxGear && rpm >BoundaryForShifting(_rpmUpDictionary)  || _gear ==neutral)
            {
                _gear++;
            }

            if (_gear >=2 && rpm <BoundaryForShifting(_rpmDownDictionary) )
            {
                _gear--;
            }
            
            // if (_gear == neutral)
            // {
            //     _gear++;
            // }

            // if (_gear > maxGear)
            // {
            //     _gear--;
            // }

            _rpm = rpm;
        }

        
       
        public int S() => _gear;
        public int E() => _rpm;
    }
}

