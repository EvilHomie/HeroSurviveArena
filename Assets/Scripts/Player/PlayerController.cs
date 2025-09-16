using DI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerInput _input;
    private float _moveSpeed;

    [Inject]
    public void Constructor(IPlayerInput playerInput, Config config)
    {
        _input = playerInput;
        _moveSpeed = config.PlayerSpeed;
        ChangeSubscribes(true);
    }

    private void OnDestroy()
    {
        ChangeSubscribes(false);
    }

    private void OnRotate(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnMove(Vector3 direction)
    {
        transform.Translate(_moveSpeed * Time.deltaTime * direction, Space.World);
    }   
    private void OnAttack()
    {
        Debug.Log("START ATACK");
    }

    private void OnEndAttack()
    {
        Debug.Log("END ATACK");
    }

    private void ChangeSubscribes(bool isSubscribing)
    {
        if (isSubscribing)
        {
            _input.MoveAction += OnMove;
            _input.RotateAction += OnRotate;
            _input.StartAtackAction += OnAttack;
            _input.EndAtackAction += OnEndAttack;
        }
        else
        {
            _input.MoveAction -= OnMove;
            _input.RotateAction -= OnRotate;
            _input.StartAtackAction -= OnAttack;
            _input.EndAtackAction -= OnEndAttack;
        }
    }
}
