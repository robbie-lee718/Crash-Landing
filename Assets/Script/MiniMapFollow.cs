using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target;

    public float height = 40f;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 newPosition = target.position;
        newPosition.y += height;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
    }
}
