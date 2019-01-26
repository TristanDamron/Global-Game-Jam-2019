using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Yoyo : MonoBehaviour
{
    public float length = 5.0f;
    public float speed = 1.0f;
    public float spinSpeed = 10.0f;
    public float playerPullSpeedInitial = 5.0f;
    public float catchPlayerDistance = 1.0f;
    public MO_PlayerController player;
    public MO_Yoyo_Target target;

    public Transform yoyoSpinTarget;

    Vector3 goalPosition;
    Vector3 goalDirection;

    Vector3 offsetFromPlayer;

    Vector3 holdPosition;

    public YoyoState yoyoState = YoyoState.IDLE;

    private float spinDirection;
    private float playerPullSpeed;
    private Vector3 catchOffset;
    private LineRenderer lineRenderer;

    private SpringJoint2D springJoint;

    public enum YoyoState
    {
        LAUNCHED,
        PULL,
        RELEASE,
        IDLE
    }

    private void Start()
    {
        target.lengthToTarget = length;
        yoyoState = YoyoState.IDLE;
        offsetFromPlayer = transform.position - player.transform.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        switch (yoyoState)
        {
            case YoyoState.IDLE:
                IdleUpdate();
                break;
            case YoyoState.LAUNCHED:
                LaunchedUpdate();
                break;
            case YoyoState.PULL:
                PullUpdate();
                break;
            case YoyoState.RELEASE:
                ReleaseYoyo();
                break;
        }
    }

    public void Launch()
    {
        if (yoyoState == YoyoState.IDLE)
        {
            goalPosition = target.transform.position;
            goalDirection = transform.position - goalPosition;
            goalPosition = transform.position - (goalDirection.normalized * length);
            yoyoState = YoyoState.LAUNCHED;
            playerPullSpeed = playerPullSpeedInitial;

            if (goalPosition.x < player.transform.position.x)
                spinDirection = 1;
            else
                spinDirection = -1;
        }
    }

    private void IdleUpdate()
    {
        transform.position = player.transform.position + offsetFromPlayer;
    }

    private void LaunchedUpdate()
    {
        DrawYoyoString();
        Debug.DrawRay(goalPosition, Vector3.up, Color.red);
        // move towards goal position
        float frameSpeed = speed * Time.deltaTime;
        float distanceFromGoal = Vector3.Distance(transform.position, goalPosition);
        if (distanceFromGoal > frameSpeed)
            transform.position -= goalDirection.normalized * frameSpeed;
        else
        {
            yoyoSpinTarget.position = player.transform.position;
            yoyoState = YoyoState.PULL;
        }
    }

    private void PullUpdate()
    {
        Spin();
        DrawYoyoString();
        // so this basically pulls in the spin target, which is spun because it's
        // a child of yoyo, then sets the player position to the same spot.
        // this isn't going to play nicely with physics though
        Vector3 forceDirection = yoyoSpinTarget.position - transform.position;
        yoyoSpinTarget.position -= forceDirection.normalized * Time.fixedDeltaTime * playerPullSpeed;
        player.transform.position = yoyoSpinTarget.position;
        Debug.DrawRay(yoyoSpinTarget.position, Vector3.up, Color.green);

        if (Vector3.Distance(
            yoyoSpinTarget.position,
            transform.position) <= catchPlayerDistance)
        {
            yoyoState = YoyoState.RELEASE;
        }
    }

    private void ReleaseYoyo()
    {
        lineRenderer.enabled = false;
        player.QuitYoyo();
        yoyoState = YoyoState.IDLE;
    }

    private void Spin()
    {
        transform.Rotate(Vector3.forward, spinSpeed * spinDirection * Time.deltaTime);
    }

    private void DrawYoyoString()
    {
        if (!lineRenderer.enabled) lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, player.transform.position + offsetFromPlayer);
        lineRenderer.SetPosition(1, transform.position);
    }
}
