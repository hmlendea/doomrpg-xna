using System;

using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

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
                Position = new PointF2D(3, 4)
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
            PointF2D newPosition = player.Position;
            PointF2D movement = PointF2D.Empty;

            switch (direction)
            {
                case MovementDirection.North:
                    movement = new PointF2D(
                        player.Direction.X * player.MovementSpeed,
                        player.Direction.Y * player.MovementSpeed);
                    break;

                case MovementDirection.West:
                    movement = new PointF2D(
                        -player.Direction.Y * player.MovementSpeed,
                        -player.Direction.X * player.MovementSpeed);
                    break;

                case MovementDirection.South:
                    movement = new PointF2D(
                        -player.Direction.X * player.MovementSpeed,
                        -player.Direction.Y * player.MovementSpeed);
                    break;

                case MovementDirection.East:
                    movement = new PointF2D(
                        player.Direction.Y * player.MovementSpeed,
                        player.Direction.X * player.MovementSpeed);
                    break;
            }

            if (movement.X != 0)
            {
                WallInstance wall = levelManager.GetWall((int)(player.Position.X + movement.X), (int)player.Position.Y);

                if (wall == null)
                {
                    newPosition.X += movement.X;
                }
            }

            if (movement.Y != 0)
            {
                WallInstance wall = levelManager.GetWall((int)player.Position.X, (int)(player.Position.Y + movement.Y));

                if (wall == null)
                {
                    newPosition.Y += movement.Y;
                }
            }

            player.Position = newPosition;
        }

        public Player GetPlayer()
        {
            return player;
        }

        private void Rotate(float amount)
        {
            float sinRot = (float)Math.Sin(amount);
            float cosRot = (float)Math.Cos(amount);
            
            player.Direction = new PointF2D(
                player.Direction.X * cosRot - player.Direction.Y * sinRot,
                player.Direction.X * sinRot + player.Direction.Y * cosRot);
        }
    }
}
