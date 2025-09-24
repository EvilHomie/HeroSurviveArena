using DI;
using UnityEngine;

namespace GameSystem
{
    public class GameManager : MonoBehaviour
    {
        GameEventBus _eventBus;

        [Inject]
        public void Construct(GameEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Start()
        {
            _eventBus.ChangeGameState?.Invoke(GameState.Init);
        }
    }
}

