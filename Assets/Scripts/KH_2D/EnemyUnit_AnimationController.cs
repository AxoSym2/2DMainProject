using UnityEngine;

public enum EnemyUnitState
{
    None = 0,
    Run,
    Attack,
    Dead
}

public class EnemyUnit_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator Animator_enemyUnit;
    private EnemyUnitState _currentState;

    public void SetState(EnemyUnitState state)
    {
        if (_currentState == state) return;
        _currentState = state;
        switch (_currentState)
        {
            case EnemyUnitState.Run:
                Animator_enemyUnit.SetBool("IsMoving", true);
                break;
            case EnemyUnitState.Attack:
                Animator_enemyUnit.SetTrigger("Attack");
                break;
            case EnemyUnitState.Dead:
                Animator_enemyUnit.SetTrigger("Dead");
                break;
        }
    }
    
    public void SetDirection(Vector2 moveInput)
    {
        Animator_enemyUnit.SetFloat("MoveX", moveInput.x);
        Animator_enemyUnit.SetFloat("MoveY", moveInput.y);
    }
}
