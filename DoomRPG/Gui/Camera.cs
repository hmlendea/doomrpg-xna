using System;

using Microsoft.Xna.Framework;
using NuciXNA.Primitives;

using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;

namespace DoomRPG.Gui
{
    public class Camera
    {
        public PointF2D Position { get; set; }

        public PointF2D Direction { get; set; }

        public PointF2D Plane { get; set; }

        public float Radius { get; set; }

        public float MoveSpeed { get; set; }

        public float RotationSpeed { get; set; }

        public float FieldOfView { get; set; }

        public IGameManager gameManager;

        Player player;
        float pendingRotation;

        private static readonly float RotationRate = (float)(Math.PI / 2 / 0.5); // Radians per second — full 90° turn in 500 ms.
        private static readonly float MovementRate = 1.0f / 0.5f; // Tiles per second — 1 tile in 500 ms.

        public Camera()
        {
            Radius = 0.3f;
            MoveSpeed = 3f;
            RotationSpeed = 3f;
            FieldOfView = 0.66f;
        }

        public void LoadContent()
        {
            player = gameManager.GetPlayer();
            Direction = player.Direction;
            Position = player.Position;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float dx = player.Position.X - Position.X;
            float dy = player.Position.Y - Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            float maxStep = MovementRate * elapsedSeconds;

            if (distance <= maxStep)
            {
                Position = player.Position;
            }
            else
            {
                Position = new PointF2D(
                    Position.X + dx / distance * maxStep,
                    Position.Y + dy / distance * maxStep);
            }

            if (pendingRotation != 0)
            {
                float maxRotationStep = RotationRate * elapsedSeconds;
                float step;

                if (pendingRotation > 0)
                {
                    step = Math.Min(pendingRotation, maxRotationStep);
                }
                else
                {
                    step = Math.Max(pendingRotation, -maxRotationStep);
                }

                ApplyRotation(step);
                pendingRotation -= step;

                if (Math.Abs(pendingRotation) < 1e-6f)
                {
                    pendingRotation = 0;
                }
            }
        }

        public void AssociateGameManager(IGameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void Rotate(float amount)
        {
            pendingRotation += amount;
        }

        private void ApplyRotation(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            Direction = new PointF2D(
                Direction.X * cos - Direction.Y * sin,
                Direction.X * sin + Direction.Y * cos);

            Plane = new PointF2D(
                Plane.X * cos - Plane.Y * sin,
                Plane.X * sin + Plane.Y * cos);
        }
    }
}
