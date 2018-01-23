using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Resources;
using NuciXNA.Graphics.Enumerations;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.Gui.GuiElements
{
    public class GuiCameraView : GuiElement
    {
        IGameManager game;
        public Camera camera; // TODO: remove workaround
        Player player;

        GuiImage ceiling;
        GuiImage floor;

        WallSlice[] wallSlices;

        Dictionary<string, Texture2D> wallTextures;

        public override void LoadContent()
        {
            player.Direction = new PointF2D(0, -1);

            camera = new Camera();
            camera.AssociateGameManager(game);

            // TODO: Remove hardcoding
            camera.Position = new PointF2D(4f, 4f);
            camera.Plane = new PointF2D(
                player.Direction.Y * camera.FieldOfView,
                -player.Direction.X * camera.FieldOfView);

            ceiling = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle2D(0, 0, 1, 1),
                TintColour = game.GetLevelCeilingColour(),
                TextureLayout = TextureLayout.Tile
            };
            floor = new GuiImage()
            {
                ContentFile = "ScreenManager/FillImage",
                SourceRectangle = new Rectangle2D(0, 0, 1, 1),
                TintColour = game.GetLevelFloorColour(),
                TextureLayout = TextureLayout.Tile
            };

            wallSlices = new WallSlice[Size.Width];
            wallTextures = new Dictionary<string, Texture2D>();

            IEnumerable<Wall> walls = game.GetLevelWallDefinitions();

            foreach (Wall wall in walls)
            {
                if (!wallTextures.ContainsKey(wall.SpritesheetName))
                {
                    Texture2D texture = ResourceManager.Instance.LoadTexture2D("Spritesheets/" + wall.SpritesheetName);
                    wallTextures.Add(wall.SpritesheetName, texture);
                }
            }

            camera.LoadContent();
            ceiling.LoadContent();
            floor.LoadContent();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            ceiling.UnloadContent();
            floor.UnloadContent();

            wallSlices = null;
            wallTextures.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            int ScreenWidth = Size.Width;
            int ScreenHeight = Size.Height;

            for (int x = 0; x < Size.Width; x++)
            {
                //x-coordinate in camera space
                double cameraX = 2 * x / (double)ScreenWidth - 1;

                double rayPosX = camera.Position.X;
                double rayPosY = camera.Position.Y;
                double rayDirX = camera.Direction.X + camera.Plane.X * cameraX;
                double rayDirY = camera.Direction.Y + camera.Plane.Y * cameraX;
                
                //which box of the map we're in  
                int mapX = (int)rayPosX;
                int mapY = (int)rayPosY;

                //length of ray from current position to next x or y-side
                double sideDistX = 0;
                double sideDistY = 0;

                //length of ray from one x or y-side to next x or y-side
                double deltaDistX = Math.Sqrt(1 + (rayDirY * rayDirY) / (rayDirX * rayDirX));
                double deltaDistY = Math.Sqrt(1 + (rayDirX * rayDirX) / (rayDirY * rayDirY));
                double perpWallDist = 0;

                //what direction to step in x or y-direction (either +1 or -1)
                Point2D step = Point2D.Empty;

                bool wallHit = false;
                int side = 0; //was a NS or a EW wall hit?

                //calculate step and initial sideDist
                if (rayDirX < 0)
                {
                    step.X = -1;
                    sideDistX = (rayPosX - mapX) * deltaDistX;
                }
                else
                {
                    step.X = 1;
                    sideDistX = (mapX + 1.0 - rayPosX) * deltaDistX;
                }

                if (rayDirY < 0)
                {
                    step.Y = -1;
                    sideDistY = (rayPosY - mapY) * deltaDistY;
                }
                else
                {
                    step.Y = 1;
                    sideDistY = (mapY + 1.0 - rayPosY) * deltaDistY;
                }

                //perform DDA
                while (!wallHit)
                {
                    //jump to next map square, OR in x-direction, OR in y-direction
                    if (sideDistX < sideDistY)
                    {
                        sideDistX += deltaDistX;
                        mapX += step.X;
                        side = 0;
                    }
                    else
                    {
                        sideDistY += deltaDistY;
                        mapY += step.Y;
                        side = 1;
                    }
                    
                    //Check if ray has hit a wall
                    if (game.GetWall(mapX, mapY) != null)
                    {
                        wallHit = true;
                    }
                }

                //Calculate distance projected on camera direction (oblique distance will give fisheye effect!)
                if (side == 0)
                {
                    perpWallDist = Math.Abs((mapX - rayPosX + (1 - step.X) / 2.0) / rayDirX);
                }
                else
                {
                    perpWallDist = Math.Abs((mapY - rayPosY + (1 - step.Y) / 2.0) / rayDirY);
                }

                //Calculate height of line to draw on screen
                int lineHeight = (int)Math.Abs(ScreenHeight / perpWallDist);
                
                //texturing calculations
                WallInstance wallInstance = game.GetWall(mapX, mapY);
                Wall wall = null;

                if (wallInstance != null)
                {
                    wall = game.GetWallDefinition(wallInstance.WallId);
                }

                //calculate where exactly the wall was hit
                double wallX;

                if (side == 1)
                {
                    wallX = rayPosX + ((mapY - rayPosY + (1 - step.Y) / 2.0) / rayDirY) * rayDirX;
                }
                else
                {
                    wallX = rayPosY + ((mapX - rayPosX + (1 - step.X) / 2.0) / rayDirX) * rayDirY;
                }

                wallX -= Math.Floor(wallX);

                //x coordinate on the texture
                int texX = (int)(wallX * GameDefines.TextureSize);
                if ((side == 0 && rayDirX > 0) ||
                    (side == 1 && rayDirY < 0))
                {
                    texX = GameDefines.TextureSize - texX - 1;
                }

                wallSlices[x].Depth = perpWallDist;
                wallSlices[x].Height = lineHeight;
                wallSlices[x].TextureX = texX;

                if (wall != null)
                {
                    wallSlices[x].Spritesheet = wall.SpritesheetName;
                    wallSlices[x].SpritesheetTextureIndex = wall.SpritesheetTextureIndex;
                }
            }

            camera.Update(gameTime);
            ceiling.Update(gameTime);
            floor.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            ceiling.Draw(spriteBatch);
            floor.Draw(spriteBatch);

            for (int x = 0; x < Size.Width; x++)
            {
                int columnStart = -wallSlices[x].Height / 2 + Size.Height / 2;
                int columnLength = wallSlices[x].Height / 2 + Size.Height / 2 - columnStart;
                
                if (!string.IsNullOrWhiteSpace(wallSlices[x].Spritesheet))
                {
                    spriteBatch.Draw(
                        wallTextures[wallSlices[x].Spritesheet],
                        new Rectangle(x, columnStart, 1, columnLength),
                        new Rectangle(wallSlices[x].TextureX + wallSlices[x].SpritesheetTextureIndex * GameDefines.TextureSize, 0, 1, GameDefines.TextureSize),
                        Color.White);
                }
            }
        }

        // TODO: Handle this better
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(IGameManager game)
        {
            this.game = game;

            player = game.GetPlayer();
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            ceiling.Location = new Point2D(0, 0);
            ceiling.Size = new Size2D(Size.Width, Size.Height / 2);

            floor.Location = new Point2D(0, Size.Height / 2);
            floor.Size = new Size2D(Size.Width, Size.Height / 2);
        }
    }
}
