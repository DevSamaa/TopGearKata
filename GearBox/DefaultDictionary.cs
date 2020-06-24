using System.Collections.Generic;

namespace GearBox
{
    public class DefaultDictionary
    {
        public IDictionary<int, int> RpmUpDictionary = new Dictionary<int, int>()
        {
            {1,2000},
            {2,2000},
            {3,2000},
            {4,2000},
            {5,2000}

        };
        
        public IDictionary<int, int> RpmDownDictionary = new Dictionary<int, int>()
        {
            {2,500},
            {3,500},
            {4,500},
            {5,500},
            {6,500}
        };
    }
}