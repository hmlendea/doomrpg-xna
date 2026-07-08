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
        GuiText strengthLabel;
        GuiText agilityLabel;
        GuiText accuracyLabel;
        GuiText defenseLabel;
        GuiText statPointsLabel;
        GuiText weaponLabel;
        GuiText turnLabel;
        GuiText bulletClipLabel;
        GuiText shellClipLabel;
        GuiText rocketLabel;
        GuiText cellClipLabel;
        GuiText halonCanLabel;

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

            strengthLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Red,
                Size = new Size2D(96, 20)
            };

            agilityLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Green,
                Size = new Size2D(96, 20)
            };

            accuracyLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.SkyBlue,
                Size = new Size2D(96, 20)
            };

            defenseLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Orange,
                Size = new Size2D(96, 20)
            };

            statPointsLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Gold,
                Size = new Size2D(140, 20)
            };

            weaponLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.White,
                Size = new Size2D(200, 20)
            };

            turnLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.White,
                Size = new Size2D(160, 24)
            };

            bulletClipLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(148, 20)
            };

            shellClipLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(148, 20)
            };

            rocketLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(148, 20)
            };

            cellClipLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(148, 20)
            };

            halonCanLabel = new GuiText
            {
                FontName = "MenuFont",
                ForegroundColour = Colour.Yellow,
                Size = new Size2D(148, 20)
            };

            RegisterChildren(background, healthLabel, armourLabel, creditsLabel, xpLabel,
                strengthLabel, agilityLabel, accuracyLabel, defenseLabel, statPointsLabel, weaponLabel, turnLabel,
                bulletClipLabel, shellClipLabel, rocketLabel, cellClipLabel, halonCanLabel);
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

            turnLabel.Text = $"Turn: {game.GetTurnNumber()}";
            turnLabel.Location = new Point2D(680, 4);

            strengthLabel.Text = $"STR: {player.Strength}";
            strengthLabel.Location = new Point2D(4, 28);

            agilityLabel.Text = $"AGI: {player.Agility}";
            agilityLabel.Location = new Point2D(100, 28);

            accuracyLabel.Text = $"ACC: {player.Accuracy}";
            accuracyLabel.Location = new Point2D(196, 28);

            defenseLabel.Text = $"DEF: {player.Defense}";
            defenseLabel.Location = new Point2D(292, 28);

            statPointsLabel.Text = $"Points: {player.StatPoints}";
            statPointsLabel.Location = new Point2D(388, 28);

            DoomRPG.Models.Weapon equippedWeapon = game.GetEquippedWeapon();

            if (equippedWeapon is not null)
            {
                weaponLabel.Text = $"[{equippedWeapon.Name}]";
            }
            else
            {
                weaponLabel.Text = "[Unarmed]";
            }

            weaponLabel.Location = new Point2D(540, 28);

            player.AmmoCounts.TryGetValue("bullet_clip", out int bulletClips);
            player.AmmoCounts.TryGetValue("shell_clip", out int shellClips);
            player.AmmoCounts.TryGetValue("rocket", out int rockets);
            player.AmmoCounts.TryGetValue("cell_clip", out int cellClips);
            player.AmmoCounts.TryGetValue("halon_can", out int halonCans);

            bulletClipLabel.Text = $"Bullets: {bulletClips}";
            bulletClipLabel.Location = new Point2D(4, 52);

            shellClipLabel.Text = $"Shells: {shellClips}";
            shellClipLabel.Location = new Point2D(152, 52);

            rocketLabel.Text = $"Rockets: {rockets}";
            rocketLabel.Location = new Point2D(300, 52);

            cellClipLabel.Text = $"Cells: {cellClips}";
            cellClipLabel.Location = new Point2D(448, 52);

            halonCanLabel.Text = $"Halon: {halonCans}";
            halonCanLabel.Location = new Point2D(596, 52);
        }

        protected override void DoDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch) { }
    }
}
