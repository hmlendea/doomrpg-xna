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
        }
        
        public void Update(GameTime gameTime)
        {
            Vector2 move = Vector2.Zero;
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Direction = player.Direction;
            Position = player.Position;
        }

        public void AssociateGameManager(IGameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        
        private void UpdateRotation(float amount)
        {
            float sinRot = (float)Math.Sin(amount);
            float cosRot = (float)Math.Cos(amount);

            float opX = Plane.X;
            Plane = new PointF2D(
                Plane.X * cosRot - Plane.Y * sinRot,
                Plane.X * sinRot + Plane.Y * cosRot);
        }
    }
}
