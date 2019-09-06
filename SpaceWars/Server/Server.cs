using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using static Controller.NetworkController;
using Model;
using SpaceWars;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

namespace Server
{
    /// <summary>
    /// Represents in stance of game settings, controlling HP, thrust trength, various delays and much more.
    /// </summary>
    class SpaceWarsSettings
    {
        // The world size.
        private int size;
        // The frame delay. After this many milliseconds, a frame can be processed.
        private uint msPerFrame;
        // The shot fire rate. The lower this goes, the higher the fire rate is.
        private uint shotDelay;
        // How many frames it takes for a dead ship to respawn.
        private uint respawnDelay;
        // The starting amount of HP.
        private int startingHP;
        // Toggles team mode on or off.
        private bool teamMode;
        // The speed of the shot.
        private uint shotSpeed;
        // The thrusting strength for the ship.
        private double thrustStrength;
        // How fast the ship turns.
        private double turningRate;
        // The hit box size for the ships.
        private uint shipHitBoxSize;
        // The hit box size for the stars.
        private uint starHitBoxSize;
        // The list of stars to add to the world.
        private List<Star> stars;

        /// <summary>
        /// Creates a default instance of this class, with set default settings for the game.
        /// </summary>
        public SpaceWarsSettings()
        {
            size = 750;
            msPerFrame = 16U;
            shotDelay = 6U;
            respawnDelay = 300U;
            startingHP = 5;
            teamMode = false;
            shotSpeed = 15U;
            thrustStrength = 0.08;
            turningRate = 2.0;
            shipHitBoxSize = 20;
            starHitBoxSize = 35;
            stars = new List<Star>();
            stars.Add(new Star(new Vector2D(0, 0), 0.01, 35));
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
        /// Gets the MS per frame delay.
        /// </summary>
        /// <returns></returns>
        public uint GetMSPerFrame()
        {
            return msPerFrame;
        }

        /// <summary>
        /// Gets the shot delay.
        /// </summary>
        /// <returns></returns>
        public uint GetShotDelay()
        {
            return shotDelay;
        }

        /// <summary>
        /// Gets the respawn delay.
        /// </summary>
        /// <returns></returns>
        public uint GetRespawnDelay()
        {
            return respawnDelay;
        }

        /// <summary>
        /// Gets the starting HP.
        /// </summary>
        /// <returns></returns>
        public int GetStartingHP()
        {
            return startingHP;
        }

        /// <summary>
        /// Gets the team mode state. True is team mode is enabled, false if not.
        /// </summary>
        /// <returns></returns>
        public bool TeamModeEnabled()
        {
            return teamMode;
        }

        /// <summary>
        /// Gets the shot speed.
        /// </summary>
        /// <returns></returns>
        public uint GetShotSpeed()
        {
            return shotSpeed;
        }

        /// <summary>
        /// Gets the thrust strength.
        /// </summary>
        /// <returns></returns>
        public double GetThrustStrength()
        {
            return thrustStrength;
        }

        /// <summary>
        /// Gets the turning rate.
        /// </summary>
        /// <returns></returns>
        public double GetTurningRate()
        {
            return turningRate;
        }

        /// <summary>
        /// Gets the ship hit box size.
        /// </summary>
        /// <returns></returns>
        public uint GetShipHitBoxSize()
        {
            return shipHitBoxSize;
        }

        /// <summary>
        /// Gets the star hit box size.
        /// </summary>
        /// <returns></returns>
        public uint GetStarHitBoxSize()
        {
            return starHitBoxSize;
        }

        /// <summary>
        /// Gets the list of stars to be added to the world.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Star> GetStars()
        {
            return new List<Star>(stars);
        }

        /// <summary>
        /// Reads an entire Star element from an XML file, and adds it to the list of stars to add later.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="hitBoxSize"></param>
        /// <returns></returns>
        private static Star ReadStar(XmlReader reader, uint hitBoxSize)
        {
            Star star = null;

            int x = 0;
            int y = 0;
            double mass = 0.0;

            bool validX = false;
            bool validY = false;
            bool validMass = false;

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "x":
                            x = reader.ReadElementContentAsInt();
                            validX = true;
                            break;
                        case "y":
                            y = reader.ReadElementContentAsInt();
                            validY = true;
                            break;
                        case "mass":
                            mass = reader.ReadElementContentAsDouble();
                            validMass = true;
                            break;
                    }
                }
                if (validX && validY && validMass)
                {
                    star = new Star(new Vector2D(x, y), mass, hitBoxSize);
                    break;
                }
            }

            return star;
        }

        /// <summary>
        /// Returns a set of settings from an XML file. If categories in the XML aren't present, default values will be used in their place.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static SpaceWarsSettings ReadXML(string filePath)
        {
            SpaceWarsSettings settings = new SpaceWarsSettings();
            try
            {

                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    settings.size = reader.ReadElementContentAsInt();
                                    break;

                                case "MSPerFrame":
                                    settings.msPerFrame = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "FramesPerShot":
                                    settings.shotDelay = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "RespawnRate":
                                    settings.respawnDelay = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "StartingHP":
                                    settings.startingHP = reader.ReadElementContentAsInt();
                                    break;

                                case "Team":
                                    settings.teamMode = reader.ReadElementContentAsBoolean();
                                    break;

                                case "ShotSpeed":
                                    settings.shotSpeed = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "ThrustStrength":
                                    settings.thrustStrength = reader.ReadElementContentAsDouble();
                                    break;

                                case "TurningRate":
                                    settings.turningRate = reader.ReadElementContentAsDouble();
                                    break;

                                case "ShipHitBoxSize":
                                    settings.shipHitBoxSize = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "StarHitBoxSize":
                                    settings.starHitBoxSize = (uint)reader.ReadElementContentAsInt();
                                    break;

                                case "Star":
                                    Star star = ReadStar(reader, settings.starHitBoxSize);
                                    if (star != null)
                                        settings.stars.Add(star);
                                    break;
                            }
                        }
                    }
                }

                return settings;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error reading settings. Using stored defaults.");
                SpaceWarsSettings defaults = new SpaceWarsSettings();
                defaults.stars.Add(new Star(new Vector2D(0, 0), 0.01, defaults.starHitBoxSize));
                return defaults;
            }
        }
    }

    /// <summary>
    /// Represents the server for the SpaceWars game.
    /// </summary>
    class Server
    {
        // The list of clients connected to the server.
        private static LinkedList<SocketState> clients;
        // The game/server settings.
        private static SpaceWarsSettings settings;
        // The game world.
        private static World theWorld;
        // The stopwatch that manages frame timings.
        private static Stopwatch watch;
        // A Thread that runs the game loop. (Infinite)
        private static Thread gameLoop;
        // The data to be sent to the clients.
        private static StringBuilder gameData;

        /// <summary>
        /// Starts the server, including a client connection loop and the game loop.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            settings = SpaceWarsSettings.ReadXML("settings.xml");
            clients = new LinkedList<SocketState>();
            watch = new Stopwatch();
            gameData = new StringBuilder();

            theWorld = new World(settings.GetSize(), settings.GetStars(), settings.GetShotDelay(), settings.GetRespawnDelay(), settings.TeamModeEnabled());

            Networking.ServerAwaitingClientLoop(HandleNewClient);

            watch.Start();

            gameLoop = new Thread(() =>
            {
                while (true)
                {
                    Update();
                }
            });

            gameLoop.Start();

        }

        /// <summary>
        /// Handler for when a client established a connection with the server.
        /// </summary>
        /// <param name="state"></param>
        private static void HandleNewClient(SocketState state)
        {
            state.ClientCallback = ReceivePlayerName;
            Networking.GetData(state);
        }

        /// <summary>
        /// Gets initial data from the client, more specifically the player name. Sends initial game info to the client.
        /// </summary>
        /// <param name="state"></param>
        private static void ReceivePlayerName(SocketState state)
        {
            try
            {
                // This block splits up the server data within the StringBuilder, and places it into initialInfo.
                StringBuilder sb = state.sb;
                char[] separator = new char[] { '\n' };
                string[] initialInfo = sb.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                // Assigns the ID sent by the server to the socket state for uniqie identification of connections.
                state.sb.Remove(0, initialInfo[0].Length + 1);
                string name = initialInfo[0];

                Ship newShip;
                lock (theWorld)
                {
                    newShip = theWorld.AddShipRandomPosition(name, settings.GetShotDelay(), settings.GetThrustStrength(), settings.GetTurningRate(), settings.GetStartingHP(), settings.GetShipHitBoxSize());
                }

                state.ClientCallback = HandleClientRequest;
                state.ID = newShip.GetID();

                Networking.SendSocket(state.theSocket, "" + newShip.GetID() + "\n" + theWorld.GetSize() + "\n");

                lock (clients)
                {
                    clients.AddLast(state);
                }

                Networking.GetData(state);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION: " + e.Message);
            }
        }

        /// <summary>
        /// When a client sends an input request to the server, this method gets called. Processes the input, sends to the world and other objs for processing.
        /// Then, requests more data.
        /// </summary>
        /// <param name="state"></param>
        private static void HandleClientRequest(SocketState state)
        {
            string totalData = state.sb.ToString();

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            // Loop until we have processed all messages.
            // We may have received more than one.
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                // Handle stuff here!!
                char[] requests = p.ToCharArray();

                lock (theWorld)
                {
                    foreach (char request in requests)
                    {
                        theWorld.ProcessCommand(state.ID, request);
                    }
                }

                // Remove it from the SocketState's growable buffer
                state.sb.Remove(0, p.Length);
            }

            state.ClientCallback = HandleClientRequest;
            Networking.GetData(state);
        }

        /// <summary>
        /// The main function driving the server almost entirely. Updates the world, then gets each object's info, 
        /// adds it to the data to be sent to the clients, then sends it to all the clients. 
        /// Any disconnected clients are also removed/dealt with.
        /// </summary>
        private static void Update()
        {
            if (watch.IsRunning)
            {
                // Make thread wait until the frame delay is met.
                while (watch.ElapsedMilliseconds < settings.GetMSPerFrame())
                {
                    Thread.Yield();
                }

                watch.Restart();

                // Update the world, and sent it as one to all clients.
                lock (theWorld)
                {
                    theWorld.Update();
                    foreach (Ship ship in theWorld.GetShips().Values)
                    {
                        gameData.Append(ship.ToString() + "\n");
                    }
                    foreach (Projectile proj in theWorld.GetProjectiles().Values)
                    {
                        gameData.Append(proj.ToString() + "\n");
                    }
                    foreach (Star star in theWorld.GetStars().Values)
                    {
                        gameData.Append(star.ToString() + "\n");
                    }
                    theWorld.Cleanup();
                }

                // Iterate through the linked list of clients in order to remove any disconnected clients.
                lock (clients)
                {
                    LinkedListNode<SocketState> firstNode = clients.First;
                    while (firstNode != null)
                    {
                        SocketState socketState = firstNode.Value;
                        Networking.SendSocket(socketState.theSocket, gameData.ToString());
                        if (!socketState.theSocket.Connected)
                        {
                            lock (theWorld)
                            {
                                if (theWorld.GetShips().ContainsKey(socketState.ID))
                                {
                                    theWorld.GetShips()[socketState.ID].MakeInactive();
                                }
                            }
                            LinkedListNode<SocketState> nextNode = firstNode.Next;
                            clients.Remove(firstNode);
                            firstNode = nextNode;
                        }
                        else
                        {
                            firstNode = firstNode.Next;
                        }
                    }
                }

                gameData.Clear();
            }
        }
    }
}
