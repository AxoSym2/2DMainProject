using UnityEngine;

public class Camera_Tracking : MonoBehaviour
{
    [SerializeField] private float _smoothSpeed = 5f;
    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if(_target == null) return;
        Vector3 targetPos = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, _smoothSpeed * Time.deltaTime);
    }
}
