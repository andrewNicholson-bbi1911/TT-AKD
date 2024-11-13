using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryCheck : MonoBehaviour
{
    [SerializeField] private BaseItemSpot _pickupSpot;
    [SerializeField] private UnityEvent _onVictory;


    public void CheckVictory()
    {
        if(_pickupSpot.EmptySpots <= 0)
        {
            _onVictory.Invoke();
        }
    }
}
