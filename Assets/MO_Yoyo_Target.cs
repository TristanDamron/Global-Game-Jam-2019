using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Yoyo_Target : MonoBehaviour
{
    [System.NonSerialized]
    public float lengthToTarget = 5.0f;
    // Update is called once per frame
    void Update()
    {
        Vector3 goalPosition = transform.position;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane screenPlane = new Plane(Vector3.forward, Vector3.zero);
        float distance = 0;
        if (screenPlane.Raycast(ray, out distance))
            goalPosition = ray.GetPoint(distance);

        Vector3 parentPosition = transform.parent.transform.position;
        Vector3 goalDirection = goalPosition - parentPosition;
        // Debug.DrawRay(goalPosition, Vector3.up, Color.red);
        // Debug.DrawRay(parentPosition, Vector3.up, Color.blue);
        Debug.DrawRay(parentPosition, goalDirection, Color.grey);
        Debug.DrawRay(parentPosition, goalDirection.normalized * lengthToTarget);
        transform.position = parentPosition + goalDirection.normalized * lengthToTarget;
    }
}
