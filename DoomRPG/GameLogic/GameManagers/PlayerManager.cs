using System;

using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;
using DoomRPG.Settings;

namespace DoomRPG.GameLogic.GameManagers
{
    public class PlayerManager : IPlayerManager
    {
        ILevelManager levelManager;

        Player player;

        public PlayerManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;

            player = new Player
            {
                Position = new PointF2D(3.5f, 4.5f),
                Health = GameDefines.PlayerStartingHealth,
                MaxHealth = GameDefines.PlayerStartingMaxHealth
            };
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }
        
        public void Update(float elapsedSeconds)
        {

        }

        public void MovePlayer(MovementDirection direction)
        {
            int dirX = (int)Math.Round(player.Direction.X);
            int dirY = (int)Math.Round(player.Direction.Y);

            int dx = 0;
            int dy = 0;

            switch (direction)
            {
                case MovementDirection.North:
                    dx = dirX;
                    dy = dirY;
                    break;

                case MovementDirection.South:
                    dx = -dirX;
                    dy = -dirY;
                    break;

                case MovementDirection.West:
                    dx = dirY;
                    dy = -dirX;
                    break;

                case MovementDirection.East:
                    dx = -dirY;
                    dy = dirX;
                    break;
            }

            float targetX = player.Position.X + dx;
            float targetY = player.Position.Y + dy;

            WallInstance wall = levelManager.GetWall((int)targetX, (int)targetY);

            if (wall is null)
            {
                player.Position = new PointF2D(targetX, targetY);
            }
        }

        public void RotatePlayer(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            player.Direction = new PointF2D(
                player.Direction.X * cos - player.Direction.Y * sin,
                player.Direction.X * sin + player.Direction.Y * cos);
        }

        public void ApplyDamage(int amount)
        {
            player.Health -= amount;

            if (player.Health < 0)
            {
                player.Health = 0;
            }
        }

        public void Heal(int amount)
        {
            player.Health += amount;

            if (player.Health > player.MaxHealth)
            {
                player.Health = player.MaxHealth;
            }
        }

        public Player GetPlayer()
        {
            return player;
        }
    }
}
