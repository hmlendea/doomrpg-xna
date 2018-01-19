using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Resources;
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

        WallSlice[] wallSlices;

        Dictionary<string, Texture2D> wallTextures;

        public override void LoadContent()
        {
            camera = new Camera();
            camera.AssociatePlayer(player);

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

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            wallSlices = null;
            wallTextures.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            base.Update(gameTime);
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
        }
    }
}
