﻿using System;
using System.Security.Cryptography;

namespace Fooz.Math
{
    /// <summary>
    /// Adapter pattern to adapt the RNGCryptoServiceProvider to the Random interface.
    /// RNGCryptoServiceProvider is a cryptographically strong random number generator.
    /// It is not as performant as Random, but random values generated by it are
    /// indistiguishable from truely random numbers. Use it when you need truly random
    /// numbers. If you want a repeatable list of psuedo random numbers based on a seed 
    /// value, use Random.
    /// </summary>
    public class RandomNumberGenerator : Random
    {
        private RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();
        private byte[] _uint32Buffer = new byte[sizeof(UInt32)];
        private Object _lock = new Object(); // Object to lock to make class thread-safe

        /// <summary>
        /// Initializes a new instance of the RandonNumberGenerator class.
        /// </summary>
        public RandomNumberGenerator() { }

        /// <summary>
        /// Initializes a new instance of the RandonNumberGenerator class.
        /// </summary>
        /// <param name="ignoredSeed">Ignored parameter. Supplied to provide the same interface as Random.</param>
        public RandomNumberGenerator(Int32 ignoredSeed) { }

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than MaxValue.</returns>
        public override Int32 Next()
        {
            Int32 random;

            lock (_lock)
            {
                _rng.GetBytes(_uint32Buffer);
                // Strip off the high-order bit to make it unsigned.
                random = BitConverter.ToInt32(_uint32Buffer, 0) & 0x7FFFFFFF;
            }

            return random;
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than maxValue; that is, the range of return values ordinarily 
        /// includes 0 but not maxValue. However, if maxValue equals 0, maxValue is returned.</returns>
        public override Int32 Next(Int32 maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");

            // Delegate to overload that takes a minValue and a maxValue useing 0 as minValue
            return Next(0, maxValue);
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.
        /// maxValue must be greater than or equal to minValue.</param>
        /// <returns>A 32-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes 
        /// minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public override Int32 Next(Int32 minValue, Int32 maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
                return minValue;

            Int32 random = -1;

            // We only want to return a random number if the random number falls with the range of bytes evenly.
            // If you want a number from 0 to 6, and a byte can have a value 0 - 255, the pattern 0 - 6 repeats
            // 42 times and the pattern 0 - 2 appears 43 times (6 divideds into 255 42 times with a remainder of 3).
            // This makes lower numbers more likely. So only use the generated random number if it is in the
            // 0 - 252 range so each number has an equal chance of being selected.

            // diff is the number of possible random numbers we are interested in.
            Int64 diff = maxValue - minValue;

            // Repeat until we get a number in the range of numbers that repeat an even number of times.
            while (random == -1)
            {
                lock (_lock)
                {
                    // Get a random integer.
                    _rng.GetBytes(_uint32Buffer);
                    UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);

                    // max is the max value a random number can have.
                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    // remainder is the number of results we want to ignore because choosing one of them
                    // would allow more lower numbers to be choosen.
                    Int64 remainder = max % diff;
                    // If the choosen number does not fall in the remainder range, use it.
                    // Other wise continue the loop.
                    if (rand < max - remainder)
                    {
                        random = (Int32)(minValue + (rand % diff));
                    }
                }
            }

            return random;
        }



        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0. 
        /// </summary>
        /// <remarks>
        /// Do we want it so that each bit pattern has equal probability of being generated, 
        /// or do we want it such that if we were to generate many random doubles they would 
        /// be evenly spread over the given interval? Given the nature of the real number 
        /// implementation in the .NET Framework, those are not the same thing. Most likely 
        /// we mean the second definition, which means that we don't want to be generating 
        /// completely random bits (which would skew the overall range of numbers toward the 
        /// lower end of the range). Instead, we want to generate numbers that are evenly spread
        /// out across the desired range.
        /// </remarks>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        public override double NextDouble()
        {
            double random;

            lock (_lock)
            {
                // One standard solution for generating a value in the range [0.0, 1.0) is to generate 
                // a non-negative integer and then divide that value by one more than the maximum
                // possible integral value.
                _rng.GetBytes(_uint32Buffer);

                UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);
                random = rand / (1.0 + UInt32.MaxValue);
                // If you want to return a double with completely random bits, use this line instead of the
                // preceeding two lines. Results from this method won't be evenly distributed across the range.
                //random = BitConverter.ToDouble(_uint32Buffer, 0);
            }

            return random;
        }

        /// <summary>
        /// Returns a non-negative random double that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
        public double NextDouble(double maxValue)
        {
            if (maxValue < 0)
                throw new ArgumentOutOfRangeException("maxValue");

            // Delegate to overload that takes a minValue and a maxValue useing 0 as minValue
            return NextDouble(0, maxValue);
        }

        /// <summary>
        /// Returns a non-negative random double that is less than the specified maximum.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned.
        /// maxValue must be greater than or equal to minValue.</param>
        /// <returns>A double-precision floating point number greater than or equal to minValue and less than maxValue; that is, the range of return values includes 
        /// minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
        public double NextDouble(double minValue, double maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException("minValue");

            if (minValue == maxValue)
                return minValue;

            return minValue + NextDouble() * (maxValue - minValue);
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        public override void NextBytes(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            lock (_rng)
            {
                _rng.GetBytes(buffer);
            }
        }
    }
}
