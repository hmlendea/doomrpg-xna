using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;

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
        protected override void DoLoadContent()
        {
            game = new GameManager();
            cameraView = new GuiCameraView();

            GuiManager.Instance.RegisterControls(cameraView);

            game.LoadContent();

            cameraView.AssociateGameManager(game);

            InputManager.Instance.MouseMoved += Instance_MouseMoved;

            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            InputManager.Instance.MouseMoved -= Instance_MouseMoved;
            
            game.UnloadContent();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
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

            SetChildrenProperties();
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {

        }

        void SetChildrenProperties()
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
