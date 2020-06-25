using System.Collections.Generic;

namespace GearBox
{
    public class DefaultDictionary
    {
        public IDictionary<int, Gear> RpmDictionary = new Dictionary<int, Gear>()
        {
            {1, new Gear { ShiftUp = 2000, ShiftDown = null }},
            {2, new Gear { ShiftUp = 2000, ShiftDown = 500 }},
            {3, new Gear { ShiftUp = 2000, ShiftDown = 500 }},
            {4, new Gear { ShiftUp = 2000, ShiftDown = 500 }},
            {5, new Gear { ShiftUp = 2000, ShiftDown = 500 }},
            {6, new Gear { ShiftUp = null, ShiftDown = 500 }},
        };
    }
}