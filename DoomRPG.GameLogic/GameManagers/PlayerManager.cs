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

        public Player Player { get; set; }

        public PlayerManager(ILevelManager levelManager)
        {
            this.levelManager = levelManager;
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
            PointF2D newPosition = Player.Position;

            if (movement.X != 0)
            {
                WallInstance wall = levelManager.GetWall((int)(Player.Position.X + movement.X), (int)Player.Position.Y);

                if (wall == null)
                {
                    newPosition.X += movement.X;
                }
            }

            if (movement.Y != 0)
            {
                WallInstance wall = levelManager.GetWall((int)Player.Position.X, (int)(Player.Position.Y + movement.Y));

                if (wall == null)
                {
                    newPosition.Y += movement.Y;
                }
            }

            Player.Position = newPosition;
        }

        void SetPlayerMovementDirection(MovementDirection direction)
        {
            Player.MovementDirection = direction;
        }

        private PointF2D GetMovement(float elapsedSeconds)
        {
            switch (Player.MovementDirection)
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
                        Player.Direction.X * (elapsedSeconds * Player.MovementSpeed),
                        Player.Direction.Y * (elapsedSeconds * Player.MovementSpeed));

                case MovementDirection.West:
                    throw new NotImplementedException();

                case MovementDirection.South:
                    return new PointF2D(
                        -Player.Direction.X * (elapsedSeconds * Player.MovementSpeed),
                        -Player.Direction.Y * (elapsedSeconds * Player.MovementSpeed));

                case MovementDirection.East:
                    throw new NotImplementedException();

                default:
                    return PointF2D.Empty;
            }
        }
    }
}
