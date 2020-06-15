﻿namespace GearBox
{
    public class GearBox
    {
        private int s = 0;
        private int e = 0;

        public void DoIt(int i) {
            if (s < 0) {
                e = i;
            } 
            else {
                if (s > 0) {
                    if (i > 2000) {
                        s++;
                    } 
                    else if (i < 500) {
                        s--;
                    }
                }
            }	
		
            if (s > 6) {
                s--;
            } 
            
            else if (s < 1) {
                s++;
            }
		
            e = i;
        }

        public int S() => s;
        public int E() => e;
    }
}