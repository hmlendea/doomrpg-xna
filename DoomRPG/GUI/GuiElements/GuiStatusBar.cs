using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NuciXNA.DataAccess.Content;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Settings;

namespace DoomRPG.Gui.GuiElements
{
    public sealed class GuiStatusBar : GuiControl
    {
        IGameManager game;
        SpriteFont font;
        Texture2D fillTexture;
        string healthText;

        public void AssociateGameManager(IGameManager game)
        {
            this.game = game;
        }

        protected override void DoLoadContent()
        {
            font = NuciContentManager.Instance.LoadSpriteFont("Fonts/MenuFont");
            fillTexture = NuciContentManager.Instance.LoadTexture2D("ScreenManager/FillImage");
            healthText = string.Empty;
        }

        protected override void DoUnloadContent()
        {
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            Player player = game.GetPlayer();

            int healthPercent = 0;

            if (player.MaxHealth > 0)
            {
                healthPercent = (int)((float)player.Health / player.MaxHealth * 100);
            }

            healthText = $"Health: {healthPercent}%";
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fillTexture, new Rectangle(Location.X, Location.Y, Size.Width, Size.Height), Color.Black);
            spriteBatch.DrawString(font, healthText, new Vector2(Location.X + 4, Location.Y + 4), Color.White);
        }
    }
}
