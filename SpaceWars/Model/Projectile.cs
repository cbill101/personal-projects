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
    /// Represents a projectile in the game world.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        // A global variable. Goes up when a new projectile is created to keep unique IDs for each projectile.
        private static int nextProjectileID;

        // The projectiles ID.
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        // The projectiles location.
        [JsonProperty]
        private Vector2D loc;

        // The projectiles directon.
        [JsonProperty]
        private Vector2D dir;

        // State that tells if the projectile is live or not. (Can damage ships, etc)
        [JsonProperty]
        private bool alive;

        // The projectile's owner, or the ships ID that fired the projectile.
        [JsonProperty]
        private int owner;

        /// <summary>
        /// A default Projectile object for JSON serialization.
        /// </summary>
        public Projectile()
        {
            ID = -1;
            loc = new Vector2D();
            dir = new Vector2D();
            alive = false;
            owner = -1;
        }

        /// <summary>
        /// Creates a Projectile object with a specified location, direction, and its owner.
        /// </summary>
        /// <param name="_loc"></param>
        /// <param name="_dir"></param>
        /// <param name="_owner"></param>
        public Projectile(Vector2D _loc, Vector2D _dir, int _owner)
        {
            ID = nextProjectileID++;
            loc = _loc;
            dir = _dir;
            alive = true;
            owner = _owner;
        }

        /// <summary>
        /// Gets the projectile's ID.
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Gets the projectile's location.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Gets the projectile's direction.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
            return dir;
        }

        /// <summary>
        /// Gets the live state of the projectile.
        /// </summary>
        /// <returns></returns>
        public bool IsAlive()
        {
            return alive;
        }

        /// <summary>
        /// Gets the projectile's owner. (Thr Ship ID)
        /// </summary>
        /// <returns></returns>
        public int GetOwner()
        {
            return owner;
        }

        /// <summary>
        /// Returns a string representing a Prjectile in JSON form.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Updates the projectile, making sure that it moves and also checks colisions with stars.
        /// </summary>
        /// <param name="Stars"></param>
        public void Update(IEnumerable<Star> Stars)
        {
            loc += dir * 15;
            foreach (Star star in Stars)
            {
                if (World.Collides(star.GetLocation(), loc, star.GetHitBoxSize()))
                {
                    alive = false;
                }
            }
        }

        /// <summary>
        /// Kills the projectile,  removing it from the world on the next frame.
        /// </summary>
        public void Kill()
        {
            alive = false;
        }
    }
}
