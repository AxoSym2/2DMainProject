using UnityEngine;

public class PlayerUnit_Move : DaniTechUIBase
{
    [SerializeField] private float _moveSpeed = 5f;
    private Rigidbody2D _rb;
    private PlayerUnit_AnimationController _animController;
    private Vector2 _moveInput;
    private Vector2 _lastMoveDir = Vector2.down;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animController = GetComponent<PlayerUnit_AnimationController>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        if (_moveInput != Vector2.zero)
        {
            _lastMoveDir = _moveInput;
            _animController.SetState(PlayerUnitState.Run);
            _animController.SetDirection(_moveInput);
        }
        else 
        {
            _animController.SetState(PlayerUnitState.Idle);
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
    }

    public Vector2 GetLastMoveDir()
    {
        return _lastMoveDir;
    }
}
