using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy_PointAttack : MonoBehaviour
{
    private float _damage;
    private float _attackRange;
    private string _prefabPath;
    private Vector2 _targetPos;
    private float _speed;

    public void Init(float damage, float attackRange, float speed, Vector2 targetPos, string prefabPath)
    {
        _damage = damage;
        _attackRange = attackRange;
        _speed = speed;
        _targetPos = targetPos;
        _prefabPath = prefabPath;
        MoveToTarget().Forget();
    }

    private async UniTaskVoid MoveToTarget()
    {
        while (Vector2.Distance(transform.position, _targetPos) > 0.1f)
        {
            Vector2 dir = (_targetPos - (Vector2)transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _speed * Time.deltaTime);

            //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, 0, angle);

            await UniTask.Yield();
        }

        Explode();
    }

    private void Explode()
    {
        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, _attackRange, LayerMask.GetMask("Player"));
        foreach(var player in players)
        {
            PlayerUnit_Base playerBase = player.GetComponent<PlayerUnit_Base>();
            if(playerBase != null)
            {
                playerBase.TakeDamage(_damage);
            }

            ObjectPoolManager.Instance.ReturnObject(_prefabPath, gameObject);
        }
    }
}
