using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui.Screens;
using NuciXNA.Input;

using DoomRPG.GameLogic.GameManagers;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IGameManager game;
        
        /// <summary>
        /// Loads the content.
        /// </summary>
        public override void LoadContent()
        {
            game = new GameManager();
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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

        }
    }
}
