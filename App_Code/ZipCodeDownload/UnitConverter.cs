//-----------------------------------------------------------------------
// <copyright file="UnitConverter.cs" company="ZIP Code Download, LLC">
//     Copyright (c) ZIP Code Download, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OpenBookPgh.ZipCodeDownload.Wizards
{
    using System;

    /// <summary>
    /// A service used to facilitate unit conversion.
    /// </summary>
    internal sealed class UnitConverter
    {
        /// <summary>
        /// The number of kilometers in one mile.
        /// </summary>
        private const double KilometersPerMile = 1.609344;

        /// <summary>
        /// The number of degrees contained in a half circle.
        /// </summary>
        private const double SemiCircleDegrees = 180.0;

        /// <summary>
        /// Converts radians to its corresponding value in degrees.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The number of degrees in the value provided.</returns>
        public double RadiansToDegrees(double value)
        {
            return value * SemiCircleDegrees / Math.PI;
        }

        /// <summary>
        /// Converts degrees to its corresponding value in radians.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The number of radians in the value provided.</returns>
        public double DegreesToRadians(double value)
        {
            return value * Math.PI / SemiCircleDegrees;
        }

        /// <summary>
        /// Converts miles to its corresponding value in kilometers.
        /// </summary>
        /// <param name="value">The number of miles to be converted.</param>
        /// <returns>The number of kilometers in the value provided.</returns>
        public double MilesToKilometers(double value)
        {
            return value * KilometersPerMile;
        }

        /// <summary>
        /// Converts kilometers to its corresponding value in miles.
        /// </summary>
        /// <param name="value">The number of kilometers to be converted.</param>
        /// <returns>The number of miles in the value provided.</returns>
        public double KilometersToMiles(double value)
        {
            return value / KilometersPerMile;
        }
    }
}