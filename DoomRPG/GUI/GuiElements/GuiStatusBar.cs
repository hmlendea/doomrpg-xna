using Microsoft.Xna.Framework;
using NuciXNA.Gui.Controls;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;

namespace DoomRPG.Gui.GuiElements
{
    public sealed class GuiStatusBar : GuiControl
    {
        IGameManager game;
        GuiImage background;
        GuiText healthLabel;
        GuiText armourLabel;
        GuiText creditsLabel;
        GuiText xpLabel;

        public void AssociateGameManager(IGameManager game)
        {
            this.game = game;
        }

        protected override void DoLoadContent()
        {
            background = new GuiImage
            {
                ContentFile = "ScreenManager/FillImage",
                TintColour = Colour.Black
            };

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

            creditsLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(150, 24)
            };

            xpLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Aqua,
                Size = new Size2D(200, 24)
            };

            RegisterChildren(background, healthLabel, armourLabel, creditsLabel, xpLabel);
        }

        protected override void DoUnloadContent() { }

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

            background.Location = Point2D.Empty;
            background.Size = Size;

            healthLabel.Text = $"Health: {healthPercent}%";
            healthLabel.Location = new Point2D(4, 4);

            armourLabel.Text = $"Armour: {armourPercent}%";
            armourLabel.Location = new Point2D(160, 4);

            creditsLabel.Text = $"Credits: {player.Credits}";
            creditsLabel.Location = new Point2D(316, 4);

            xpLabel.Text = $"Level {player.Level}  XP: {player.Experience}/{player.ExperienceToNextLevel}";
            xpLabel.Location = new Point2D(472, 4);
        }

        protected override void DoDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) { }
    }
}
