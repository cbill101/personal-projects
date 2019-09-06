using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Controller.NetworkController;

namespace Controller
{
    /// <summary>
    /// Represents the "controller" for the SpaceWars game. Manages user input, and sends it to the server for further processing.
    /// </summary>
    public class InputController
    {
        // An int representing the turn control. -1 if a left turn, 1 if a right turn, 0 if no turn.
        private int turnType;

        // Controls thrusting.
        private bool thrusting;

        // Controls firing.
        private bool firing;

        // The server to send the command to.
        private Socket server;

        /// <summary>
        /// Sets the server to send input commands to.
        /// </summary>
        /// <param name="serv"></param>
        public void SetServer(Socket serv)
        {
            server = serv;
        }

        /// <summary>
        /// Processes the input by making a string, enclosed inparentheses the inputs by the user.
        /// R will be sent if the user makes a right turn.
        /// L will be sent if the user makes a left turn.
        /// T will be sent if the user is thrusting forward.
        /// F will be sent if the user is firing projectiles.
        /// If the user is using multiple inputs, they will be sent collectively to the server.
        /// </summary>
        public void ProcessInput()
        {
            if (turnType == 0 && !thrusting && !firing)
                return;

            StringBuilder stringBuilder = new StringBuilder();

            if (turnType == 1)
                stringBuilder.Append("R");
            else if (turnType == -1)
                stringBuilder.Append("L");
            if (thrusting)
                stringBuilder.Append("T");
            if (firing)
                stringBuilder.Append("F");

            Networking.SendSocket(server, "(" + stringBuilder.ToString() + ")\n");
        }

        /// <summary>
        /// Handles input when keys are pressed down.
        /// </summary>
        /// <param name="e"></param>
        public void HandleKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    firing = true;
                    break;
                case Keys.Left:
                    if (turnType != 0)
                        break;
                    turnType = -1;
                    break;
                case Keys.Up:
                    thrusting = true;
                    break;
                case Keys.Right:
                    if (turnType != 0)
                        break;
                    turnType = 1;
                    break;
            }
        }

        /// <summary>
        /// Handles input when keys are released.
        /// </summary>
        /// <param name="e"></param>
        public void HandleKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    this.firing = false;
                    break;
                case Keys.Left:
                    if (turnType != -1)
                        break;
                    turnType = 0;
                    break;
                case Keys.Up:
                    thrusting = false;
                    break;
                case Keys.Right:
                    if (turnType != 1)
                        break;
                    turnType = 0;
                    break;
            }
        }
    }
}
