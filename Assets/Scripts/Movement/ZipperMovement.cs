using UnityEngine;
using System.Collections;

public class ZipperMovement : BasicMovement {

    [Header("Zipper Movement Info")]
    public float zipDelay;
    public float moveDuration;
    [Header("Zipper Projectile's Spin Rate")]
    public float spinForce;

    private float moveTimer;
    private bool targetAquired;

    protected override void Start() {
        base.Start();

        if (spinForce != 0f)
            myBody.angularVelocity = Random.Range(-spinForce, spinForce);
    }

    protected override void Move() {
        base.Move();

        if (moveTimer <= moveDuration) {
            moveTimer += Time.deltaTime;

            if (!targetAquired) {
                AquireTarget();
                direction = Direction.Directed;
            }
        }
        else if (targetAquired) {
            StartCoroutine(ZipReset());
        }
    }

    void AquireTarget() {
        target = Random.insideUnitCircle * speed;
        targetAquired = true;
    }

    IEnumerator ZipReset() {
        targetAquired = false;
        direction = Direction.Still;
        yield return new WaitForSeconds(zipDelay);
        moveTimer = 0f;
    }
}