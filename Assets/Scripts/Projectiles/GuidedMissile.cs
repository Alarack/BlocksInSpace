using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LookAtTarget))]
public class GuidedMissile : Projectile {

    [Header("Basic Guide Stats")]
    public float trackTime;
    public float trackDelay;
    [Header("Drunk Stuff")]
    public bool drunken;
    public float drunkTime;

    private float drunkTimer;
    private bool tracked = false;
    private LookAtTarget lookScript;

    protected override void Start() {
        base.Start();
        lookScript = GetComponent<LookAtTarget>();
        lookScript.activateLookAt = false;

        if (drunken) {
            trackTime *= 3f;
            lookScript.inaccuracy *= 25;
            trackDelay /= 3;
        }
        StartCoroutine(Tracking());
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (drunken) {
            DrunkenBehaviour();
        }

        if (tracked && lookScript.target != Vector3.zero) {
            lookScript.activateLookAt = true;
        }
        else {
            lookScript.activateLookAt = false;
        }
    }

    IEnumerator Tracking() {
        yield return new WaitForSeconds(trackDelay);
        if (!playerShot)
           lookScript.target = FindObjectOfType<EntityPlayer>().transform.position;
        else {
           lookScript.target = TargetUtils.FindNearestTarget(transform.position, TargetUtils.FindAllTargets(transform, lookScript.targetRadius, lookScript.whatIsTarget));
        }

        if (lookScript.target != Vector3.zero) {
            tracked = true;
            yield return new WaitForSeconds(trackTime);
            tracked = false;
        }

        GetComponent<EntityMovement>().speed *= 3;
    }

    void DrunkenBehaviour() {
        drunkTimer -= Time.deltaTime;
        if (drunkTimer <= 0) {
            lookScript.error = Random.Range(-lookScript.inaccuracy, lookScript.inaccuracy); ;
            drunkTimer = drunkTime;
        }
    }
}