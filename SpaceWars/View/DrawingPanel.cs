using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
using System.Drawing;
using System.IO;

namespace View
{
    /// <summary>
    /// Represents a Panel where all the SpaceWars world objects will be drawn to.
    /// </summary>
    public class DrawingPanel : Panel
    {
        // The game world.
        private World theWorld;

        // Image collections of ship, projectile, star images to draw.
        private Image[] shipModels;
        private Image[] shipModelsThrusting;
        private Image[] projModels;
        private Image starModel;

        /// <summary>
        /// Represents a default panel with no world set to it.
        /// </summary>
        public DrawingPanel()
        {
            theWorld = null;
            DoubleBuffered = true;
            shipModels = new Image[] 
            {
                Properties.Resources.ship0,
                Properties.Resources.ship1,
                Properties.Resources.ship2,
                Properties.Resources.ship3,
                Properties.Resources.ship4,
                Properties.Resources.ship5,
                Properties.Resources.ship6,
                Properties.Resources.ship7
            };
            shipModelsThrusting = new Image[]
            {
                Properties.Resources.thrust0,
                Properties.Resources.thrust1,
                Properties.Resources.thrust2,
                Properties.Resources.thrust3,
                Properties.Resources.thrust4,
                Properties.Resources.thrust5,
                Properties.Resources.thrust6,
                Properties.Resources.thrust7
            };
            projModels = new Image[]
            {
                Properties.Resources.shot0,
                Properties.Resources.shot1,
                Properties.Resources.shot2,
                Properties.Resources.shot3,
                Properties.Resources.shot4,
                Properties.Resources.shot5,
                Properties.Resources.shot6,
                Properties.Resources.shot7
            };
            starModel = Properties.Resources.star;
        }

        /// <summary>
        /// Sets the world for the panel to access information and draw objects.
        /// </summary>
        /// <param name="w"></param>
        public void SetWorld(World w)
        {
            theWorld = w;
        }

        /// <summary>
        /// Delegate function to draw game objects.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();
        }

        /// <summary>
        /// Draws a ship object, with the model drawn depending on its ID.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            Ship ship = o as Ship;
            int width = 35;
            int height = 35;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            int id = ship.GetID() % shipModels.Length;
            Image image;

            if (ship.IsThrusting())
                image = shipModelsThrusting[id];
            else
                image = shipModels[id];

            e.Graphics.DrawImage(image, -(width / 2), -(height / 2), width, height);
        }

        /// <summary>
        /// Draws a projectile, with color depending on the player's ID.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShotCaller(object o, PaintEventArgs e)
        {
            Projectile shot = o as Projectile;
            int width = 20;
            int height = 20;
            int id = shot.GetOwner() % projModels.Length;

            Image image = projModels[id];

            e.Graphics.DrawImage(image, -(width/2), -(height/2), width, height);
        }

        /// <summary>
        /// Draws a star object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void StarDraw(object o, PaintEventArgs e)
        {
            Star star = o as Star;
            int width = 50;
            int height = 50;

            Image image = starModel;
            e.Graphics.DrawImage(image, -(width / 2), -(height / 2), width, height);
        }

        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn. Draws all objects on screen each frame.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if(theWorld != null)
            {
                lock (theWorld.GetShips())
                {
                    foreach (Ship ship in theWorld.GetShips().Values)
                    {
                        if (ship.Alive)
                        {
                            DrawObjectWithTransform(e, ship, theWorld.GetSize(), ship.GetLocation().GetX(), ship.GetLocation().GetY(), ship.GetDirection().ToAngle(), ShipDrawer);
                        }
                    }
                }

                lock(theWorld.GetStars())
                {
                    foreach (Star star in theWorld.GetStars().Values)
                    {
                        DrawObjectWithTransform(e, star, theWorld.GetSize(), star.GetLocation().GetX(), star.GetLocation().GetY(), 0, StarDraw);
                    }
                }

                lock (theWorld.GetProjectiles())
                {
                    foreach (Projectile shot in theWorld.GetProjectiles().Values)
                    {
                        DrawObjectWithTransform(e, shot, theWorld.GetSize(), shot.GetLocation().GetX(), shot.GetLocation().GetY(), shot.GetDirection().ToAngle(), ShotCaller);
                    }
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }
}
