//-----------------------------------------------------------------------
// <copyright file="Coordinate.cs" company="ZIP Code Download, LLC">
//     Copyright (c) ZIP Code Download, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OpenBookAllegheny.ZipCodeDownload.Wizards
{
    using System;

    /// <summary>
    /// Represents a particular location on the planet using the decimal degree coordinate system.
    /// </summary>
    public sealed class Coordinate
    {
        /// <summary>
        /// The minimum possible value for decimal degrees of a half circle.
        /// </summary>
        private const double MinDegrees = -180.0;

        /// <summary>
        /// The maximum possible value for decimal degrees of a half circle.
        /// </summary>
        private const double MaxDegrees = 180.0;

        /// <summary>
        /// The service used to provide unit conversion functionality.
        /// </summary>
        private readonly UnitConverter unitConverter = new UnitConverter();

        /// <summary>
        /// The line north (+) or south (-) of the equator in decimal degrees.
        /// </summary>
        private readonly double latitude;

        /// <summary>
        /// The line east (+) or west (-) of the prime meridian in decimal degrees.
        /// </summary>
        private readonly double longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="Coordinate"/> class.
        /// </summary>
        /// <param name="latitude">The line north (+) or south (-) of the equator in decimal degrees.</param>
        /// <param name="longitude">The line east (+) or west (-) of the prime meridian in decimal degrees.</param>
        public Coordinate(double latitude, double longitude)
        {
            if (latitude > MaxDegrees || latitude < MinDegrees)
            {
                throw new ArgumentException("The value provided is out of bounds.", "latitude");
            }
            else if (longitude > MaxDegrees || longitude < MinDegrees)
            {
                throw new ArgumentException("The value provided is out of bounds.", "longitude");
            }

            this.latitude = latitude;
            this.longitude = longitude;
        }

        /// <summary>
        /// Gets the line north (+) or south (-) of the equator in decimal degrees.
        /// </summary>
        public double Latitude
        {
            get { return this.latitude; }
        }

        /// <summary>
        /// Gets the line east (+) or west (-) of the prime meridian in decimal degrees.
        /// </summary>
        public double Longitude
        {
            get { return this.longitude; }
        }

        /// <summary>
        /// Gets a value which indicates if the coordinate provided is identical in value to the current object.
        /// </summary>
        /// <param name="obj">The obj to be compared to the this coordinate.</param>
        /// <returns>A value which indicates if the coordinate provided is identical in value to the current object</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                // Having the same memory address or reference means it's the exact same object.
                return true;
            }

            Coordinate coordinate = obj as Coordinate;
            // Compare the values of the coordinate object to determine if it's identical in value.
            return null != coordinate
                && this.latitude == coordinate.Latitude
                && this.longitude == coordinate.Longitude;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>/// A hash code for the current <see cref="T:System.Object"/>./// </returns>
        public override int GetHashCode()
        {
            // TODO
            return base.GetHashCode();
        }

        /// <summary>
        /// Expresses the coordinate in radians instead of decimal degrees.
        /// </summary>
        /// <returns>The coordinate expressed in radians.</returns>
        internal Coordinate ToRadians()
        {
            return new Coordinate(
                this.unitConverter.DegreesToRadians(this.latitude),
                this.unitConverter.DegreesToRadians(this.longitude));
        }
    }
}