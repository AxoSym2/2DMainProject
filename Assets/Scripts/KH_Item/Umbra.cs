using UnityEngine;

public class Umbra : MonoBehaviour
{
    private int _amount;

    public void Init(int amount)
    {
        _amount = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUnit_Base player = collision.GetComponent<PlayerUnit_Base>();
        if (player != null)
        {
            DaniTechGameManager.Inst.AddUmbra(_amount);
            ObjectPoolManager.Instance.ReturnObject("Prefabs/Item/Umbra", gameObject);
        }
    }
}
