using Enemy;

namespace GameSystem
{
    public class EnemiesPool : AbstractPool<AbstractEnemy>
    {
        protected override void Subscribe()
        {
            _gameEventBus.EnemyDie += OnItemDeactivated;
            _gameEventBus.ChangeGameState += OnChangeGameState;
            _gameFlowSystem.UpdateTick += ReleaseInactive;
        }

        protected override void Unsubscribe()
        {
            _gameEventBus.EnemyDie -= OnItemDeactivated;
            _gameEventBus.ChangeGameState -= OnChangeGameState;
            _gameFlowSystem.UpdateTick -= ReleaseInactive;
        }

        private void OnChangeGameState(GameState gameState)
        {
            if (gameState == GameState.GameOver || gameState == GameState.Victory)
            {
                ReleaseAll();
            }
        }
    }
}



/* просто заметка
* var index = Random.Range(0, list.Count);
var item = list[index];

// Меняем местами с последним элементом
list[index] = list[list.Count - 1];

// Удаляем последний элемент
list.RemoveAt(list.Count - 1);
*/
