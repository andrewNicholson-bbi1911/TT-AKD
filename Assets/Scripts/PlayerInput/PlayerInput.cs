using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private BaseItemManipulator _baseItemManipulator;
    [SerializeField] private float _defaultPlayerSpeed = 4f;

    private void OnValidate()
    {
        _characterController ??= GetComponent<CharacterController>();
    }


    private void Update()
    {
        UpdateMovment();
        _baseItemManipulator.UpdateSelections();
        if (Input.GetMouseButtonDown(0))
        {
            _baseItemManipulator.TryUpdateItem();
        }
    }


    private void UpdateMovment()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");


        var localTranslation = new Vector3(horizontal, 0, vertical) * _defaultPlayerSpeed * Time.deltaTime;
        var globlTranlation = transform.localToWorldMatrix * localTranslation;

        _characterController.Move(globlTranlation);
    }
}
