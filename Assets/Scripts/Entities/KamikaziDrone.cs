using UnityEngine;
using System.Collections;

public class KamikaziDrone : Drone {

    public float detonationDistance;
    public bool boomtest;

    private LookAtTarget lookscript;


    protected override void Start() {
        base.Start();

        lookscript = GetComponent<LookAtTarget>();
    }

    protected override void Update() {
        base.Update();

        if(TargetUtils.FindDistance(lookscript.target, transform.position) < detonationDistance && boomtest) {
            Debug.Log("Go for boom");
            GloriousEnd();
        }
    }

    void GloriousEnd() {
        activePrimary.GetComponent<Weapon>().Fire();
        GetComponent<Health>().KillEntity();
    }

}