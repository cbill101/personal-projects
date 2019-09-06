using SpaceWars;
using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents the entire game world.
    /// </summary>
    public class World
    {
        // The default world size.
        private const int DEFAULT_SIZE = 750;
        // Total amount of frames sent.
        private uint time;
        // The world size.
        private int size = DEFAULT_SIZE;
        // Respawn after this many frames.
        private uint respawnDelay = 300U;
        // After this many frames, a shot can be fired again.
        private uint shotFireDelay = 6U;
        // A instance of Random, for use to make respawn locations.
        private Random rnd;
        // The players
        private Dictionary<int, Ship> players;
        // The stars
        private Dictionary<int, Star> stars;
        // The projectiles
        private Dictionary<int, Projectile> shots;
        //if team mode
        private bool teams;

        /// <summary>
        /// A default World object, creating the basic collections.
        /// </summary>
        public World()
        {
            players = new Dictionary<int, Ship>();
            stars = new Dictionary<int, Star>();
            shots = new Dictionary<int, Projectile>();
            time = 0U;
            rnd = new Random();
        }

        /// <summary>
        /// Creates a World object with more specifics. Assigns size, stars to add, shot delay, respawn delay, and team mode values.
        /// </summary>
        /// <param name="newSize"></param>
        /// <param name="starList"></param>
        /// <param name="sfd"></param>
        /// <param name="rd"></param>
        /// <param name="team"></param>
        public World(int newSize, IEnumerable<Star> starList, uint sfd, uint rd, bool team) : this()
        {
            teams = team;
            size = newSize;
            shotFireDelay = sfd;
            respawnDelay = rd;
            foreach (Star star in starList)
            {
                stars[star.GetID()] = star;
            }
        }

        /// <summary>
        /// Updates Team 1's score, all ships on the team will have their score increased by 1.
        /// </summary>
        public void UpdateScoreTeam1()
        {
            foreach (Ship ship in players.Values)
            {
                if (ship.GetID() % 2 == 0)
                    ship.IncreaseScore();
            }
        }

        /// <summary>
        /// Updates every ship's score in Team 2.
        /// </summary>
        public void UpdateScoreTeam2()
        {
            foreach (Ship ship in players.Values)
            {
                if (ship.GetID() % 2 == 1)
                    ship.IncreaseScore();
            }
        }

        /// <summary>
        /// Gets the world size.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            return size;
        }

        /// <summary>
        /// Gets the respawn delay, or how many frames the world takes before a ship can respawn.
        /// </summary>
        /// <returns></returns>
        public uint GetRespawnDelay()
        {
            return respawnDelay;
        }

        /// <summary>
        /// Gets the shot delay, or how many frames the world takes to have a ship be abe to fire a projectile again.
        /// </summary>
        /// <returns></returns>
        public uint GetShotFireDelay()
        {
            return shotFireDelay;
        }

        /// <summary>
        /// Sets the world size.
        /// </summary>
        /// <param name="_size"></param>
        public void SetSize(int _size)
        {
            size = _size;
        }

        /// <summary>
        /// Returns a dictionary of the world's ships.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Ship> GetShips()
        {
            return players;
        }

        /// <summary>
        /// Returns a dictionary of the world's stars.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Star> GetStars()
        {
            return stars;
        }

        /// <summary>
        /// Returns a dictionary of the world's projectiles.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Projectile> GetProjectiles()
        {
            return shots;
        }

        /// <summary>
        /// Proceses the ship's command, using the passed in ID and command character.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ch"></param>
        public void ProcessCommand(int ID, char ch)
        {
            Projectile shot;
            if (ch == 'F' && players[ID].Fire(time, out shot))
            {
                shots.Add(shot.GetID(), shot);
            }

            players[ID].ProcessCommand(ch);
        }

        /// <summary>
        /// Adds a ship to the world. 
        /// </summary>
        /// <param name="_loc"></param>
        /// <param name="_dir"></param>
        /// <param name="_name"></param>
        /// <param name="_fireRate"></param>
        /// <param name="_thrustStrength"></param>
        /// <param name="_turnRate"></param>
        /// <param name="_startingHP"></param>
        /// <param name="_hitBoxSize"></param>
        /// <returns></returns>
        public Ship AddShip(Vector2D _loc, Vector2D _dir, string _name, uint _fireRate, double _thrustStrength, double _turnRate, int _startingHP, uint _hitBoxSize)
        {
            Ship ship;
            if (teams)
            {
                ship = new Ship(_loc, _dir, _name, _fireRate, _thrustStrength, _turnRate, _startingHP, _hitBoxSize, 0, 0, true);
            }
            else
            {
                ship = new Ship(_loc, _dir, _name, _fireRate, _thrustStrength, _turnRate, _startingHP, _hitBoxSize, 0, 0, false);
            }

            players.Add(ship.GetID(), ship);
            return ship;
        }

        /// <summary>
        /// Adds a ship to the world, at a random location.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fireRate"></param>
        /// <param name="thrustStrength"></param>
        /// <param name="turnRate"></param>
        /// <param name="startingHP"></param>
        /// <param name="hitBoxSize"></param>
        /// <returns></returns>
        public Ship AddShipRandomPosition(string name, uint fireRate, double thrustStrength, double turnRate, int startingHP, uint hitBoxSize)
        {
            Vector2D position = GetRandomPosition();
            Vector2D location = new Vector2D(-1, 0);

            return AddShip(position, location, name, fireRate, thrustStrength, turnRate, startingHP, hitBoxSize);
        }

        /// <summary>
        /// Adds a star to the world, with a specific location, mass, and hit box size.
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="mass"></param>
        /// <param name="hitBoxSize"></param>
        public void AddStar(Vector2D loc, double mass, uint hitBoxSize)
        {
            Star star = new Star(loc, mass, hitBoxSize);
            stars.Add(star.GetID(), star);
        }

        /// <summary>
        /// Returns a validated random position vector, for respawning or other uses.
        /// </summary>
        /// <returns></returns>
        public Vector2D GetRandomPosition()
        {
            bool validSpawn;
            Vector2D pos;

            do
            {
                validSpawn = true;
                double x = rnd.Next(-size / 2, size / 2);
                double y = rnd.Next(-size / 2, size / 2);
                pos = new Vector2D(x, y);

                foreach (Star star in stars.Values)
                {
                    if ((star.GetLocation() - pos).Length() < star.GetHitBoxSize())
                    {
                        validSpawn = false;
                        break;
                    }
                }
            }
            while (!validSpawn);

            return pos;
        }

        /// <summary>
        /// The overall update function for the entire world. Updates ships and projectiles, and manages activity based on location.
        /// </summary>
        public void Update()
        {
            foreach (Ship ship in players.Values)
            {
                if (!ship.Alive && time - ship.GetLastDeath() > respawnDelay)
                {
                    ship.Respawn(GetRandomPosition());
                }

                ship.Update(stars.Values, time);

                if (ship.GetLocation().GetX() > size / 2 || ship.GetLocation().GetX() < -(size / 2))
                {
                    ship.WrapAroundX();
                }
                if (ship.GetLocation().GetY() > size / 2 || ship.GetLocation().GetY() < -(size / 2))
                {
                    ship.WrapAroundY();
                }
            }
            foreach (Projectile shot in shots.Values)
            {
                shot.Update(stars.Values);
                DetectHit(shot);

                if (shot.GetLocation().GetX() > size / 2 || shot.GetLocation().GetX() < -(size / 2))
                {
                    shot.Kill();
                }
                if (shot.GetLocation().GetY() > size / 2 || shot.GetLocation().GetY() < -(size / 2))
                {
                    shot.Kill();
                }
            }

            time += 1U;
        }

        /// <summary>
        /// Cleans all the inactive objects from the world, removes them from the dictionaries.
        /// </summary>
        public void Cleanup()
        {
            foreach (Ship ship in new List<Ship>(players.Values))
            {
                if (!ship.IsActive())
                {
                    players.Remove(ship.GetID());
                }
            }
            foreach (Projectile shot in new List<Projectile>(shots.Values))
            {
                if (!shot.IsAlive())
                {
                    shots.Remove(shot.GetID());
                }
            }
        }

        /// <summary>
        /// Detects collisions between ships and projectiles, and acts on that based on team mode, self shots, etc.
        /// </summary>
        /// <param name="shot"></param>
        public void DetectHit(Projectile shot)
        {
            foreach (Ship ship in players.Values)
            {
                Vector2D distance = ship.GetLocation() - shot.GetLocation();
                if (ship.Alive && distance.Length() < ship.GetHitBoxSize() && shot.GetOwner() != ship.GetID())
                {
                    if (teams && (shot.GetOwner() % 2) == (ship.GetID() % 2))
                    {
                        shot.Kill();
                    }
                    else
                    {
                        shot.Kill();
                        ship.Hit(time);
                        if (!ship.Alive)
                        {
                            if (players.ContainsKey(shot.GetOwner()))
                            {
                                if (teams)
                                {
                                    if (players[shot.GetOwner()].GetID() % 2 == 0)
                                    {
                                        UpdateScoreTeam1();
                                    }
                                    else
                                        UpdateScoreTeam2();
                                    break;
                                }
                                else
                                {
                                    players[shot.GetOwner()].IncreaseScore();
                                    break;
                                }
                            }
                            break;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Helper function that detects a collision between two objects. The hit box size is for the first object and the objLocation vector.
        /// </summary>
        /// <param name="objLocation"></param>
        /// <param name="otherObjLocation"></param>
        /// <param name="hitBoxSize"></param>
        /// <returns></returns>
        public static bool Collides(Vector2D objLocation, Vector2D otherObjLocation, uint hitBoxSize)
        {
            return (objLocation - otherObjLocation).Length() < hitBoxSize;
        }
    }
}