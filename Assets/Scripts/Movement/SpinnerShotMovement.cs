using UnityEngine;
using System.Collections;

public class SpinnerShotMovement : BasicMovement {

    [Header("SpinnerShot Movement Info")]
    public float moveDuration;
    public float spinSpeed;
    public float maxSpin;

    private SpawnerShot mySpawner;

    protected override void Start() {
        base.Start();

        mySpawner = GetComponent<SpawnerShot>();
        mySpawner.beginSpawning = false;

        float moveVar = Random.Range(-0.5f, 0.5f);
        moveDuration += moveVar;
    }

    protected override void Move() {
        if (moveDuration > 0f) {
            moveDuration -= Time.deltaTime;
            base.Move();
        }
        else {
            myBody.velocity = Vector2.zero;
            if (myBody.angularVelocity < maxSpin)
                myBody.angularVelocity += spinSpeed;
        }

        if (myBody.angularVelocity >= maxSpin && !mySpawner.beginSpawning) {
            mySpawner.beginSpawning = true;
        }
    }
}