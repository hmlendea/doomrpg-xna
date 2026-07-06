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

        static readonly float RotationRate = (float)(Math.PI / 2 / 0.5); // radians per second — full 90° in 500ms

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
        }

        public void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position = player.Position;

            if (pendingRotation != 0)
            {
                float maxStep = RotationRate * elapsedSeconds;
                float step;

                if (pendingRotation > 0)
                {
                    step = Math.Min(pendingRotation, maxStep);
                }
                else
                {
                    step = Math.Max(pendingRotation, -maxStep);
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

        void ApplyRotation(float angle)
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
