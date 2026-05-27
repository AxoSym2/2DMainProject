using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{
    private float _damage;
    private float _speed;
    private Vector2 _direction;
    private string _prefabPath;

    private void Update()
    {
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;
    }

    public void Init(float  damage, float speed, Vector2 direction, string prefabPath)
    {
        _damage = damage; 
        _speed = speed;
        _direction = direction.normalized;
        _prefabPath = prefabPath;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        AutoReturn().Forget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerUnit_Base player = collision.GetComponent<PlayerUnit_Base>();
        if (player != null)
        {
            player.TakeDamage(_damage);
            if (string.IsNullOrEmpty(_prefabPath) == false)
            {
                ObjectPoolManager.Instance.ReturnObject(_prefabPath, gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private async Cysharp.Threading.Tasks.UniTaskVoid AutoReturn()
    {
        await Cysharp.Threading.Tasks.UniTask.Delay(System.TimeSpan.FromSeconds(5f));
        if (gameObject.activeSelf)
        {
            ObjectPoolManager.Instance.ReturnObject(_prefabPath, gameObject);
        }
    }
}
