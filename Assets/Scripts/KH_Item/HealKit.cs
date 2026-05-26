using System.Security.Cryptography;
using UnityEngine;

public class HealKit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUnit_Base player = collision.GetComponent<PlayerUnit_Base>();
        if(player != null )
        {
            player.Heal(100f);
            ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/HealKit", gameObject);
        }
    }
}
