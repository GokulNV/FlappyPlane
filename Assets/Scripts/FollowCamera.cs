using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    /// <summary>
    /// The speed at which the camera follows its target.
    /// </summary>
    [SerializeField] private float _speed;

    /// <summary>
    /// The point of view (POV) transform of the camera.
    /// </summary>
    [SerializeField] private Transform _cameraPov;

    /// <summary>
    /// The transform of the airplane that the camera follows.
    /// </summary>
    [SerializeField] private Transform _airplane;

    /// <summary>
    /// The transform of this camera.
    /// </summary>
    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
    }

    /// <summary>
    /// Moves the camera towards the camera point of view and adjusts its rotation to look at the airplane.
    /// </summary>
    private void LateUpdate()
    {
        _myTransform.position = Vector3.MoveTowards(_myTransform.position, _cameraPov.position, Time.deltaTime * _speed);
        _myTransform.LookAt(_airplane.position);
    }
}
