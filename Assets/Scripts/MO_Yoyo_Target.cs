using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Yoyo_Target : MonoBehaviour
{
    [System.NonSerialized]
    public float lengthToTarget = 5.0f;
    // [SerializeField]
    // private float targetSpeed_;
    private Transform target_;


    void Start() {
        target_ = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goalPosition = transform.position;        

        // Only ray cast the mouse position if the joystick is not connected
        // if (Input.GetJoystickNames().Length == 0) {
            Ray ray;            
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Plane screenPlane = new Plane(Vector3.forward, Vector3.zero);
            float distance = 0;
            if (screenPlane.Raycast(ray, out distance))
                goalPosition = ray.GetPoint(distance);
            /*
        } else {
            // if joystick is connected... TODO BROKEN
            goalPosition = new Vector3(
                target_.position.x + Input.GetAxisRaw("Yoyo Aim Horizontal"),
                target_.position.y - Input.GetAxisRaw("Yoyo Aim Vertical"),
                target_.position.z);
        }
        */

        goalPosition.z = 0.0f;
        Vector3 parentPosition = transform.parent.transform.position;
        parentPosition.z = 0.0f;
        Vector3 goalDirection = goalPosition - parentPosition;
        goalDirection.z = 0.0f;
        Debug.DrawRay(goalPosition, Vector3.up, Color.red);
        Debug.DrawRay(parentPosition, Vector3.up, Color.blue);
        Debug.DrawRay(parentPosition, goalDirection, Color.grey);
        Debug.DrawRay(parentPosition, goalDirection.normalized * lengthToTarget);
        Vector3 position = parentPosition + goalDirection.normalized * lengthToTarget;
        position.z = 0.0f;
        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
