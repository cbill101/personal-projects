/*
 * BIG NOTICE
 * 
 * ALWAYS run all the tests. Because of our implementation, the IDs are global, so all tests have to be run at the same time, no exceptions.
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using SpaceWars;
using System.Collections.Generic;

namespace ModelTester
{
    [TestClass]
    public class ModelTester
    {

    ///--------------------------------------Projectile------------------------------------------------------------------------------------
        [TestMethod]
        public void DefaultOwner()
        {
            Projectile tester = new Projectile();
            Assert.IsTrue(tester.GetOwner() == -1);
        }


        [TestMethod]
        public void DefaultAlive()
        {
            Projectile tester = new Projectile();
            Assert.IsFalse(tester.IsAlive());
        }

        [TestMethod]
        public void DefaultID()
        {
            Projectile tester = new Projectile();
            Assert.IsTrue(tester.GetID() == -1);
        }

        [TestMethod]
        public void CustomeOwner()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Assert.IsTrue(tester.GetOwner() == 42);
        }


        [TestMethod]
        public void customeAlive()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Assert.IsTrue(tester.IsAlive());
        }

        [TestMethod]
        public void CustomID()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);

            Assert.AreEqual(tester.GetID(), 2);
        }

        [TestMethod]
        public void CustomLoc()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Assert.AreEqual(tester.GetLocation(), new Vector2D(4, 6) );
        }
        [TestMethod]
        public void CustomDir()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Assert.AreEqual(tester.GetDirection(), new Vector2D(7, 8));
        }

        [TestMethod]
        public void ProjectileKill()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Star testStar = new Star(new Vector2D(109, 126), .06, 50);
            List<Star> stars = new List<Star>();
            stars.Add(testStar);
            tester.Update(stars);
            Assert.IsFalse(tester.IsAlive());
        }
        [TestMethod]
        public void ProjectileKillDirect()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            tester.Kill();
            Assert.IsFalse(tester.IsAlive());
        }
        [TestMethod]
        public void ProjectileJSONToString()
        {
            Projectile tester = new Projectile(new Vector2D(4, 6), new Vector2D(7, 8), 42);
            Assert.AreEqual("{\"proj\":7,\"loc\":{\"x\":4.0,\"y\":6.0},\"dir\":{\"x\":7.0,\"y\":8.0},\"alive\":true,\"owner\":42}", tester.ToString());
        }
        ///--------------------------------------Ship------------------------------------------------------------------------------------
        [TestMethod]
        public void ShipDefaultTeam()
        {
            Ship tester = new  Ship();
            Assert.IsTrue(tester.GetTeam() == -1);
        }


        [TestMethod]
        public void ShipDefaultAlive()
        {
            Ship tester = new Ship();
            Assert.IsFalse(tester.IsActive());
        }

        [TestMethod]
        public void ShipDefaultID()
        {
            Ship tester = new Ship();
            Assert.IsTrue(tester.GetID() == -1);
        }


        [TestMethod]
        public void ShipCustomID()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.SetID(5);
            Assert.AreEqual(tester.GetID(), 5);
        }

        [TestMethod]
        public void ShipCustomLoc()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            Assert.AreEqual(tester.GetLocation(), new Vector2D(4, 6));
        }
        [TestMethod]
        public void ShipCustomDir()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            Assert.AreEqual(tester.GetDirection(), new Vector2D(7, 8));
        }

        [TestMethod]
        public void ShipHit()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.Hit(5);
            Assert.AreEqual(tester.GetHP(), 4);
        }
        [TestMethod]
        public void ShipincreaseSCore()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.IncreaseScore();
            Assert.AreEqual(tester.GetScore(), 1);
        }
        [TestMethod]
        public void ShipgetName()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            Assert.AreEqual(tester.GetName(), "meme");
        }
        [TestMethod]
        public void Shipisthrusting()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            Assert.IsFalse(tester.IsThrusting());
        }
        [TestMethod]
        public void ShipDetermineTeams()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, true);
            Assert.AreEqual(1, tester.GetTeam());

            Ship test2 = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, true);
            Assert.AreEqual(0, test2.GetTeam());
        }
        [TestMethod]
        public void ShipGetHitBoxSize()
        {
            Ship tester = new Ship(new Vector2D(100, 100), new Vector2D(1, 0), "meme", 4, .6, 2.0, 5, 25, 0, 0, false);
            Assert.AreEqual(25U, tester.GetHitBoxSize());
        }
        [TestMethod]
        public void ShipMakeInactive()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.MakeInactive();
            Assert.IsFalse(tester.IsActive());
        }
        [TestMethod]
        public void ShipWrapAroundBoth()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.WrapAroundX();
            Assert.AreEqual(new Vector2D(-4, 6), tester.GetLocation());
            tester.WrapAroundX();
            Assert.AreEqual(new Vector2D(4, 6), tester.GetLocation());
            tester.WrapAroundY();
            Assert.AreEqual(new Vector2D(4, -6), tester.GetLocation());
            tester.WrapAroundY();
            Assert.AreEqual(new Vector2D(4, 6), tester.GetLocation());
        }
        [TestMethod]
        public void ShipDead()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            for(int hitIndex = 0; hitIndex < 5; hitIndex++)
            {
                tester.Hit(5U);
            }
            Assert.AreEqual(5U, tester.GetLastDeath());
        }
        [TestMethod]
        public void ShipDeadOnStarCollisionAndRespawn()
        {
            Ship tester = new Ship(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25, 0, 0, false);
            tester.Update(new List<Star>() { new Star() }, 0U);
            Assert.IsFalse(tester.Alive);

            tester.Respawn(new Vector2D(50, 50));
            Assert.IsTrue(tester.GetHP() == 5);
            Assert.AreEqual(new Vector2D(50, 50), tester.GetLocation());
        }
        [TestMethod]
        public void ShipInputTestFull()
        {
            Ship tester = new Ship(new Vector2D(100, 100), new Vector2D(1, 0), "meme", 4, .6, 2.0, 5, 25, 0, 0, false);
            tester.ProcessCommand('L');
            tester.ApplyCommands();
            Assert.IsTrue(tester.GetDirection().GetX() < 1 && tester.GetDirection().GetY() < 0);

            tester.ProcessCommand('R');
            tester.ApplyCommands();
            Assert.AreEqual(new Vector2D(1, 0), tester.GetDirection());

            tester.ProcessCommand('T');
            tester.Update(new List<Star>() { new Star(new Vector2D(0, 0), 0.01, 35) }, 0U);
            Assert.IsTrue(tester.GetLocation().GetX() > 100 && tester.GetLocation().GetY() < 100);

            tester.ProcessCommand('R');
            tester.ApplyCommands();
            Assert.IsTrue(tester.GetDirection().GetX() < 1 && tester.GetDirection().GetY() > 0);
        }
        [TestMethod]
        public void ShipFiring()
        {
            Ship tester = new Ship(new Vector2D(100, 100), new Vector2D(1, 0), "meme", 4, .6, 2.0, 5, 25, 0, 0, false);
            Assert.IsTrue(tester.Fire(4U, out Projectile shot));
            Assert.IsFalse(tester.Fire(6U, out Projectile fail));
            Assert.IsTrue(tester.Fire(8U, out Projectile success));
            tester.Die(9U);
            Assert.IsFalse(tester.Fire(20U, out Projectile cantFireWhenDead));
        }
        [TestMethod]
        public void ShipJSONToString()
        {
            Ship tester = new Ship();
            Assert.AreEqual("{\"ship\":-1,\"loc\":{\"x\":-1.0,\"y\":-1.0},\"dir\":{\"x\":-1.0,\"y\":-1.0},\"thrust\":false,\"name\":\"\",\"hp\":0,\"score\":0}", tester.ToString());
        }
        ///--------------------------------------Star------------------------------------------------------------------------------------
        [TestMethod]
        public void StarDefault()
        {
            Star star = new Star();
            Assert.AreEqual(star.GetID(), -1);
        }
        [TestMethod]
        public void StarGetMass()
        {
            Star star = new Star(new Vector2D(4, 6), 0.6, 25);
            Assert.AreEqual(star.GetMass(), 0.6);
        }
        [TestMethod]
        public void StarJSONToString()
        {
            Star star = new Star();
            Assert.AreEqual("{\"star\":-1,\"loc\":{\"x\":-1.0,\"y\":-1.0},\"mass\":0.0}", star.ToString());
        }


        ///--------------------------------------World------------------------------------------------------------------------------------
        [TestMethod]
        public void WorldGetSize()
        {
            World tester = new World();
            tester.SetSize(700);
            Assert.AreEqual(tester.GetSize(), 700);
        }
        [TestMethod]
        public void WorldGetDelay()
        {
            World tester = new World();
            Assert.AreEqual(tester.GetRespawnDelay(), (uint) 300);
        }
        [TestMethod]
        public void WorldGetDElay2()
        {
            World tester = new World();
            Assert.AreEqual(tester.GetShotFireDelay(), (uint) 6);
        }
        [TestMethod]
        public void Worldnotempty()
        {
            World tester = new World();
            Ship ship = new Ship();
            Projectile proj = new Projectile();
            Star star = new Star();
            Assert.IsNotNull(tester.GetShips());
            Assert.IsNotNull(tester.GetProjectiles());
            Assert.IsNotNull(tester.GetStars());
        }
        [TestMethod]
        public void WorldAdds()
        {
            World tester = new World();
            tester.AddShip(new Vector2D(4, 6), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25);
            tester.AddStar(new Vector2D(4, 6), 4.0, 25);
            Assert.IsNotNull(tester.GetShips());
            Assert.IsNotNull(tester.GetStars());
        }
        [TestMethod]
        public void WorldAddAndClean()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World();
            tester.AddShip(new Vector2D(105, 105), new Vector2D(7, 8), "meme", 4, .6, 4.0, 5, 25);
            tester.AddStar(new Vector2D(4, 6), 4.0, 25);
            Assert.IsNotNull(tester.GetShips());
            Assert.IsNotNull(tester.GetStars());
            for(int updateIdx = 0; updateIdx < 5; updateIdx++)
            {
                tester.Update();
            }
            tester.ProcessCommand(23, 'F');
            tester.GetShips()[23].MakeInactive();
            tester.GetProjectiles()[10].Kill();

            tester.Cleanup();

            Assert.IsFalse(tester.GetShips().ContainsKey(23));
            Assert.IsFalse(tester.GetProjectiles().ContainsKey(10));
        }
        [TestMethod]
        public void RespawnShipWorld()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(4, 6), new Vector2D(0, 1), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            for (int updateIdx = 0; updateIdx < 302; updateIdx++)
            {
                // Once 301 frames pass, the ship should respawn at a new location!
                tester.Update();
            }

            Assert.IsTrue(tester.GetShips()[24].Alive);
        }
        [TestMethod]
        public void WrapAroundTesting()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(376, 376), new Vector2D(0, 1), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.Update();
            Ship ship = tester.GetShips()[25];
            Assert.AreEqual(-376, ship.GetLocation().GetX(), 0.1);
            Assert.AreEqual(-376, ship.GetLocation().GetY(), 0.1);
        }
        [TestMethod]
        public void ShipHitByProjectileFFA()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(100, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(130, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for(int updateIdx = 0; updateIdx < 5; updateIdx++)
            {
                tester.Update();
            }

            tester.ProcessCommand(26, 'F');

            for (int updateIdx = 0; updateIdx < 5; updateIdx++)
            {
                tester.Update();
                tester.Cleanup();
            }

            Assert.IsTrue(tester.GetShips()[27].GetHP() == 4);
        }

        [TestMethod]
        public void ShipKilledByProjectiles()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(100, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(130, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 40; updateIdx++)
            {
                tester.ProcessCommand(28, 'F');
                tester.Update();
                tester.Cleanup();
            }

            Assert.IsFalse(tester.GetShips()[29].Alive);
        }

        [TestMethod]
        public void ShipKilledByProjectilesTeam1()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, true);
            tester.AddShip(new Vector2D(100, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(130, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(-300, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 40; updateIdx++)
            {
                tester.ProcessCommand(30, 'F');
                tester.Update();
                tester.Cleanup();
            }

            Assert.IsFalse(tester.GetShips()[37].Alive);
            Assert.IsTrue(tester.GetShips()[30].GetScore() == tester.GetShips()[38].GetScore());
        }

        [TestMethod]
        public void ShipKilledByProjectilesTeam2()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, true);
            tester.AddShip(new Vector2D(100, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(130, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(-300, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 40; updateIdx++)
            {
                tester.ProcessCommand(45, 'F');
                tester.Update();
                tester.Cleanup();
            }

            Assert.IsFalse(tester.GetShips()[46].Alive);
            Assert.IsTrue(tester.GetShips()[45].GetScore() == tester.GetShips()[53].GetScore());
        }

        [TestMethod]
        public void FriendlyFire()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, true);
            tester.AddShip(new Vector2D(100, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(-300, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(130, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 40; updateIdx++)
            {
                tester.ProcessCommand(54, 'F');
                tester.Update();
                tester.Cleanup();
            }

            Assert.IsTrue(tester.GetShips()[62].Alive && tester.GetShips()[62].GetHP() == 5);

        }
        [TestMethod]
        public void CheckProjectileBoundsX()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(-370, 100), new Vector2D(-1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(370, 100), new Vector2D(1, 0), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 6; updateIdx++)
            {
                tester.ProcessCommand(69, 'F');
                tester.Update();
                if (updateIdx == 4)
                {
                    Assert.IsTrue(tester.GetProjectiles().Count > 0);
                }
                tester.Cleanup();
            }

            for (int updateIdx = 0; updateIdx < 6; updateIdx++)
            {
                tester.ProcessCommand(70, 'F');
                tester.Update();
                if (updateIdx == 4)
                {
                    Assert.IsTrue(tester.GetProjectiles().Count > 0);
                }
                tester.Cleanup();
            }

            Assert.IsTrue(tester.GetProjectiles().Count == 0);

        }
        [TestMethod]
        public void CheckProjectileBoundsY()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShip(new Vector2D(0, -370), new Vector2D(0, -1), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);
            tester.AddShip(new Vector2D(0, 370), new Vector2D(0, 1), "meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            for (int updateIdx = 0; updateIdx < 6; updateIdx++)
            {
                tester.ProcessCommand(71, 'F');
                tester.Update();
                if (updateIdx == 4)
                {
                    Assert.IsTrue(tester.GetProjectiles().Count > 0);
                }
                tester.Cleanup();
            }

            for (int updateIdx = 0; updateIdx < 6; updateIdx++)
            {
                tester.ProcessCommand(72, 'F');
                tester.Update();
                if (updateIdx == 4)
                {
                    Assert.IsTrue(tester.GetProjectiles().Count > 0);
                }
                tester.Cleanup();
            }

            Assert.IsTrue(tester.GetProjectiles().Count == 0);

        }
        [TestMethod]
        public void RandomPositionSafety()
        {
            // DO NOT MOVE THIS TEST AT ALL. MAKE TESTS BELOW THIS ONE OR ELSE FAILURE OCCURS. (Because of the global IDs.)
            World tester = new World(750, new List<Star>() { new Star(new Vector2D(4, 6), 0.01, 25) }, 4U, 300U, false);
            tester.AddShipRandomPosition("meme", tester.GetShotFireDelay(), 0.6, 4.0, 5, 25);

            Assert.IsTrue(tester.GetShips()[73].Alive);

        }
    }
}
