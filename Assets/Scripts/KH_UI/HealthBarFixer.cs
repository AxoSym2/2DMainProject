using UnityEngine;

public class HealthBarFixer : MonoBehaviour
{
    private void LateUpdate()
    {
        if(transform.lossyScale.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -scale.x;
            transform.localScale = scale;
        }
    }
}
