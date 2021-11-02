using UnityEngine;

public class GunDirection : MonoBehaviour
{
    private static float MAX_ANGLE = 30f;
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            if (angle >= MAX_ANGLE) angle = MAX_ANGLE;
            //else if (angle < -MAX_ANGLE) angle = -MAX_ANGLE;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else
        {
            Debug.Log(angle);
            if (angle - Mathf.Sign(angle) * 180 <= -MAX_ANGLE) angle = 180 - MAX_ANGLE;
            //if (angle - Mathf.Sign(angle) * 180 > MAX_ANGLE) angle = -180 + MAX_ANGLE;
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -angle));
        }

    }
}
