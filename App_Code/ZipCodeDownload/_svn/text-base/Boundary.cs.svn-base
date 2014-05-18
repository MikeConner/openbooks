//-----------------------------------------------------------------------
// <copyright file="Boundary.cs" company="ZIP Code Download, LLC">
//     Copyright (c) ZIP Code Download, LLC. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OpenBookPgh.ZipCodeDownload.Wizards
{
    /// <summary>
    /// Holds four points which represent the northern, southern, eastern and western boundaries from a point of origin.
    /// </summary>
    public sealed class Boundary
    {
        /// <summary>
        /// The northern-most boundary or line of latitude from a point of origin.
        /// </summary>
        private double north;

        /// <summary>
        /// The southern-most boundary or line of latitude from a point of origin.
        /// </summary>
        private double south;

        /// <summary>
        /// The eastern-most boundary or line of longitude from a point of origin.
        /// </summary>
        private double east;

        /// <summary>
        /// The western-most boundary or line of longitude from a point of origin.
        /// </summary>
        private double west;

        /// <summary>
        /// Initializes a new instance of the <see cref="Boundary"/> class.
        /// </summary>
        /// <param name="north">The northern-most boundary or line of latitude from a point of origin.</param>
        /// <param name="south">The southern-most boundary or line of latitude from a point of origin.</param>
        /// <param name="east">The eastern-most boundary or line of longitude from a point of origin.</param>
        /// <param name="west">The western-most boundary or line of longitude from a point of origin.</param>
        public Boundary(double north, double south, double east, double west)
        {
            this.north = north;
            this.south = south;
            this.east = east;
            this.west = west;
        }

        /// <summary>
        /// Gets the northern-most boundary line of latitude from a point of origin.
        /// </summary>
        public double North
        {
            get { return this.north; }
        }

        /// <summary>
        /// Gets the southern-most boundary line of latitude from a point of origin.
        /// </summary>
        public double South
        {
            get { return this.south; }
        }

        /// <summary>
        /// Gets the eastern-most boundary line of longitude from a point of origin.
        /// </summary>
        public double East
        {
            get { return this.east; }
        }

        /// <summary>
        /// Gets the western-most boundary line of longitude from a point of origin.
        /// </summary>
        public double West
        {
            get { return this.west; }
        }
    }
}