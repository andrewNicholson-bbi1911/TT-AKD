using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseItemManipulator : MonoBehaviour
{
    [SerializeField] private Transform _raycastOriginTransform;
    [Space]
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private SortingLayer sortingLayer;
    [Space]
    [SerializeField] private BaseItemSpot _manipulatorSpot;

    private BaseItem _sellectingItem = null;
    private BaseItemSpot _potentialSpot = null;


    public void UpdateSelections()
    {
        if (Physics.Raycast(_raycastOriginTransform.position, _raycastOriginTransform.forward, out RaycastHit hit, _layerMask))
        {
            if (hit.collider.TryGetComponent(out BaseItem item) && item.UpdateSellection(true, transform))
            {
                if(_sellectingItem != item)
                {
                    _sellectingItem?.UpdateSellection(false, transform);
                    _sellectingItem = item;
                }
            }
            else
            {
                _sellectingItem?.UpdateSellection(false, transform);
                _sellectingItem = null;
            }

            if(hit.collider.TryGetComponent(out BaseItemSpot itemSpot))
            {
                _potentialSpot = itemSpot;
            }
            else
            {
                _potentialSpot = null;
            }
        }
        else
        {
            _sellectingItem?.UpdateSellection(false, transform);
            _sellectingItem = null;
            _potentialSpot = null;
        }
    }


    public void TryUpdateItem()
    {
        if(_sellectingItem != null && _manipulatorSpot.EmptySpots > 0)
        {
            TryTakeItem();
        }
        else if(_sellectingItem != null && _manipulatorSpot.EmptySpots == 0)
        {
            TryReplaceItem();
        }
        else if(_potentialSpot != null && _manipulatorSpot.IsBusy)
        {
            TryPutItem();
        }
    }


    private void TryPutItem()
    {
        if(_manipulatorSpot.TryTakeAnyItem(out var item))
        {
            if ( _potentialSpot.TryPlaceItem(item))
            {
                Debug.Log($"{this}>>>item put successfully");
            }
            else
            {
                Debug.LogWarning($"{this}>>> current item can't be put at chosen spot");
                _manipulatorSpot.TryPlaceItem(item);
            }
        }
        else
        {
            Debug.LogWarning($"{this}>>>item cant be put");
        }
    }


    private void TryReplaceItem()
    {
        var lastSelectingItemSpot = _sellectingItem.ActualItemSpotLink;
        if(_manipulatorSpot.TryTakeAnyItem(out var replacingItem))
        {
            if (lastSelectingItemSpot.TryTakeItem(_sellectingItem))
            {
                if (_manipulatorSpot.TryPlaceItem(_sellectingItem))
                {
                    if (lastSelectingItemSpot.TryPlaceItem(replacingItem))
                    {
                        _sellectingItem.UpdateSellection(false, transform);
                        _sellectingItem = null;
                        Debug.Log($"{this}>>>items succcessfully replaced");
                    }
                    else
                    {
                        _manipulatorSpot.TryTakeItem(_sellectingItem);
                        lastSelectingItemSpot.TryPlaceItem(_sellectingItem);
                        _manipulatorSpot.TryPlaceItem(replacingItem);
                        Debug.LogWarning($"{this}>>>replacingItem can't be put at manipulator spot");
                    }
                }
                else
                {
                    lastSelectingItemSpot.TryPlaceItem(_sellectingItem);
                    _manipulatorSpot.TryPlaceItem(replacingItem);
                    Debug.LogWarning($"{this}>>>sellected item can't be taken");
                }
            }
            else
            {
                _manipulatorSpot.TryPlaceItem(replacingItem);
                Debug.LogWarning($"{this}>>>sellected item can't be taken");
            }
        }
        else
        {
            Debug.LogWarning($"{this}>>>item from manipulatot spot can't be taken");
        }
    }


    private void TryTakeItem()
    {
        var lastSelectingItemSpot = _sellectingItem.ActualItemSpotLink;
        if (lastSelectingItemSpot?.TryTakeItem(_sellectingItem)??true)
        {
            if (_manipulatorSpot.TryPlaceItem(_sellectingItem))
            {
                _sellectingItem.UpdateSellection(false, transform);
                _sellectingItem = null;
                Debug.Log($"{this}>>>item taken successfully");
            }
            else
            {
                lastSelectingItemSpot?.TryPlaceItem(_sellectingItem);
                Debug.LogWarning($"{this}>>>item cant be taken to actual manipulator spot");
            }
        }
        else
        {
            Debug.LogWarning($"{this}>>>item cant be taken");
        }
    }
}
