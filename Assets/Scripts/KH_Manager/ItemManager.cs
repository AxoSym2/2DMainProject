using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; set; }

    private List<ICollectable> _activeItems = new List<ICollectable>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterItem(ICollectable item)
    {
        if (_activeItems.Contains(item) == false)
        {
            _activeItems.Add(item);
        }
    }

    public void UnregisterItem(ICollectable item)
    {
        _activeItems.Remove(item);
    }

    public void AttractAllItems(Transform playerTransform)
    {
        for (int i = _activeItems.Count - 1; i >= 0; i--)
        {
            if (_activeItems[i] != null)
            {
                _activeItems[i].MoveToPlayer(playerTransform);
            }
        }
    }

    public void ClearAllItems()
    {
        _activeItems.Clear();
    }
}
