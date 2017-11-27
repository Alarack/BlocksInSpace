using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetingWeapon : MonoBehaviour {

    public enum TargetingState {
        Tracking,
        Locking,
        Firing
    }


    [Header("Targeting Mob Info")]
    public GameObject reticle;
    public float trackSpeed;
    public float fireDelay;
    public float fireDuration;
    public LayerMask whatIsTarget;

    public TargetingState state;

    private GameObject myReticle;
    private LookAtTarget lookScript;

    private Reticle reticleScript;
    private BasicMovement reticleMovementScript;
    private Weapon myWeapon;

    void Start() {
        myWeapon = GetComponent<Weapon>();
        if (myWeapon == null)
            myWeapon = GetComponentInChildren<Weapon>();

        lookScript = GetComponent<LookAtTarget>();
        CreateReticle();
    }

    void Update() {
        if (GetComponentInParent<Health>().isDead) {
            Destroy(myReticle);
        }

        lookScript.target = myReticle.transform.position;

        if(GetComponentInParent<EntityPlayer>() != null || reticleScript.myTarget == null) {
            List<Transform> potentalTargets = TargetUtils.FindAllTargets(transform, lookScript.targetRadius, lookScript.whatIsTarget);
            List<GameObject> newTargets = new List<GameObject>();
            foreach (Transform t in potentalTargets) {
                newTargets.Add(t.gameObject);
            }

            if(potentalTargets.Count < 1) {
                myWeapon.autoFire = false;
            }

            int rand = Random.Range(0, newTargets.Count);

            if(newTargets[rand] == null) {
                newTargets.RemoveAt(rand);
                return;
            }
            else {
                reticleScript.myTarget = newTargets[rand];
            }

            
        }

        switch (state) {

            case TargetingState.Tracking:
                Tracking();

                break;

            case TargetingState.Locking:
                StartCoroutine(Locking());

                break;

            case TargetingState.Firing:
                StartCoroutine(Firing());

                break;
        }
    }

    void Tracking() {
        //reticleScript.targetAquired = false;
        myWeapon.autoFire = false;
        //reticleScript.lockedOn = false;
        reticleMovementScript.direction = BasicMovement.Direction.Directed;

        if (Vector3.Distance(myReticle.transform.position, reticleScript.myTarget.transform.position) <= 0.5f) {
            state = TargetingState.Locking;
        }
    }

    IEnumerator Locking() {
        reticleMovementScript.direction = BasicMovement.Direction.Still;
        reticleScript.LockOnTarget();

        if (reticleScript.lockedOn) {
            yield return new WaitForSeconds(fireDelay);
            state = TargetingState.Firing;
        }
    }

    IEnumerator Firing() {
        myWeapon.autoFire = true;
        yield return new WaitForSeconds(fireDuration);
        if(reticleScript != null)
            reticleScript.UnlockOnTarget();

        if (!reticleScript.lockedOn) {
            state = TargetingState.Tracking;
        }
    }

    void CreateReticle() {
        GameObject activeReticle = Instantiate(reticle, transform.position, transform.rotation) as GameObject;
        //activeReticle.transform.SetParent(transform, true);
        myReticle = activeReticle;
        reticleScript = activeReticle.GetComponent<Reticle>();
        reticleScript.myTarget = FindObjectOfType<EntityPlayer>().gameObject;

        reticleMovementScript = activeReticle.GetComponent<BasicMovement>();
        reticleMovementScript.speed = trackSpeed;
    }
}
