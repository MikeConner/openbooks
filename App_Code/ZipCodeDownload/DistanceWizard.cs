//-----------------------------------------------------------------------
// <copyright file="DistanceWizard.cs" company="ZIP Code Download, LLC">
//     Copyright (c) ZIP Code Download, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OpenBookAllegheny.ZipCodeDownload.Wizards
{
    using System;

    /// <summary>
    /// Computes the distance between any two coordinates on a sphere.
    /// </summary>
    public sealed class DistanceWizard
    {
        /// <summary>
        /// The distance, in miles, from the center of the earth to sea level.
        /// </summary>
        private const double EarthRadiusMiles = 3963.189;

        /// <summary>
        /// The service used to provide unit conversion functionality.
        /// </summary>
        private readonly UnitConverter unitConverter = new UnitConverter();

        /// <summary>
        /// Computes the distance between the two coordinates provided.
        /// </summary>
        /// <param name="origin">The point or origin from which to compute the distance.</param>
        /// <param name="relative">The coordinate outlying or relative to the point of origin.</param>
        /// <param name="measure">The unit of measure to be used when returning the distance, e.g. miles or kilometers.</param>
        /// <returns>The distance between the two coordinates expressed in the desired unit of measure.</returns>
        public double CalculateDistance(Coordinate origin, Coordinate relative, Measurement measure)
        {
            if (origin == null || relative == null || origin.Equals(relative))
            {
                // Invalid coordinate(s), no distance.
                return 0;
            }

            // Convert each coordinate from decimal degrees to radians in order to perform the geometric calculations.
            origin = origin.ToRadians();
            relative = relative.ToRadians();

            // Perform the actual distance calculation.
            double distance = Math.Sin(origin.Latitude) * Math.Sin(relative.Latitude);
            distance += Math.Cos(origin.Latitude)
                * Math.Cos(relative.Latitude)
                * Math.Cos(relative.Longitude - origin.Longitude);
            distance = -1 * Math.Atan(distance / Math.Sqrt(1 - distance * distance)) + Math.PI / 2;
            distance *= EarthRadiusMiles;

            if (Measurement.Kilometers == measure)
            {
                // Convert the distance calculated to utilize the desired unit of measure.
                return this.unitConverter.MilesToKilometers(distance);
            }

            return distance;
        }
    }
}