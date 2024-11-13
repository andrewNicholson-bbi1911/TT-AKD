using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BaseItem : MonoBehaviour
{
    public ItemSpotTransformLink ActualItemSpotLink { get => _actualItemSpot; }

    public const float DEFAULT_INTERRACTION_RANGE = 4f;
    public static readonly Vector3 PlayerOffset = new Vector3(0, 1.3f, 0);

    public bool IsBusy { get => _actualItemSpot != null; }

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _sellectionMaterial;

    [SerializeField] public UnityAction onSellected;
    [SerializeField] public UnityAction onDesellected;

    private ItemSpotTransformLink _actualItemSpot;


    private void OnValidate()
    {
        _meshRenderer ??= GetComponent<MeshRenderer>();
    }


    public bool TryUpdateItemSpot(ItemSpotTransformLink itemSpot)
    {
        if(_actualItemSpot != null && _actualItemSpot != itemSpot)
        {
            if (!_actualItemSpot.Contains(this) && ((itemSpot?.Contains(this)??true) || (itemSpot?.Contains(null)??true)))
            {
                _actualItemSpot = itemSpot;
                return true;
            }
            else
            {
                return false;
            }
        }

        _actualItemSpot = itemSpot;
        return true;
    }


    public bool UpdateSellection(bool selected, Transform takerPosition)
    {
        if (selected && IsObjectInRange(takerPosition))
        {
            _meshRenderer.SetMaterials(new() { _meshRenderer.materials[0], _sellectionMaterial });
            onSellected?.Invoke();
            return true;
        }
        else
        {
            _meshRenderer.SetMaterials(new() { _meshRenderer.materials[0]});
            onDesellected?.Invoke();
            return false;
        }
    }


    private bool IsObjectInRange(Transform takerPosition)
        => (transform.position - (takerPosition.position + PlayerOffset))
        .magnitude <= DEFAULT_INTERRACTION_RANGE;
    
}
