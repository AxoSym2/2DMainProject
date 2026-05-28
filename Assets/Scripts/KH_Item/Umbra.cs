using UnityEngine;

public class Umbra : MonoBehaviour, ICollectable
{
    private int _amount;

    public void Init(int amount)
    {
        _amount = amount;
    }

    public void OnEnable()
    {
        ItemManager.Instance.RegisterItem(this);
    }

    public void OnDisable()
    {
        ItemManager.Instance.UnregisterItem(this);
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

    public void MoveToPlayer(Transform playerTransform)
    {
        StartCoroutine(MoveCoroutine(playerTransform));
    }

    private System.Collections.IEnumerator MoveCoroutine(Transform playerTransform)
    {
        while (Vector2.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 10f * Time.deltaTime);
            yield return null;
        }
    }
}
