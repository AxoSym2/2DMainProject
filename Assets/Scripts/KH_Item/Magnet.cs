using UnityEngine;

public class Magnet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUnit_Base player = collision.GetComponent<PlayerUnit_Base>();
        if (player != null )
        {
            ItemManager.Instance.AttractAllItems(player.transform);
            ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/Magnet", gameObject);
        }
    }
}