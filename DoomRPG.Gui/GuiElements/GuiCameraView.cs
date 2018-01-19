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
        Camera camera;
        Player player;

        GuiImage ceiling;
        GuiImage floor;

        WallSlice[] wallSlices;

        Dictionary<string, Texture2D> wallTextures;

        public override void LoadContent()
        {
            camera = new Camera();
            camera.AssociatePlayer(player);

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
