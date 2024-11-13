using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    [SerializeField] private Transform _doorModelTransform;
    [SerializeField] private bool _opened = false;
    [SerializeField] private float _openingTime = 2f;
    [SerializeField] private float _openedScaleY = 1;
    [SerializeField] private float _closedScaleY = 4.5f;


    public void UpdateOpenState()
    {
        _opened = !_opened;
        StopAllCoroutines();
        StartCoroutine(UpdateScale());
    }


    private IEnumerator UpdateScale()
    {
        float timeSpent = 0;

        var endState = _opened ? _openedScaleY : _closedScaleY;
        var delta = _closedScaleY - _openedScaleY;

        while (timeSpent <= _openingTime)
        {
            yield return null;
            timeSpent += Time.deltaTime;
            var value = !_opened
                ? (_openedScaleY + (timeSpent / _openingTime) * delta)
                : (_closedScaleY - (timeSpent / _openingTime) * delta);

            _doorModelTransform.localScale = new Vector3(1, Mathf.Clamp(value, _openedScaleY, _closedScaleY) ,1);
        }

        _doorModelTransform.localScale = new Vector3(1, endState, 1);
    }
}
