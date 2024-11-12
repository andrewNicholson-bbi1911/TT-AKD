using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _headTransform;
    [Space]
    [SerializeField] private float _rotationMultiplier = 1f;
    [SerializeField] private float _maxLookUpAngle = 80f;
    [SerializeField] private float _minLookUpAngle = -75f;
    [Space]
    [SerializeField] private bool _isActivated = true;


    public void Update()
    {
        if (_isActivated)
        {
            UpdateLookAtPoint();
        }
    }


    public void LateUpdate()
    {
        if (_isActivated)
        {
            UpdateLookAtPoint();
        }
    }


    private void OnApplicationPause(bool pause)
    {
        _isActivated = !pause;
        Cursor.lockState = _isActivated ? CursorLockMode.Locked : CursorLockMode.None;
    }


    private void UpdateLookAtPoint()
    {
        var mouseX = Input.GetAxis("Mouse X") * _rotationMultiplier;
        var mouseY = Input.GetAxis("Mouse Y") * _rotationMultiplier;
        Vector2 currentMousePosition = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        transform.Rotate(Vector3.up, mouseX);
        _headTransform.Rotate(Vector3.left, mouseY , Space.Self);

        var currentHeadRotationXValue = _headTransform.localRotation.eulerAngles.x;
        if(currentHeadRotationXValue > 180)
        {
            currentHeadRotationXValue = currentHeadRotationXValue - 360;
        }

        _headTransform.localRotation = Quaternion.Euler(Mathf.Clamp(currentHeadRotationXValue, _minLookUpAngle, _maxLookUpAngle), 0,0);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_headTransform.position, (_headTransform.position + _headTransform.forward * 4));
    }
}
