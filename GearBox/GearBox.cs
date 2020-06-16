namespace GearBox
{
    public class GearBox
    {
        private int _gear = 0;
        private int _rpm = 0;

        //deprecated
        public void DoIt(int rpm) {
            ShiftGears(rpm);
        }

        public void ShiftGears(int rpm)
        {
            const int boundaryForShiftingUp = 2000;
            const int boundaryForShiftingDown = 500;
            const int neutral = 0;
            const int maxGear = 6;
            
            if (_gear >neutral && rpm >boundaryForShiftingUp )
            {
                _gear++;
            }

            if (_gear >neutral && rpm <boundaryForShiftingDown )
            {
                _gear--;
            }
            
            if (_gear == neutral)
            {
                _gear++;
            }

            if (_gear > maxGear)
            {
                _gear--;
            }

            _rpm = rpm;
        }
        public int S() => _gear;
        public int E() => _rpm;
    }
}

// 1st -> 2nd: 2000rpm
// 2nd -> 3rd: 2500rpm
// 3rd -> 4th: 2800rpm
// 4th -> 5th: 2200rpm
// 5th -> 6th: 1000rpm
// 4th -> 3rd: 532rpm