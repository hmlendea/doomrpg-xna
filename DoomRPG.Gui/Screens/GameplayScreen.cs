using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;
using NuciXNA.Input.Events;

using DoomRPG.GameLogic.GameManagers;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Gui.GuiElements;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IGameManager game;
        GuiCameraView cameraView;
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            game = new GameManager();
            cameraView = new GuiCameraView();

            GuiManager.Instance.GuiElements.Add(cameraView);

            game.LoadContent();

            cameraView.AssociateGameManager(game);

            InputManager.Instance.MouseMoved += Instance_MouseMoved;

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            game.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            game.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (InputManager.Instance.IsAnyKeyDown(Keys.Up, Keys.W))
            {
                game.MovePlayer(MovementDirection.North);
            }
            else if (InputManager.Instance.IsAnyKeyDown(Keys.Down, Keys.S))
            {
                game.MovePlayer(MovementDirection.South);
            }
            else if (InputManager.Instance.IsAnyKeyDown(Keys.Left, Keys.A))
            {
                game.MovePlayer(MovementDirection.West);
            }
            else if (InputManager.Instance.IsAnyKeyDown(Keys.Right, Keys.D))
            {
                game.MovePlayer(MovementDirection.East);
            }
        }

        protected override void SetChildrenProperties()
        {
            cameraView.Size = ScreenManager.Instance.Size;
        }

        private void Instance_MouseMoved(object sender, MouseEventArgs e)
        {
            float angle = (e.PreviousLocation.X - e.Location.X) * 0.0125f;

            if (angle != 0.0f)
            {
                game.RotatePlayer(angle);
                cameraView.camera.Rotate(angle);
            }
        }
    }
}
