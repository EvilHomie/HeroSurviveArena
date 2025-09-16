using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Scriptable Objects/Config")]
public class Config : ScriptableObject
{
    [SerializeField] float _playerSpeed;
    
    public float PlayerSpeed => _playerSpeed;
}
