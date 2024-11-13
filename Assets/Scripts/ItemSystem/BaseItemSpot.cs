using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseItemSpot : MonoBehaviour
{
    public static float DEFAULT_INTERRACTION_RANGE => BaseItem.DEFAULT_INTERRACTION_RANGE;
    public static Vector3 PlayerOffset => BaseItem.PlayerOffset;

    public bool IsBusy { get => _spotLinks.Find((x) => x.IsBusy) != null; }
    public int EmptySpots { get => _spotLinks.FindAll((x) => !x.IsBusy).Count; }

    [SerializeField] private List<ItemSpotTransformLink> _spotLinks;

    [SerializeField] private UnityEvent _onNewItemPlaced;


    private void OnValidate()
    {
        foreach(var link in _spotLinks)
        {
            link.UpdateItemPosition();
        }
    }


    public bool TryPlaceItem(BaseItem item)
    {
        foreach(var spotLink in _spotLinks)
        {
            if (spotLink.TryPlaceItem(item))
            {
                _onNewItemPlaced.Invoke();
                return true;
            }
        }
        return false;
    }


    public bool TryTakeItem(BaseItem item)
    {
        foreach (var spotLink in _spotLinks)
        {
            if (spotLink.TryTakeItem(item))
            {
                return true;
            }
        }
        return false;
    }


    public bool TryTakeAnyItem(out BaseItem item)
    {
        foreach(var spotLink in _spotLinks)
        {
            if(spotLink.TryTakeItem(out item))
            {
                return true;
            }
        }

        item = null;
        return false;
    }


    private void OnDrawGizmos()
    {
        foreach(var link in _spotLinks)
        {
            if (link.IsBusy)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(link.Position, 0.15f);
        }
    }
}



[System.Serializable]
public class ItemSpotTransformLink
{
    public Vector3 Position { get => _spotTransform.position; }
    public bool IsBusy { get => _actualItem != null; }

    [SerializeField] private Transform _spotTransform;
    [SerializeField] private BaseItem _actualItem;


    public void UpdateItemPosition()
    {
        if(_actualItem != null)
        {
            _actualItem.TryUpdateItemSpot(this);
            _actualItem.transform.parent = _spotTransform;
            _actualItem.transform.localPosition = Vector3.zero;
            _actualItem.transform.localRotation = Quaternion.identity;
        }
    }


    public bool Contains(BaseItem item) => item == _actualItem;


    public bool TryPlaceItem(BaseItem item)
    {

        if (IsBusy || !item.TryUpdateItemSpot(this))
            return false;

        _actualItem = item;

        UpdateItemPosition();
        return true;
    }


    public bool TryTakeItem(BaseItem item)
    {
        if(_actualItem == item && _actualItem != null)
        {
            _actualItem.transform.parent = null;
            _actualItem = null;
            item.TryUpdateItemSpot(null);
            return true;
        }

        return false;
    }


    public bool TryTakeItem(out BaseItem item)
    {
        if(_actualItem != null)
        {
            item = _actualItem;
            _actualItem.transform.parent = null;
            _actualItem = null;
            item.TryUpdateItemSpot(null);
            return true ;
        }

        item = null;
        return false;
    }
}
