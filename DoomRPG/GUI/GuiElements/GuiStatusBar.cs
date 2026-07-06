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
        Texture2D fillTexture;
        GuiText healthLabel;
        GuiText armourLabel;

        public void AssociateGameManager(IGameManager game)
        {
            this.game = game;
        }

        protected override void DoLoadContent()
        {
            fillTexture = NuciContentManager.Instance.LoadTexture2D("ScreenManager/FillImage");

            healthLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.White,
                Size = new Size2D(150, 24)
            };

            armourLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.White,
                Size = new Size2D(150, 24)
            };

            healthLabel.LoadContent();
            armourLabel.LoadContent();
        }

        protected override void DoUnloadContent()
        {
            healthLabel.UnloadContent();
            armourLabel.UnloadContent();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            Player player = game.GetPlayer();

            int healthPercent = 0;

            if (player.MaxHealth > 0)
            {
                healthPercent = (int)((float)player.Health / player.MaxHealth * 100);
            }

            int armourPercent = 0;

            if (player.MaxArmour > 0)
            {
                armourPercent = (int)((float)player.Armour / player.MaxArmour * 100);
            }

            healthLabel.Text = $"Health: {healthPercent}%";
            healthLabel.Location = new Point2D(Location.X + 4, Location.Y + 4);

            armourLabel.Text = $"Armour: {armourPercent}%";
            armourLabel.Location = new Point2D(Location.X + 160, Location.Y + 4);

            healthLabel.Update(gameTime);
            armourLabel.Update(gameTime);
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fillTexture, new Rectangle(Location.X, Location.Y, Size.Width, Size.Height), Color.Black);
            healthLabel.Draw(spriteBatch);
            armourLabel.Draw(spriteBatch);
        }
    }
}
