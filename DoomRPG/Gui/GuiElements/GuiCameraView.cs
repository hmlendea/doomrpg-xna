using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Content;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.Gui.GuiElements
{
    public class GuiCameraView : GuiControl
    {
        IGameManager game;
        public Camera camera; // TODO: Remove the camera workaround.
        Player player;

        GuiImage ceiling;
        GuiImage floor;

        WallSlice[] wallSlices;

        Dictionary<string, Texture2D> wallTextures;
        Dictionary<string, Texture2D> mobTextures;
        List<MobInstance> sortedMobs;

        protected override void DoLoadContent()
        {
            player.Direction = new PointF2D(0, -1);

            camera = new Camera();
            camera.AssociateGameManager(game);

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
            wallTextures = [];
            mobTextures = [];
            sortedMobs = [];

            IEnumerable<Wall> walls = game.GetLevelWallDefinitions();

            foreach (Wall wall in walls)
            {
                if (!wallTextures.ContainsKey(wall.SpritesheetName))
                {
                    Texture2D texture = NuciContentManager.Instance.LoadTexture2D("Spritesheets/" + wall.SpritesheetName);
                    wallTextures.Add(wall.SpritesheetName, texture);
                }
            }

            foreach (MobInstance mobInstance in game.GetMobInstances())
            {
                Mob mobDefinition = game.GetMobDefinition(mobInstance.MobId);

                if (!mobTextures.ContainsKey(mobDefinition.SpritesheetName))
                {
                    Texture2D texture = NuciContentManager.Instance.LoadTexture2D("mobs/" + mobDefinition.SpritesheetName);
                    mobTextures.Add(mobDefinition.SpritesheetName, texture);
                }
            }

            foreach (TerminalInstance terminal in game.GetTerminalInstances())
            {
                if (!wallTextures.ContainsKey(terminal.SpritesheetName))
                {
                    Texture2D texture = NuciContentManager.Instance.LoadTexture2D("Spritesheets/" + terminal.SpritesheetName);
                    wallTextures.Add(terminal.SpritesheetName, texture);
                }
            }

            // Registration via RegisterChildren is avoided as those controls would be drawn above the raycasted view.
            ceiling.LoadContent();
            floor.LoadContent();

            camera.LoadContent();
            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            ceiling.UnloadContent();
            floor.UnloadContent();

            wallSlices = null;
            wallTextures.Clear();
            mobTextures.Clear();
            sortedMobs = null;
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            SetChildrenProperties();

            int screenWidth = Size.Width;
            int screenHeight = Size.Height;

            for (int x = 0; x < Size.Width; x += 1)
            {
                double cameraSpaceX = 2 * x / (double)screenWidth - 1;

                double rayPositionX = camera.Position.X;
                double rayPositionY = camera.Position.Y;
                double rayDirectionX = camera.Direction.X + camera.Plane.X * cameraSpaceX;
                double rayDirectionY = camera.Direction.Y + camera.Plane.Y * cameraSpaceX;

                // Determines which tile of the level the ray currently occupies.
                int tileX = (int)rayPositionX;
                int tileY = (int)rayPositionY;

                // Length of ray from current position to next x or y-side.
                double sideDistanceX = 0;
                double sideDistanceY = 0;

                // Length of ray from one x or y-side to next x or y-side.
                double deltaDistanceX = Math.Sqrt(1 + rayDirectionY * rayDirectionY / (rayDirectionX * rayDirectionX));
                double deltaDistanceY = Math.Sqrt(1 + rayDirectionX * rayDirectionX / (rayDirectionY * rayDirectionY));
                double perpendicularWallDistance = 0;

                // Direction to step in x or y (either +1 or -1).
                Point2D stepDirection = Point2D.Empty;

                bool aWallWasHit = false;
                int hitSide = 0; // Indicates whether a north/south (0) or east/west (1) wall face was hit.

                // Calculates the step direction and initial side distances.
                if (rayDirectionX < 0)
                {
                    stepDirection.X = -1;
                    sideDistanceX = (rayPositionX - tileX) * deltaDistanceX;
                }
                else
                {
                    stepDirection.X = 1;
                    sideDistanceX = (tileX + 1.0 - rayPositionX) * deltaDistanceX;
                }

                if (rayDirectionY < 0)
                {
                    stepDirection.Y = -1;
                    sideDistanceY = (rayPositionY - tileY) * deltaDistanceY;
                }
                else
                {
                    stepDirection.Y = 1;
                    sideDistanceY = (tileY + 1.0 - rayPositionY) * deltaDistanceY;
                }

                // Performs the DDA algorithm.
                while (!aWallWasHit)
                {
                    // Advances the ray to the next tile in x or y direction.
                    if (sideDistanceX < sideDistanceY)
                    {
                        sideDistanceX += deltaDistanceX;
                        tileX += stepDirection.X;
                        hitSide = 0;
                    }
                    else
                    {
                        sideDistanceY += deltaDistanceY;
                        tileY += stepDirection.Y;
                        hitSide = 1;
                    }

                    // Checks whether the ray has hit a wall.
                    if (game.GetWall(tileX, tileY) is not null)
                    {
                        aWallWasHit = true;
                    }
                }

                // Calculates the distance projected on the camera direction (oblique distance would cause fisheye distortion).
                if (hitSide == 0)
                {
                    perpendicularWallDistance = Math.Abs((tileX - rayPositionX + (1 - stepDirection.X) / 2.0) / rayDirectionX);
                }
                else
                {
                    perpendicularWallDistance = Math.Abs((tileY - rayPositionY + (1 - stepDirection.Y) / 2.0) / rayDirectionY);
                }

                // Calculates the height of the wall slice to draw on screen.
                int wallLineHeight = (int)Math.Abs(screenHeight / perpendicularWallDistance);

                // Performs texturing calculations for the wall slice.
                WallInstance wallInstance = game.GetWall(tileX, tileY);
                Wall wall = null;

                if (wallInstance is not null)
                {
                    wall = game.GetWallDefinition(wallInstance.WallId);
                }

                // Calculates the exact position on the wall face where the ray hit.
                double wallHitOffset = rayPositionY + (tileX - rayPositionX + (1 - stepDirection.X) / 2.0) / rayDirectionX * rayDirectionY;

                if (hitSide == 1)
                {
                    wallHitOffset = rayPositionX + (tileY - rayPositionY + (1 - stepDirection.Y) / 2.0) / rayDirectionY * rayDirectionX;
                }

                wallHitOffset -= Math.Floor(wallHitOffset);

                // X coordinate on the texture.
                int textureX = (int)(wallHitOffset * GameDefines.TextureSize);

                if ((hitSide == 0 && rayDirectionX > 0) ||
                    (hitSide == 1 && rayDirectionY < 0))
                {
                    textureX = GameDefines.TextureSize - textureX - 1;
                }

                wallSlices[x].Depth = perpendicularWallDistance;
                wallSlices[x].Height = wallLineHeight;
                wallSlices[x].TextureX = textureX;

                if (wall is not null)
                {
                    wallSlices[x].Spritesheet = wall.SpritesheetName;
                    wallSlices[x].SpritesheetTextureIndex = wall.SpritesheetTextureIndex;
                }

                TerminalInstance terminal = game.GetTerminalAtPosition(tileX, tileY);

                if (terminal is not null)
                {
                    wallSlices[x].Spritesheet = terminal.SpritesheetName;
                    wallSlices[x].SpritesheetTextureIndex = terminal.SpritesheetTextureIndex;
                }
            }

            sortedMobs = [.. game.GetMobInstances()
                .OrderByDescending(instance =>
                    Math.Pow(instance.Position.X + 0.5 - camera.Position.X, 2) +
                    Math.Pow(instance.Position.Y + 0.5 - camera.Position.Y, 2))];

            camera.Update(gameTime);
            ceiling.Update(gameTime);
            floor.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            ceiling.Draw(spriteBatch);
            floor.Draw(spriteBatch);

            for (int x = 0; x < Size.Width; x++)
            {
                int columnStart = -wallSlices[x].Height / 2 + Size.Height / 2;
                int columnEnd = columnStart + wallSlices[x].Height;

                int drawStart = Math.Max(0, columnStart);
                int drawEnd = Math.Min(Size.Height, columnEnd);
                int drawLength = drawEnd - drawStart;

                if (drawLength > 0 && !string.IsNullOrWhiteSpace(wallSlices[x].Spritesheet))
                {
                    int textureYOffset = 0;
                    int textureHeight = GameDefines.TextureSize;

                    if (wallSlices[x].Height > 0)
                    {
                        textureYOffset = (drawStart - columnStart) * GameDefines.TextureSize / wallSlices[x].Height;
                    }

                    if (wallSlices[x].Height > 0)
                    {
                        textureHeight = Math.Max(1, drawLength * GameDefines.TextureSize / wallSlices[x].Height);
                    }

                    spriteBatch.Draw(
                        wallTextures[wallSlices[x].Spritesheet],
                        new Rectangle(x, drawStart, 1, drawLength),
                        new Rectangle(wallSlices[x].TextureX + wallSlices[x].SpritesheetTextureIndex * GameDefines.TextureSize, textureYOffset, 1, textureHeight),
                        Color.White);
                }
            }

            DrawMobSprites(spriteBatch);
        }

        private void DrawMobSprites(SpriteBatch spriteBatch)
        {
            int screenWidth = Size.Width;
            int screenHeight = Size.Height;

            foreach (MobInstance mobInstance in sortedMobs)
            {
                Mob mobDefinition = game.GetMobDefinition(mobInstance.MobId);

                if (!mobTextures.TryGetValue(mobDefinition.SpritesheetName, out Texture2D texture))
                {
                    continue;
                }

                // Sprite position relative to camera, centred on the tile.
                double spriteX = mobInstance.Position.X + 0.5 - camera.Position.X;
                double spriteY = mobInstance.Position.Y + 0.5 - camera.Position.Y;

                // Transforms the sprite position into camera space via the inverse camera matrix.
                double inverseDeterminant = 1.0 / (camera.Plane.X * camera.Direction.Y - camera.Direction.X * camera.Plane.Y);
                double transformX = inverseDeterminant * (camera.Direction.Y * spriteX - camera.Direction.X * spriteY);
                double transformY = inverseDeterminant * (-camera.Plane.Y * spriteX + camera.Plane.X * spriteY);

                if (transformY <= 0)
                {
                    continue;
                }

                int spriteScreenX = (int)(screenWidth / 2 * (1 + transformX / transformY));

                // Height and width are set to the same value, producing a square billboard.
                int spriteHeight = (int)Math.Abs(screenHeight / transformY);
                int spriteWidth = spriteHeight;

                // Shift the sprite downward so it appears grounded on the floor.
                int verticalScreenOffset = (int)(GameDefines.MobVerticalDrawOffset * spriteHeight);
                int drawStartY = Math.Max(0, -spriteHeight / 2 + screenHeight / 2 + verticalScreenOffset);
                int drawEndY = Math.Min(screenHeight, spriteHeight / 2 + screenHeight / 2 + verticalScreenOffset);
                int drawStartX = Math.Max(0, -spriteWidth / 2 + spriteScreenX);
                int drawEndX = Math.Min(screenWidth, spriteWidth / 2 + spriteScreenX);

                int columnStartY = -spriteHeight / 2 + screenHeight / 2 + verticalScreenOffset;

                for (int stripe = drawStartX; stripe < drawEndX; stripe++)
                {
                    if (transformY >= wallSlices[stripe].Depth)
                    {
                        continue;
                    }

                    int textureX = Math.Clamp(
                        (stripe - (-spriteWidth / 2 + spriteScreenX)) * texture.Width / spriteWidth,
                        0, texture.Width - 1);

                    int drawLength = drawEndY - drawStartY;

                    if (drawLength <= 0)
                    {
                        continue;
                    }

                    int textureYOffset = Math.Clamp((drawStartY - columnStartY) * texture.Height / spriteHeight, 0, texture.Height - 1);
                    int textureHeight = Math.Clamp(drawLength * texture.Height / spriteHeight, 1, texture.Height - textureYOffset);

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(stripe, drawStartY, 1, drawLength),
                        new Rectangle(textureX, textureYOffset, 1, textureHeight),
                        Color.White);
                }
            }
        }

        // TODO: Find a better approach for the game manager association.
        /// <summary>
        /// Associates the game manager.
        /// </summary>
        /// <param name="game">Game.</param>
        public void AssociateGameManager(IGameManager game)
        {
            this.game = game;

            player = game.GetPlayer();
        }

        private void SetChildrenProperties()
        {
            ceiling.Location = new Point2D(0, 0);
            ceiling.Size = new Size2D(Size.Width, Size.Height / 2);

            floor.Location = new Point2D(0, Size.Height / 2);
            floor.Size = new Size2D(Size.Width, Size.Height / 2);
        }
    }
}
