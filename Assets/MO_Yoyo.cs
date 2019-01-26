using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Yoyo : MonoBehaviour
{
    public float length = 5.0f;
    public float speed = 1.0f;
    public float spinSpeed = 10.0f;
    public float playerPullSpeedInitial = 5.0f;
    public float playerPullSpeedFalloff = 1.0f;
    public float spinTargetDistance = 2.0f;
    public MO_PlayerController player;
    public MO_Yoyo_Target target;

    public Transform yoyoSpinTarget;

    Vector3 goalPosition;
    Vector3 goalDirection;

    Vector3 offsetFromPlayer;

    public YoyoState yoyoState = YoyoState.IDLE;

    [SerializeField]
    private float playerPullSpeed;
    private Vector3 catchOffset;

    public enum YoyoState
    {
        LAUNCHED,
        SPIN_PULL,
        SPIN_CATCH,
        IDLE
    }

    private void Start()
    {
        yoyoState = YoyoState.IDLE;
        offsetFromPlayer = transform.position - player.transform.position;
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
            case YoyoState.SPIN_PULL:
                SpinPullUpdate();
                break;
            case YoyoState.SPIN_CATCH:
                SpinCatchUpdate();
                break;
        }
    }

    public void Launch()
    {
        if (yoyoState == YoyoState.IDLE)
        {
            goalPosition = target.transform.position;
            goalDirection = transform.position - goalPosition;
            yoyoSpinTarget.position = transform.position + (goalDirection.normalized * spinTargetDistance);
            yoyoState = YoyoState.LAUNCHED;
            playerPullSpeed = playerPullSpeedInitial;
        }
    }

    private void IdleUpdate()
    {
        transform.position = player.transform.position + offsetFromPlayer;
    }

    private void LaunchedUpdate()
    {
        Debug.DrawRay(goalPosition, Vector3.up, Color.red);
        // move towards goal position
        float frameSpeed = speed * Time.deltaTime;
        float distanceFromGoal = Vector3.Distance(transform.position, goalPosition);
        if (distanceFromGoal > frameSpeed)
            transform.position -= goalDirection.normalized * frameSpeed;
        else
        {
            transform.position = goalPosition;
            yoyoState = YoyoState.SPIN_PULL;
        }
    }

    private void SpinPullUpdate()
    {
        Spin();
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector3 forceDirection = yoyoSpinTarget.position - player.transform.position;
        rb.AddForce(forceDirection.normalized * playerPullSpeed);
        // playerPullSpeed -= playerPullSpeedFalloff * Time.deltaTime;
        // if (playerPullSpeed < 0) playerPullSpeed = 0;
        if (Vector3.Distance(transform.position, player.transform.position) <= spinTargetDistance)
        {
            yoyoState = YoyoState.SPIN_CATCH;
            catchOffset = yoyoSpinTarget.position - player.transform.position;
        }
        Debug.DrawRay(yoyoSpinTarget.position, Vector3.up, Color.green);
    }

    private void SpinCatchUpdate()
    {
        Spin();
        Debug.DrawRay(yoyoSpinTarget.position, Vector3.up, Color.green);
        player.transform.position = yoyoSpinTarget.position - offsetFromPlayer;
    }

    private void Spin()
    {
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
