using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit_Boss : MonoBehaviour
{
    [SerializeField] private string _projectilePath = "Prefabs/Enemy/Enemy_Prefab/Enemy_Projectile_Eclipse";
    [SerializeField] private string _pointAttackPath = "Prefabs/Enemy/Enemy_Prefab/Enemy_PointAttack_Eclipse";

    private EnemyUnitData _enemyData;
    private Transform _target;
    private bool _isAttacking = false;

    private List<string> _attackCycle = new List<string>();
    private List<string> _remainingAttacks = new List<string>();

    public void Init(EnemyUnitData enemyData, Transform transform)
    {
        _enemyData = enemyData;
        _target = transform;

        var projectile = GetComponent<EnemyUnit_Projectile>();
        if (projectile != null )
        {
            projectile.SetProjectilePath( _projectilePath );
        }
        var pointAttack = GetComponent<EnemyUnit_PointAttack>();
        if( pointAttack != null )
        {
            pointAttack.SetPointAttackPath( _pointAttackPath );
        }

        BuildAttackCycle();
        StartBossPattern().Forget();
    }

    private void BuildAttackCycle()
    {
        _attackCycle.Clear();
        if (GetComponent<EnemyUnit_Instance>() != null) 
        {
            _attackCycle.Add("Instance");
            _attackCycle.Add("Dash");
        } 
        if (GetComponent<EnemyUnit_Projectile>() != null) _attackCycle.Add("Projectile");
        if (GetComponent<EnemyUnit_PointAttack>() != null) _attackCycle.Add("PointAttack");
    }

    private void ShuffleCycle()
    {
        _remainingAttacks = new List<string> (_attackCycle);
        for (int i = _remainingAttacks.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            string temp = _remainingAttacks[i];
            _remainingAttacks[i] = _remainingAttacks[j];
            _remainingAttacks[j] = temp;
        }
    }

    private async UniTaskVoid StartBossPattern()
    {
        while (true)
        {
            if (_remainingAttacks.Count == 0)
                ShuffleCycle();

            string nextAttack = _remainingAttacks[0];
            _remainingAttacks.RemoveAt(0);

            DoAttack(nextAttack);

            await UniTask.Delay(TimeSpan.FromSeconds(_enemyData.AttackCoolDown));

            if (gameObject.activeSelf == false) return;
        }
    }

    private void DoAttack(string attackType)
    {
        GetComponent<EnemyUnit_AnimationController>()?.SetBossState(attackType);
        switch (attackType)
        {
            case "Instance":
                GetComponent<EnemyUnit_Instance>()?.DoAttack(); 
                break;
            case "Dash":
                GetComponent<EnemyUnit_Instance>()?.DashAttack();
                break;
            case "Projectile":
                GetComponent<EnemyUnit_Projectile>()?.FireSpreadProjectile(10, 75f);
                break;
            case "PointAttack":
                GetComponent<EnemyUnit_PointAttack>()?.FirePointAttack();
                break;
        }
    }
}
