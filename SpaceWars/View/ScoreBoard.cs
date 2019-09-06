using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    /// <summary>
    /// Represents a scoreboard for the SpaceWars game. Each entry conmtain's the player's username, score, and health.
    /// </summary>
    public class Scoreboard : Panel
    {
        // The world, to access the ships within.
        private World theWorld;

        // The list of ship ID's. Helps with sorting by score.
        List<int> ships;

        // The offset for drawing an entry below another. (Initially 0, goes up by 40 for each new player)
        int height;

        /// <summary>
        /// Creates a default Scoreboard, without a set world.
        /// </summary>
        public Scoreboard()
        {
            theWorld = null;
            height = 0;
            DoubleBuffered = true;
            ships = new List<int>();
        }

        /// <summary>
        /// Makes a scoreboard, and associates it with the specified world.
        /// </summary>
        /// <param name="w"></param>
        public Scoreboard(World w)
        {
            theWorld = w;
            height = 0;
            DoubleBuffered = true;
            ships = new List<int>();
        }

        /// <summary>
        /// Sets the world for the scoreboard to access and update itself with.
        /// </summary>
        /// <param name="w"></param>
        public void SetWorld(World w)
        {
            theWorld = w;
        }

        /// <summary>
        /// Adds a ship to the socreboard's list of ships, then sorts by score after the addition.
        /// </summary>
        /// <param name="ship"></param>
        public void AddShipToBoard(Ship ship)
        {
            lock(ships)
            {
                ships.Add(ship.GetID());
            }
            SortShipsByScore();
        }

        /// <summary>
        /// Sorts the list by player score.
        /// </summary>
        public void SortShipsByScore()
        {
            lock (ships)
            {
                ships.Sort(Comparer<int>.Create((ship1, ship2) => theWorld.GetShips()[ship2].GetScore().CompareTo(theWorld.GetShips()[ship1].GetScore())));
            }
        }
        
        /// <summary>
        /// Updates the scoreboard on each frame. All info about each player will be drawn, and the list of ships will be automatically sorted on particular events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if(theWorld != null)
            {
                height = 0;
                using (SolidBrush blackBrush = new SolidBrush(Color.Black))
                {
                    using (SolidBrush greenBrush = new SolidBrush(Color.ForestGreen))
                    {
                        using (Pen blackPen = new Pen(Color.Black))
                        {
                            using (Font font = new Font("Verdana", 12f))
                            {
                                lock (ships)
                                {
                                    foreach (int shipID in ships)
                                    {
                                        Ship ship = theWorld.GetShips()[shipID];
                                        Rectangle healthBar = new Rectangle(0, 20 + height, 220, 15);
                                        e.Graphics.DrawRectangle(blackPen, healthBar);
                                        e.Graphics.FillRectangle(greenBrush, 0, 22 + height, ship.GetHP() * 44, 13);
                                        e.Graphics.DrawString(ship.GetName() + ": " + ship.GetScore(), font, blackBrush, 0.0f, height);
                                        height += 40;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            base.OnPaint(e);
        }
    }
}
