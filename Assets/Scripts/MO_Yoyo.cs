using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MO_Yoyo : MonoBehaviour
{
    public float length = 5.0f;
    public float speed = 1.0f;
    // public float spinSpeed = 10.0f;
    public float maxVelocity = 14f;
    public float lateralDistance = 0.5f;
    // public float playerPullSpeedInitial = 5.0f;
    public PlayerController player;
    public MO_Yoyo_Target target;

    public Transform yoyoSpinTarget;

    Vector3 goalPosition;
    Vector3 goalDirection;

    Vector3 offsetFromPlayer;

    Vector3 holdPosition;

    public YoyoState yoyoState = YoyoState.IDLE;

    private float spinDirection;
    // private float playerPullSpeed;
    private Vector3 catchOffset;
    private LineRenderer lineRenderer;

    // so to get the trajectory right the player will actually push toward a 
    // position above the yoyo
    public float actualTargetOffset = 10.0f;
    public float yoyoGravity = 5.0f;

    private Vector3 actualTarget;
    private ParticleSystem _particles;

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
        target.gameObject.SetActive(true);
        _particles = GetComponent<ParticleSystem>();
    }

    private void Update()
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
                Release();
                player.QuitYoyo();
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
            target.gameObject.SetActive(false);
            // playerPullSpeed = playerPullSpeedInitial;

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
        DrawYoyoString();

        Vector3 forceDirection = actualTarget - player.transform.position;
        float playerDistance = forceDirection.magnitude;

        Vector3 direction = actualTarget - player.transform.position;
        direction = Vector3.Cross(direction, Vector3.forward * spinDirection);
        direction = direction.normalized;
        actualTarget = transform.position + (direction * playerDistance * lateralDistance);
        // always pull player toward us
        // Vector3 forceDirection = transform.position - player.transform.position;
        Debug.DrawLine(actualTarget, transform.position, Color.red);
        Vector3 force = forceDirection.normalized * yoyoGravity;
        Debug.DrawRay(player.transform.position, force, Color.grey);
        player.rb_.AddForce(force);
        if (player.rb_.velocity.magnitude > maxVelocity) {
            player.rb_.velocity = player.rb_.velocity.normalized * maxVelocity;            
        } else {
            transform.Rotate(Vector3.up * Time.deltaTime * 100000f);            
            _particles.Play();                    
        }

    }

    public void Release()
    {
        _particles.Stop();
        lineRenderer.enabled = false;
        // player.QuitYoyo();
        target.gameObject.SetActive(true);
        yoyoState = YoyoState.IDLE;
    }

    private void Spin()
    {
        transform.Rotate(Vector3.forward, 1000f * spinDirection * Time.deltaTime);
    }

    private void DrawYoyoString()
    {
        if (!lineRenderer.enabled) lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, player.transform.position + offsetFromPlayer);
        lineRenderer.SetPosition(1, transform.position);        
    }
}
