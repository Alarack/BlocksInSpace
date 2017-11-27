using UnityEngine;
using System.Collections;

public class GalaMovement : BasicMovement {

    [Header("GalaMovement Info")]
    public float loopDuration;
    public float loopInterval;
    public float loopDegree;
    public Transform loopPoint;
    [Space(10)]
    public bool strafing;

    private bool isLooping = false;
    private float loopTimer;

    protected override void Start() {
        base.Start();
        if (transform.position.x > 0) {
            SwapRotation();
        }
    }

    protected override void Move() {
        base.Move();

        if (isLooping) {
            if (strafing)
                GetComponent<EntityEnemy>().autoFire = true;
            Loop();
        }

        else {
            if (strafing)
                GetComponent<EntityEnemy>().autoFire = false;
            RewindLoop();
        }
    }

    void RewindLoop() {
        loopTimer += Time.deltaTime;
        if (loopTimer >= loopInterval) {
            isLooping = true;
            loopTimer = loopDuration;

            if (strafing)
                SwapRotation();
        }
    }

    void Loop() {
        if (loopPoint.localPosition.x < 0f) {
            transform.RotateAround(loopPoint.position, Vector3.forward, loopDegree * Time.deltaTime);
            loopTimer -= Time.deltaTime;

        }
        else if (loopPoint.localPosition.x > 0f) {
            transform.RotateAround(loopPoint.position, Vector3.forward, -loopDegree * Time.deltaTime);
            loopTimer -= Time.deltaTime;
        }

        if (loopTimer <= 0)
            isLooping = false;
    }

    public void SwapRotation() {
        loopPoint.transform.localPosition = new Vector2(-loopPoint.localPosition.x, -loopPoint.transform.localPosition.y);
    }
}