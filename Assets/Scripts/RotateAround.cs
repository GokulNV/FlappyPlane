using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _pivotObject;

    private Transform _myTransform;

    private void Awake()
    {
        _myTransform = transform;
    }

    private void Update()
    {
        _myTransform.RotateAround(_pivotObject.position, -Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
