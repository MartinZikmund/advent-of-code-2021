using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day24_1
{
    public class Rewrite
    {
        public long W1(byte w1)
        {
            var w = w1;
            var x = 0L;
            var y = 0L;
            var z = 0L;

            x = 1;
            y = w + 6;
            z = w + 6;

            return z;
        }

        public long W2(byte w2, long z)
        {
            var w = w2;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w2 + 11;
            z = z * 26 + (w2 + 11);

            return z;
        }

        public long W3(byte w3, long z)
        {
            var w = w3;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w3 + 5;
            z = z * 26 + (w3 + 5);

            return z;
        }

        public long W4(byte w4, long z)
        {
            var w = w4;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w4 + 6;
            z = z * 26 + (w4 + 6);

            return z;
        }

        public long W5(byte w5, long z)
        {
            var w = w5;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w5 + 8;
            z = z * 26 + (w5 + 8);

            return z;
        }

        public long W6(byte w6, long z)
        {
            var w = w6;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 1; // (w5 + 8) % 26 - 1
            x = x == w6 ? 0 : 1;

            y = (w6 + 14) * x;
            
            z /= 26; // now equal to (z3 * 26 + (w4 + 6)) from W4 as we cut off w5s changes
            z = z * (25 * x + 1) + (w6 + 14) * x;
            // if x == 0
            //    z = z4 * 26 + (w4 + 6) - this
            // else
            //    z = z4 * 26 + (w6 + 14)

            return z;
        }

        public long W7(byte w7, long z)
        {
            var w = w7;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w7 + 9;
            z = z * 26 + (w7 + 9);

            return z;
        }

        public long W8(byte w8, long z)
        {
            var w = w8;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 16; // (w7 + 9) % 26 - 16
            x = x == w8 ? 0 : 1;

            y = (w8 + 4) * x;

            z /= 26; // now equal to z6 from W6 as we cut off w7s changes
            z = z * (25 * x + 1) + (w8 + 4) * x;
            // if x == 0
            //    z = z6
            // else
            //    z = z6 * 26 + (w8 + 4)

            return z;
        }

        public long W9(byte w9, long z)
        {
            var w = w9;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 8;
            x = x == w9 ? 0 : 1;

            y = (w9 + 7) * x;

            z /= 26; // ?!??!???!
            z = z * (25 * x + 1) + (w9 + 7) * x;
            // if x == 0
            //    z = z8 / 26
            // else
            //    z = (z8 / 26) * 26 + (w9 + 7)

            return z;
        }

        public long W10(byte w10, long z) // IRRELEVANT - can be 9, without changing future z value
        {
            var w = w10;
            var x = 0L;
            var y = 0L;

            x = 1;
            y = w10 + 13;
            z = z * 26 + (w10 + 13);

            return z;
        }

        public long W11(byte w11, long z)
        {
            var w = w11;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 16;
            x = x == w11 ? 0 : 1;

            y = (w11 + 11) * x;

            z /= 26; // z9 (cut off w10s changes)
            z = z * (25 * x + 1) + (w11 + 11) * x;
            // if x == 0
            //    z = z9
            // else
            //    z = z9 * 26 + (w9 + 11)

            return z;
        }

        public long W12(byte w12, long z)
        {
            var w = w12;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 13;
            x = x == w12 ? 0 : 1;

            y = (w12 + 11) * x;

            z /= 26; // ?????????
            z = z * (25 * x + 1) + (w12 + 11) * x;
            // if x == 0
            //    z = z11 / 26
            // else
            //    z = (z11 / 26) * 26 + (w12 + 11)

            return z;
        }

        public long W13(byte w13, long z)
        {
            var w = w13;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 6;
            x = x == w13 ? 0 : 1;

            y = (w13 + 6) * x;

            z /= 26; // ?????????
            z = z * (25 * x + 1) + (w13 + 6) * x;
            // if x == 0
            //    z = z12 / 26
            // else
            //    z = (z12 / 26) * 26 + (w13 + 6)

            return z;
        }

        public long W14(byte w14, long z)
        {
            var w = w14;
            var x = 0L;
            var y = 0L;

            x = z % 26 - 6;
            x = x == w14 ? 0 : 1;

            y = (w14 + 1) * x;

            z /= 26; // ?????????
            z = z * (25 * x + 1) + (w14 + 1) * x;
            // if x == 0
            //    z = z13 / 26
            // else
            //    z = (z13 / 26) * 26 + (w14 + 1)

            return z;
        }
    }
}
