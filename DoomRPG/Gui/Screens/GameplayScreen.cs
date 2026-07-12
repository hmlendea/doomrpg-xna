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
using DoomRPG.Models;
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
        GuiDialogue dialogueBox;

        int notificationTimer;
        int turnAtNotificationStart;
        bool dialogueVisible;

        int previousScrollWheelValue;
        MouseState previousMouseState;

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

            dialogueBox = new GuiDialogue();

            GuiManager.Instance.RegisterControls(cameraView, statusBar, notificationLabel, dialogueBox);

            game.LoadContent();

            cameraView.AssociateGameManager(game);
            statusBar.AssociateGameManager(game);

            KeyPressed += OnKeyPressed;

            previousScrollWheelValue = Mouse.GetState().ScrollWheelValue;
            previousMouseState = Mouse.GetState();

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
                notificationTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (notificationTimer <= 0 || game.GetTurnNumber() != turnAtNotificationStart)
                {
                    notificationTimer = 0;
                    notificationLabel.Text = string.Empty;
                }
            }

            MouseState currentMouseState = Mouse.GetState();

            int currentScrollWheelValue = currentMouseState.ScrollWheelValue;
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

            int viewHeight = ScreenManager.Instance.Size.Height - GameDefines.StatusBarHeight;
            bool clickInView = currentMouseState.Y < viewHeight;

            if (currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                clickInView)
            {
                PerformAttack();
            }

            if (currentMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                previousMouseState.RightButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                clickInView)
            {
                PerformInteraction();
            }

            previousMouseState = currentMouseState;

            SetChildrenProperties();
        }

        protected override void DoDraw(SpriteBatch spriteBatch)
        {
        }

        private void ShowDialogue(string text)
        {
            dialogueBox.Show(text);
            dialogueVisible = true;
        }

        private void DismissDialogue()
        {
            dialogueBox.Hide();
            dialogueVisible = false;
        }

        private void ShowNotification(string text, Colour colour)
        {
            notificationLabel.Text = text;
            notificationLabel.ForegroundColour = colour;
            notificationTimer = 1000;
            turnAtNotificationStart = game.GetTurnNumber();
        }

        private void SetChildrenProperties()
        {
            int viewHeight = ScreenManager.Instance.Size.Height - GameDefines.StatusBarHeight;

            cameraView.Size = new Size2D(ScreenManager.Instance.Size.Width, viewHeight);

            statusBar.Location = new Point2D(0, viewHeight);
            statusBar.Size = new Size2D(ScreenManager.Instance.Size.Width, GameDefines.StatusBarHeight);

            notificationLabel.Location = new Point2D(0, viewHeight / 2 - 30);
            notificationLabel.Size = new Size2D(ScreenManager.Instance.Size.Width, 60);

            int dialogueBoxHeight = 120;

            dialogueBox.Location = new Point2D(0, viewHeight - dialogueBoxHeight);
            dialogueBox.Size = new Size2D(ScreenManager.Instance.Size.Width, dialogueBoxHeight);
        }

        private void PerformInteraction()
        {
            if (dialogueVisible)
            {
                DismissDialogue();
                return;
            }

            DoorInteractionResult doorResult = game.InteractWithDoor();

            if (doorResult.WasDoorFound)
            {
                if (!string.IsNullOrEmpty(doorResult.ErrorMessage))
                {
                    ShowNotification(doorResult.ErrorMessage, Colour.Red);
                }
                else if (!string.IsNullOrEmpty(doorResult.DestinationLevelId))
                {
                    game.ChangeLevel(doorResult.DestinationLevelId);
                    cameraView.ReloadLevel();
                }

                return;
            }

            string text = game.InteractWithTerminal();

            if (text is null)
            {
                text = game.InteractWithMob();
            }

            if (!string.IsNullOrEmpty(text))
            {
                ShowDialogue(text);
            }
        }

        private void PerformAttack()
        {
            AttackResult result = game.Attack();

            switch (result.Outcome)
            {
                case AttackOutcome.NoAmmo:
                    ShowNotification("Not enough ammo!", Colour.ChromeYellow);
                    break;

                case AttackOutcome.NoTarget:
                    break;

                case AttackOutcome.Missed:
                    ShowNotification($"Missed!", Colour.White);
                    break;

                case AttackOutcome.Hit:
                    ShowNotification($"Hit {result.MobName} for {result.Damage} damage!", Colour.White);
                    HandleAmmoLowNotification(result);
                    break;

                case AttackOutcome.Kill:
                    ShowNotification($"{result.Damage} damage! {result.MobName} died!", Colour.Green);
                    HandleAmmoLowNotification(result);
                    break;

                case AttackOutcome.Critical:
                    ShowNotification($"Crit! {result.Damage} damage!", Colour.Orange);
                    HandleAmmoLowNotification(result);
                    break;

                case AttackOutcome.CriticalKill:
                    ShowNotification($"Crit! {result.Damage} damage! {result.MobName} died!", Colour.Orange);
                    HandleAmmoLowNotification(result);
                    break;

                case AttackOutcome.WorldObjectHit:
                    ShowNotification($"Hit {result.WorldObjectName} for {result.Damage} damage!", Colour.White);
                    HandleAmmoLowNotification(result);
                    break;

                case AttackOutcome.WorldObjectDestroyed:
                    HandleWorldObjectDestroyedNotification(result);
                    HandleAmmoLowNotification(result);
                    break;
            }
        }

        private void HandleAmmoLowNotification(AttackResult result)
        {
            Weapon weapon = game.GetEquippedWeapon();

            if (weapon is null || string.IsNullOrEmpty(weapon.AmmunitionId))
            {
                return;
            }

            if (result.RemainingAmmunition == 0)
            {
                ShowNotification("Last shot!", Colour.ChromeYellow);
            }
            else if (result.RemainingAmmunition == 1)
            {
                ShowNotification("1 shot left!", Colour.ChromeYellow);
            }
            else if (result.RemainingAmmunition == 2)
            {
                ShowNotification("2 shots left!", Colour.ChromeYellow);
            }
        }

        private void HandleWorldObjectDestroyedNotification(AttackResult result)
        {
            if (result.ExplosionDamageDealtToPlayer > 0)
            {
                ShowNotification($"{result.WorldObjectName} exploded! You took {result.ExplosionDamageDealtToPlayer} damage!", Colour.Red);
            }
            else
            {
                ShowNotification($"{result.WorldObjectName} exploded!", Colour.Orange);
            }
        }

        private void HandleMoveResult(MoveResult result)
        {
            if (!string.IsNullOrEmpty(result.PickedUpObjectName))
            {
                string pickupMessage = $"Picked up {result.PickedUpObjectName}!";

                if (result.HealAmountReceived > 0)
                {
                    pickupMessage += $" +{result.HealAmountReceived} HP";
                }

                ShowNotification(pickupMessage, string.IsNullOrEmpty(result.PickedUpKeyId) ? Colour.Green : Colour.Yellow);
            }
        }

        private void OnKeyPressed(object sender, KeyboardKeyEventArgs e)
        {
            if (e.Key == Keys.Up || e.Key == Keys.W)
            {
                MoveResult moveResult = game.MovePlayer(MovementDirection.North);
                HandleMoveResult(moveResult);
            }
            else if (e.Key == Keys.Down || e.Key == Keys.S)
            {
                MoveResult moveResult = game.MovePlayer(MovementDirection.South);
                HandleMoveResult(moveResult);
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
                PerformAttack();
            }
            else if (e.Key == Keys.E)
            {
                PerformInteraction();
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
