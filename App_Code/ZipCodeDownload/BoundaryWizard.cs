//-----------------------------------------------------------------------
// <copyright file="BoundaryWizard.cs" company="ZIP Code Download, LLC">
//     Copyright (c) ZIP Code Download, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OpenBookAllegheny.ZipCodeDownload.Wizards
{
    using System;

    /// <summary>
    /// Helps determine which coordinates fall outside a particular boundary by computing a loose range around a center coordinate.
    /// </summary>
    public sealed class BoundaryWizard
    {
        /// <summary>
        /// The distance, in miles, from the center of the earth to sea level.
        /// </summary>
        private const double EarthRadiusMiles = 3963.189;

        /// <summary>
        /// The distance, in miles, a single degree of longitude is at the earth's equator.
        /// </summary>
        private const double EquatorMilesPerLongitudeDegree = 69.172;

        /// <summary>
        /// The service used to provide unit conversion functionality.
        /// </summary>
        private readonly UnitConverter unitConverter = new UnitConverter();

        /// <summary>
        /// Finds the boundary around a point of origin.
        /// </summary>
        /// <param name="origin">The specific coordinate or location around which the boundary should be computed.</param>
        /// <param name="distance">The distance, in the specified unit of measure, around which the boundary will be computed.</param>
        /// <param name="measure">The unit of measure for distance provided.</param>
        /// <returns>The boundary around the point of origin.</returns>
        public Boundary CalculateBoundary(Coordinate origin, double distance, Measurement measure)
        {
            if (origin == null)
            {
                // rather than throw an exception, just return a blank/empty radius.
                return new Boundary(0, 0, 0, 0);
            }
            else if (distance < 0)
            {
                // Negative distances are impossible, invert the value.
                distance *= -1.0;
            }

            if (measure == Measurement.Kilometers)
            {
                // Convert the distance to miles internally.  Note the values computed will be
                // the same regardless of the unit of measure used.  This is similar to
                // measuring the temperature outside - the actual temperature doesn't change
                // just because it's measured in Celsius or Fahrenheit.
                distance = this.unitConverter.KilometersToMiles(distance);
            }

            // Convert the origin to radians in order to perform the geometric computations.
            Coordinate originAsRadians = origin.ToRadians();

            // Compute the southern and northern boundaries
            double north = origin.Latitude + distance / EquatorMilesPerLongitudeDegree;
            double south = origin.Latitude - distance / EquatorMilesPerLongitudeDegree;

            // Compute the eastern and western boundaries
            double east = Math.Sin((-1 * distance / EarthRadiusMiles) + Math.PI / 2);
            east -= Math.Sin(originAsRadians.Latitude) * Math.Sin(originAsRadians.Latitude);
            east /= Math.Cos(originAsRadians.Latitude) * Math.Cos(originAsRadians.Latitude);
            east = Math.Acos(east) + originAsRadians.Longitude;
            east = this.unitConverter.RadiansToDegrees(east);

            double west = origin.Longitude - (east - origin.Longitude);

            return new Boundary(north, south, east, west);
        }
    }
}