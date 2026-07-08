using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NuciXNA.Gui;
using NuciXNA.Gui.Controls;
using NuciXNA.Gui.Screens;
using NuciXNA.Graphics.Drawing;
using NuciXNA.Input;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers;
using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Gui.GuiElements;
using DoomRPG.Models.Enumerations;
using DoomRPG.Settings;

namespace DoomRPG.Gui.Screens
{
    /// <summary>
    /// Gameplay screen.
    /// </summary>
    public class GameplayScreen : Screen
    {
        IGameManager game;
        GuiCameraView cameraView;
        GuiStatusBar statusBar;
        GuiText notificationLabel;

        int notificationTimer;
        int turnAtNotificationStart;

        int previousScrollWheelValue;

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void DoLoadContent()
        {
            game = new GameManager();
            cameraView = new GuiCameraView();
            statusBar = new GuiStatusBar();
            notificationLabel = new GuiText
            {
                FontName = "LargeFont",
                HorizontalAlignment = Alignment.Middle,
                Text = string.Empty
            };

            GuiManager.Instance.RegisterControls(cameraView, statusBar, notificationLabel);

            game.LoadContent();

            cameraView.AssociateGameManager(game);
            statusBar.AssociateGameManager(game);

            KeyPressed += OnKeyPressed;

            previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;

            SetChildrenProperties();
        }

        protected override void DoUnloadContent()
        {
            KeyPressed -= OnKeyPressed;

            game.UnloadContent();
        }

        protected override void DoUpdate(GameTime gameTime)
        {
            game.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            if (notificationTimer > 0)
            {
                notificationTimer -= (int)(gameTime.ElapsedGameTime.TotalMilliseconds);

                if (notificationTimer <= 0 || game.GetTurnNumber() != turnAtNotificationStart)
                {
                    notificationTimer = 0;
                    notificationLabel.Text = string.Empty;
                }
            }

            int currentScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            int scrollDelta = currentScrollWheelValue - previousScrollWheelValue;

            if (scrollDelta > 0)
            {
                game.CycleWeaponNext();
            }
            else if (scrollDelta < 0)
            {
                game.CycleWeaponPrevious();
            }

            previousScrollWheelValue = currentScrollWheelValue;

            SetChildrenProperties();
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
        }

        void ShowNotification(string text, Colour colour)
        {
            notificationLabel.Text = text;
            notificationLabel.ForegroundColour = colour;
            notificationTimer = 1000;
            turnAtNotificationStart = game.GetTurnNumber();
        }

        void SetChildrenProperties()
        {
            int viewHeight = ScreenManager.Instance.Size.Height - GameDefines.StatusBarHeight;

            cameraView.Size = new Size2D(ScreenManager.Instance.Size.Width, viewHeight);

            statusBar.Location = new Point2D(0, viewHeight);
            statusBar.Size = new Size2D(ScreenManager.Instance.Size.Width, GameDefines.StatusBarHeight);

            notificationLabel.Location = new Point2D(0, viewHeight / 2 - 30);
            notificationLabel.Size = new Size2D(ScreenManager.Instance.Size.Width, 60);
        }

        void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Up || e.Key == Keys.W)
            {
                game.MovePlayer(MovementDirection.North);
            }
            else if (e.Key == Keys.Down || e.Key == Keys.S)
            {
                game.MovePlayer(MovementDirection.South);
            }
            else if (e.Key == Keys.Left || e.Key == Keys.A)
            {
                float angle = (float)(Math.PI / 2);
                game.RotatePlayer(angle);
                cameraView.camera.Rotate(angle);
            }
            else if (e.Key == Keys.Right || e.Key == Keys.D)
            {
                float angle = -(float)(Math.PI / 2);
                game.RotatePlayer(angle);
                cameraView.camera.Rotate(angle);
            }
            else if (e.Key == Keys.Space)
            {
                bool attacked = game.Attack();

                if (!attacked)
                {
                    ShowNotification("Not enough ammo!", Colour.ChromeYellow);
                }
                else
                {
                    var weapon = game.GetEquippedWeapon();

                    if (weapon is not null && !string.IsNullOrEmpty(weapon.AmmunitionId))
                    {
                        game.GetPlayer().AmmoCounts.TryGetValue(weapon.AmmunitionId, out int remaining);

                        if (remaining == 1)
                        {
                            ShowNotification("Last shot!", Colour.ChromeYellow);
                        }
                        else if (remaining == 2)
                        {
                            ShowNotification("2 shots left!", Colour.ChromeYellow);
                        }
                    }
                }
            }
            else
            {
                int slot = (int)e.Key - (int)Keys.D0;

                if (slot >= 1 && slot <= 9)
                {
                    game.SelectWeaponBySlot(slot);
                }
            }
        }
    }
}
