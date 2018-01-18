using DoomRPG.GameLogic.GameManagers.Interfaces;
using DoomRPG.Models;
using DoomRPG.Models.Enumerations;

namespace DoomRPG.GameLogic.GameManagers
{
    public class GameManager : IGameManager
    {
        readonly ILevelManager levelManager;
        readonly IPlayerManager playerManager;

        public GameManager()
        {
            levelManager = new LevelManager();
            playerManager = new PlayerManager(levelManager);
        }

        public void LoadContent()
        {
            levelManager.LoadContent("test"); // TODO: Remove hardcoding
            playerManager.LoadContent();
        }

        public void UnloadContent()
        {
            levelManager.UnloadContent();
            playerManager.UnloadContent();
        }

        public void Update(float elapsedSeconds)
        {
            levelManager.Update(elapsedSeconds);
            playerManager.Update(elapsedSeconds);
        }

        public void MovePlayer(MovementDirection direction)
        {
            playerManager.SetPlayerMovementDirection(direction);
        }

        public WallInstance GetWall(int x, int y)
        {
            return levelManager.GetWall(x, y);
        }
    }
}
