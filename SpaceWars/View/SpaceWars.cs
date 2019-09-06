using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Model;
using static Controller.NetworkController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace View
{
    /// <summary>
    /// Represents the main form for the SpaceWars game. The form manages incoming data and GUI structure for the game.
    /// </summary>
    public partial class SpaceWars : Form
    {
        // The game world.
        private World theWorld;

        // The client's unique socket ID.
        private int uniqueID;

        // The input manager for the game.
        private InputController inputController;

        // The panel where all the game objects are drawn.
        private DrawingPanel drawPanel;

        // The scoreboard, keeping track of each ship's score and health.
        private Scoreboard scoreBoard;

        /// <summary>
        /// Represents a SpaceWars form, creating all the controls necessary for the game to function.
        /// </summary>
        public SpaceWars()
        {
            InitializeComponent();
            theWorld = new World();
            inputController = new InputController();

            drawPanel = new DrawingPanel();
            drawPanel.Location = new Point(0, 58);
            drawPanel.BackColor = Color.Black;
            drawPanel.Size = new Size(750, 750);

            scoreBoard = new Scoreboard(theWorld);
            scoreBoard.Location = new Point(750, 58);
            scoreBoard.Size = new Size(230, 750);
            scoreBoard.BackColor = Color.LightGray;

            this.Controls.Add(drawPanel);
            this.Controls.Add(scoreBoard);

            this.AcceptButton = connectButton;
            connectButton.Enabled = false;
        }

        /// <summary>
        /// The fucntion executed upon first connection to a SpaceWars server. If the connection fails, the user can insert a new server address.
        /// If the connection is successful, the username will be sent to the server and ReceiuveStartup will be called after the server replies.
        /// </summary>
        /// <param name="state"></param>
        private void FirstContact(SocketState state)
        {
            if (state.theSocket == null || !state.theSocket.Connected)
            {
                MethodInvoker resetControls = () =>
                {
                    MessageBox.Show("Unable to connect to server, or server is not present. Try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connectButton.Enabled = true;
                    serverTextBox.Enabled = true;
                    usernameTextBox.Enabled = true;
                };
                Invoke(resetControls);
            }
            else
            {
                state.ClientCallback = ReceiveStartup;
                inputController.SetServer(state.theSocket);
                Networking.SendSocket(state.theSocket, usernameTextBox.Text + "\n");
            }
        }

        /// <summary>
        /// Called when the server sends its first set of data. This extracts the initial data sent from the server (the client's ID, and world size).
        /// This also adjusts the GUI to adapt to the server's world size. After this, the function initiates the networking data loop that drives the entire game.
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveStartup(SocketState state)
        {
            // This block splits up the server data within the StringBuilder, and places it into initialInfo.
            StringBuilder sb = state.sb;
            char[] separator = new char[] { '\n' };
            string[] initialInfo = sb.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries);

            // Assigns the ID sent by the server to the socket state for uniqie identification of connections.
            state.sb.Remove(0, initialInfo[0].Length + 1);
            uniqueID = int.Parse(initialInfo[0]);
            state.ID = uniqueID;

            // Assign the world to a new world with the specified size. Also assigns the world to the child controls for later access.
            state.sb.Remove(0, initialInfo[1].Length + 1);
            theWorld.SetSize(int.Parse(initialInfo[1]));
            drawPanel.SetWorld(theWorld);
            scoreBoard.SetWorld(theWorld);

            // Adapts the GUI to the new world size by resizing the drawing panel, scoreboard, and prevents resizing of the form.
            MethodInvoker resizePanels = new MethodInvoker(() =>
            {
                drawPanel.Size = new Size(theWorld.GetSize(), theWorld.GetSize());
                scoreBoard.Location = new Point(theWorld.GetSize(), 58);
                scoreBoard.Height = theWorld.GetSize();
                this.Height = theWorld.GetSize() + 90;
                this.Width = theWorld.GetSize() + scoreBoard.Width;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
            });
            Invoke(resizePanels);

            state.ClientCallback = ReceiveWorld;
            Networking.GetData(state);
        }

        /// <summary>
        /// Receives information from the server. Deserializes full JSON objects sent by the server and updates the world using the object's information.
        /// After, initiates a frame draw and requests more data from the server.
        /// </summary>
        /// <param name="state"></param>
        public void ReceiveWorld(SocketState state)
        {
            StringBuilder sb = state.sb;
            char[] pattern = new char[] { '\n' };
            string[] info = sb.ToString().Split(pattern, StringSplitOptions.RemoveEmptyEntries);

            foreach (string jsonObject in info)
            {
                try
                {
                    // This statement further ensures JSON object completeness.
                    if(jsonObject[0] == '{' && jsonObject[jsonObject.Length - 1] == '}')
                    {
                        JObject jsonObj = JObject.Parse(jsonObject);

                        JToken shipToken = jsonObj["ship"];
                        JToken projToken = jsonObj["proj"];
                        JToken starToken = jsonObj["star"];

                        // Potential references to the JSON object sent.
                        Ship theShip = null;
                        Star theStar = null;
                        Projectile theProjectile = null;

                        // Assigns the JSON object to the deserialized object type.
                        if (shipToken != null)
                            theShip = JsonConvert.DeserializeObject<Ship>(jsonObject);
                        if (projToken != null)
                            theProjectile = JsonConvert.DeserializeObject<Projectile>(jsonObject);
                        if (starToken != null)
                            theStar = JsonConvert.DeserializeObject<Star>(jsonObject);

                        // Update the ship entries if the object is a ship.
                        lock (theWorld.GetShips())
                        {
                            if (theShip != null)
                            {
                                if (theWorld.GetShips().ContainsKey(theShip.GetID()))
                                {
                                    if(theWorld.GetShips()[theShip.GetID()].Alive && !theShip.Alive)
                                    {
                                        scoreBoard.SortShipsByScore();
                                    }

                                    theWorld.GetShips()[theShip.GetID()] = theShip;
                                }
                                else
                                {
                                    theWorld.GetShips()[theShip.GetID()] = theShip;
                                    scoreBoard.AddShipToBoard(theShip);
                                }
                            }
                        }

                        // Update projectile entries if the object is a projectile.
                        lock(theWorld.GetProjectiles())
                        {
                            if (theProjectile != null)
                            {
                                if (theProjectile.IsAlive())
                                {
                                    theWorld.GetProjectiles()[theProjectile.GetID()] = theProjectile;
                                }
                                else if (theWorld.GetProjectiles().ContainsKey(theProjectile.GetID()))
                                {
                                    theWorld.GetProjectiles().Remove(theProjectile.GetID());
                                }
                            }
                        }

                        // Update star entries if the object is a star.
                        lock(theWorld.GetStars())
                        {
                            if (theStar != null)
                            {
                                theWorld.GetStars()[theStar.GetID()] = theStar;
                            }
                        }

                        sb.Remove(0, jsonObject.Length + 1);
                    }
                }
                catch (JsonReaderException e)
                {

                }
                catch (Exception e)
                {

                }
            }

            // Invalidate this form and all its children (true)
            // This will cause the form to redraw as soon as it can
            // Also, any input is processed and sent over to the server
            MethodInvoker invalidator = new MethodInvoker(() => this.Invalidate(true));
            Invoke(invalidator);
            inputController.ProcessInput();

            state.ClientCallback = ReceiveWorld;
            Networking.GetData(state);
        }

        /// <summary>
        /// When the Help -> Controls tab is clicked, a MessageBox shows the controls for the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlsTab_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Left Arrow: Turn Left\n" +
                "Right Arrow: Turn Right\n" +
                "Up Arrow: Fire Thrusters (Accelerate)\n" +
                "Space: Fire", 
                "SpaceWars Controls", MessageBoxButtons.OK, MessageBoxIcon.Information
                );
        }

        /// <summary>
        /// When the Close tab is clicked, the game client closes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseTab_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// If the is no username, the user cannot connect to the server. If there is an entered username, connecting is allowed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (usernameTextBox.Text == "")
                connectButton.Enabled = false;
            else
                connectButton.Enabled = true;
        }

        /// <summary>
        /// When the Connect button is clicked, the clientwill connect to the server address specified in the server text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Networking.ConnectToServer(FirstContact, serverTextBox.Text);
            connectButton.Enabled = false;
            usernameTextBox.Enabled = false;
            serverTextBox.Enabled = false;
        }

        /// <summary>
        /// Processes game input when keys are pressed down.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            inputController.HandleKeyDown(e);
        }

        /// <summary>
        /// Processes game input when keys are released.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            inputController.HandleKeyUp(e);
        }

        /// <summary>
        /// When Help -> About is clicked, a MessageBox will show who wrote the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutTab_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Space Wars (Star Wars Themed), Made By Chris Billingsley and Dylan Northcutt\n" +
                "Enjoy!", "About The Program", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
