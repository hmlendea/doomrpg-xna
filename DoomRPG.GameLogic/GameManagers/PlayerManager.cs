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

            player = new Player();
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }
        
        public void Update(float elapsedSeconds)
        {
            PointF2D movement = GetMovement(elapsedSeconds);
            PointF2D newPosition = player.Position;

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

        public void SetPlayerMovementDirection(MovementDirection direction)
        {
            player.MovementDirection = direction;
        }

        public Player GetPlayer()
        {
            return player;
        }

        private PointF2D GetMovement(float elapsedSeconds)
        {
            switch (player.MovementDirection)
            {
                case MovementDirection.NorthWest:
                    throw new NotImplementedException();

                case MovementDirection.NorthEast:
                    throw new NotImplementedException();

                case MovementDirection.SouthWest:
                    throw new NotImplementedException();

                case MovementDirection.SouthEast:
                    throw new NotImplementedException();

                case MovementDirection.North:
                    return new PointF2D(
                        player.Direction.X * (elapsedSeconds * player.MovementSpeed),
                        player.Direction.Y * (elapsedSeconds * player.MovementSpeed));

                case MovementDirection.West:
                    throw new NotImplementedException();

                case MovementDirection.South:
                    return new PointF2D(
                        -player.Direction.X * (elapsedSeconds * player.MovementSpeed),
                        -player.Direction.Y * (elapsedSeconds * player.MovementSpeed));

                case MovementDirection.East:
                    throw new NotImplementedException();

                default:
                    return PointF2D.Empty;
            }
        }
    }
}
