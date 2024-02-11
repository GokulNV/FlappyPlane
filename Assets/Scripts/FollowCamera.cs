using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cameraPov;
    [SerializeField] private Transform _airplane;

    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
    }

    private void LateUpdate()
    {
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, _cameraPov.position, Time.deltaTime * _speed);
        _myTransform.LookAt(_airplane.position);
    }
}