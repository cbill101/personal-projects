using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Represents a JSON serializable star in SpaceWars. Each Star has its own location, ID, and mass.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        // With each new Star, the value here is assigned as the ID, and after, increases by 1 for the next Star.
        private static int nextStarID;

        // The ship's ID.
        [JsonProperty(PropertyName = "star")]
        private int ID;

        // The ship's location.
        [JsonProperty]
        private Vector2D loc;

        // The ship's mass.
        [JsonProperty]
        private double mass;

        private uint hitBoxSize;

        /// <summary>
        /// A default star for JSON serialization.
        /// </summary>
        public Star()
        {
            ID = -1;
            loc = new Vector2D();
            mass = 0.0;
            hitBoxSize = 35;
        }

        /// <summary>
        /// Creates a Star with the speficied location vector and mass.
        /// </summary>
        /// <param name="_loc"></param>
        /// <param name="_mass"></param>
        public Star(Vector2D _loc, double _mass, uint _hitBoxSize)
        {
            ID = nextStarID++;
            loc = _loc;
            mass = _mass;
            hitBoxSize = _hitBoxSize;
        }

        /// <summary>
        /// Gets the star's ID.
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Gets the star's location.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Gets the star's mass.
        /// </summary>
        /// <returns></returns>
        public double GetMass()
        {
            return mass;
        }

        public uint GetHitBoxSize()
        {
            return hitBoxSize;
        }

        /// <summary>
        /// Converts the star object to a string, represented in JSON.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
