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
    /// A truct that represents commands from the client. The values in the struct determine how the ship reacts on update.
    /// </summary>
    public struct Commands
    {
        // Checks if the ship should be thrusting.
        private bool thrusting;
        // checks if the shi should be turning.
        private int turning;

        /// <summary>
        /// Returns a value, based on the turning request.
        /// </summary>
        public int RequestTurning
        {
            get
            {
                return turning;
            }
            set
            {
                turning = value;
            }
        }

        /// <summary>
        /// Returns a bool value based on the ship's thrust request.
        /// </summary>
        public bool RequestThrusting
        {
            get
            {
                return thrusting;
            }
            set
            {
                thrusting = value;
            }
        }

        /// <summary>
        /// Reset's the ship command requests. No turning, firing, or thrusting.
        /// </summary>
        public void Clear()
        {
            turning = 0;
            thrusting = false;
        }
    }
    /// <summary>
    /// Represents a JSON serializable ship for SpaceWars. Each ship has its own location, direction, ID, and username associated with it.
    /// Also has unique states like thrusting, and checking if the ship is alive.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        // A global variable. This increases by 1 every time a new ship is created.
        private static int nextShipID;
        // Keeps track of whether the ships has an active socket associated with it or not.
        private bool active;
        // The thrust vector.
        private Vector2D accelerationVector;
        // The velocity vector for the ship.
        private Vector2D velocity;
        // How fast the ship fires projectiles, (Default: Can fire after every 6 frames)
        private uint fireRate = 6U;
        // Keeps track of when the last fired projectile was.
        private uint lastFired = 0U;
        // Keeps track of when the last death was, for respawning reasons.
        private uint lastDeath = 0U;
        // The amount of thrust strength. The higher this is, the faster the ship goes on thrust.
        private double thrustStrength;
        // How fast the ship turns.
        private double turningRate;
        // The size of the hit box in pixels.
        private uint hitBoxSize;
        // The starting health points for the ship
        private int startingHP;

        // The ship's unique ID.
        [JsonProperty(PropertyName = "ship")]
        private int ID;

        // The ship's location.
        [JsonProperty]
        private Vector2D loc;

        // The ship's direction.
        [JsonProperty]
        private Vector2D dir;

        // The thrusting state.
        [JsonProperty]
        private bool thrust;

        // The player's username.
        [JsonProperty]
        private string name;

        // The ship's health.
        [JsonProperty]
        private int hp;

        // The ship's number of kills.
        [JsonProperty]
        private int score;

        private Commands commands;

        /// <summary>
        /// Gets a value indicating whether the ship is currently alive.
        /// </summary>
        public bool Alive
        {
            get
            {
                return hp > 0;
            }
        }

        /// <summary>
        /// A default Ship for JSON serialization.
        /// </summary>
        public Ship()
        {
            ID = -1;
            loc = new Vector2D();
            dir = new Vector2D();
            velocity = new Vector2D();
            accelerationVector = new Vector2D();
            thrust = false;
            name = "";
            hp = 0;
            score = 0;
            active = false;
            commands.Clear();
        }

        /// <summary>
        /// Creates a ship with the specified position, direction, and username.
        /// </summary>
        /// <param name="_loc"></param>
        /// <param name="_dir"></param>
        /// <param name="_name"></param>
        public Ship(Vector2D _loc, Vector2D _dir, string _name, uint _fireRate, double _thrustStrength, double _turnRate, int _startingHP, uint _hitBoxSize, int Score1, int Score2, bool teams)
        {
            ID = nextShipID++;
            loc = new Vector2D(_loc);
            dir = new Vector2D(_dir);
            velocity = new Vector2D(0, 0);
            accelerationVector = new Vector2D(0, 0);
            thrust = false;
            startingHP = _startingHP;
            hp = _startingHP;
            if (teams)
            {
                if (ID % 2 > 0)
                {
                    score = Score2;
                    name = "Team 1; " + _name;
                }
                else
                {
                    score = Score1;
                    name = "Team 2; " + _name;
                    nextShipID = nextShipID + 6;
                }
            }
            else
            {
                score = 0;
                name = _name;
               
            }
            active = true;
            fireRate = _fireRate;
            turningRate = _turnRate;
            hitBoxSize = _hitBoxSize;
            thrustStrength = _thrustStrength;
            commands.Clear();
        }

        /// <summary>
        /// Gets the team that the ship is assigned to.
        /// Returns 0 if Team 1, 1 if Team 2.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int GetTeam()
        {
            return ID % 2;
        }

        /// <summary>
        /// Makes the ship inactive, no connection is associated with it anymore.
        /// </summary>
        public void MakeInactive()
        {
            active = false;
        }

        /// <summary>
        /// Gets whether the ship is active (has an active connection associated with it) or not.
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return active;
        }

        /// <summary>
        /// Gets the ship's ID.
        /// </summary>
        /// <returns></returns>
        public int GetID()
        {
            return ID;
        }

        /// <summary>
        /// Sets the ship's ID.
        /// </summary>
        /// <param name="id"></param>
        public void SetID(int id)
        {
            ID = id;
        }

        /// <summary>
        /// Gets the ship';s location in the world.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetLocation()
        {
            return loc;
        }

        /// <summary>
        /// Gets the ship's direction that it is facing.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetDirection()
        {
            return dir;
        }

        /// <summary>
        /// Returns a value indicating if the ship is thrusting or not.
        /// </summary>
        /// <returns></returns>
        public bool IsThrusting()
        {
            return thrust;
        }

        /// <summary>
        /// Gets the ship's associated username.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Gets the ship's health points.
        /// </summary>
        /// <returns></returns>
        public int GetHP()
        {
            return hp;
        }

        /// <summary>
        /// Gets the ship's score, or amount of kills.
        /// </summary>
        /// <returns></returns>
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Gets the time (in frames) when this ship's last death occured.
        /// </summary>
        /// <returns></returns>
        public uint GetLastDeath()
        {
            return lastDeath;
        }

        /// <summary>
        /// Gets the ship's hit box size.
        /// </summary>
        /// <returns></returns>
        public uint GetHitBoxSize()
        {
            return hitBoxSize;
        }

        /// <summary>
        /// Puts the ship at full health and places it at the specified location.
        /// </summary>
        /// <param name="newLocation"></param>
        public void Respawn(Vector2D newLocation)
        {
            hp = startingHP;
            velocity = new Vector2D(0, 0);
            accelerationVector = new Vector2D(0, 0);

            loc = new Vector2D(newLocation);
            dir = new Vector2D(-1, 0);
        }

        /// <summary>
        /// If the ship goes outside the world borders horizontally, this makes sure the ships stays in bounds, wrapping around to the other side.
        /// </summary>
        public void WrapAroundX()
        {
            loc = new Vector2D(loc.GetX() * -1, loc.GetY());
        }

        /// <summary>
        /// If the ship goes outside the world borders vertically, this makes sure the ships stays in bounds, wrapping around to the other side.
        /// </summary>
        public void WrapAroundY()
        {
            loc = new Vector2D(loc.GetX(), loc.GetY() * -1);
        }

        /// <summary>
        /// Fires a projectile, if the ship is alive and the fire delay has been satisfied. True is both conditions meet, false otherwise.
        /// Also records when this projectile was shot within the ship itself.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="shot"></param>
        /// <returns></returns>
        public bool Fire(uint time, out Projectile shot)
        {
            shot = null;
            if (!this.Alive || ((time - lastFired) < fireRate))
            {
                return false;
            }
            lastFired = time;

            Vector2D shotdir = new Vector2D(GetDirection());
            shot = new Projectile(GetLocation(), shotdir, GetID());
            
            return true;
            //If not alive or shot exceeds fire rate, return false. Otherwise, make a new projectile and return true.
        }

        /// <summary>
        /// Processes the command character passed in, acts accordingly.
        /// </summary>
        /// <param name="ch"></param>
        public void ProcessCommand(char ch)
        {
            switch(ch)
            {
                case 'L':
                    commands.RequestTurning = -1;
                    break;
                case 'R':
                    commands.RequestTurning = 1;
                    break;
                case 'T':
                    commands.RequestThrusting = true;
                    break;
            }
        }

        /// <summary>
        /// Applies existing ship commands to turn, thrust, or fire.
        /// </summary>
        public void ApplyCommands()
        {
            if(commands.RequestTurning == 1)
            {
                dir.Rotate(turningRate);
            }
            else if(commands.RequestTurning == -1)
            {
                dir.Rotate(-turningRate);
            }

            if (commands.RequestThrusting == true)
            {
                thrust = true;
                accelerationVector = new Vector2D(dir * thrustStrength);
            }
            else
                thrust = false;

            commands.Clear();
        }

        /// <summary>
        /// Increases the ship's score by 1.
        /// </summary>
        public void IncreaseScore()
        {
            score += 1;        
        }

        /// <summary>
        /// Converts the ship object to a string, represented in JSON.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Updates the ship by applying any client commands and calculating velocity and forces acting on the ship.
        /// </summary>
        /// <param name="stars"></param>
        /// <param name="time"></param>
        public void Update(IEnumerable<Star> stars, uint time)
        {
            ApplyCommands();
            GetPhysics(stars, time);
        }

        /// <summary>
        /// Calculates the thrust and gravity forces, adding to the velocity of the ship.
        /// </summary>
        /// <param name="stars"></param>
        /// <param name="time"></param>
        public void GetPhysics(IEnumerable<Star> stars, uint time)
        {
            Vector2D forces = new Vector2D(accelerationVector);
            accelerationVector = new Vector2D(0, 0);

            foreach(Star star in stars)
            {
                Vector2D g = star.GetLocation() - loc;
                if(World.Collides(star.GetLocation(), loc, star.GetHitBoxSize()))
                {
                    if(Alive)
                    {
                        Die(time);
                        return;
                    }
                }
                g.Normalize();
                g = g * star.GetMass();
                forces += g;
            }

            velocity += forces;
            loc += velocity;          
        }

        /// <summary>
        /// Decreases the ship's HP by 1. If the ship's HP is 0, the passed in time will be recorded as the ship's last death.
        /// </summary>
        /// <param name="time"></param>
        public void Hit(uint time)
        {
            hp -= 1;
            if(hp == 0)
            {
                Die(time);
            }
        }

        /// <summary>
        /// Records the ship's last death, and makes the HP 0.
        /// </summary>
        /// <param name="time"></param>
        public void Die(uint time)
        {
            hp = 0;
            lastDeath = time;
        }

    }
}
