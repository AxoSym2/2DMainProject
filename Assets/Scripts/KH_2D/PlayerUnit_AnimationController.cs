using UnityEngine;

public enum PlayerUnitState
{
    None,
    Idle,
    Run,
    Die,
    Hit
}

public class PlayerUnit_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator Animator_playerUnit;
    private PlayerUnitState _currentState;

    public void SetState(PlayerUnitState state)
    {
        if (_currentState == state) return;
        _currentState = state;
        switch (_currentState) 
        {
            case PlayerUnitState.Idle:
                Animator_playerUnit.SetBool("IsMoving", false);
                break;
            case PlayerUnitState.Run:
                Animator_playerUnit.SetBool("IsMoving", true);
                break;
            case PlayerUnitState.Hit:
                Animator_playerUnit.SetTrigger("Hit");
                break;
            case PlayerUnitState.Die:
                Animator_playerUnit.SetTrigger("Die");
                break;
        }
    }

    public void SetDirection(Vector2 moveInput)
    {
        Animator_playerUnit.SetFloat("MoveX", moveInput.x);
        Animator_playerUnit.SetFloat("MoveY", moveInput.y);
    }
}
