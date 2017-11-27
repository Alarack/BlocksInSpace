using UnityEngine;
using System.Collections;

public class OrbitMaster : EntityEnemy {

    [Header("Orbit Master Info")]
    public GameObject[] minions;
    public Transform loopPoint;
    public float loopDegree;

    protected override void Awake() {
        base.Awake();
        foreach (GameObject minion in minions) {
            if (minion != null)
                minion.GetComponent<EntityEnemy>().unitColor = unitColor;
        }
    }

    protected override void Update() {
        base.Update();

        if (GetComponent<Health>().isDead) {
            KillMinions();
        }
    }

    void FixedUpdate() {
        foreach (GameObject minion in minions) {
            if (minion != null)
                minion.transform.RotateAround(loopPoint.position, Vector3.forward, loopDegree * Time.deltaTime);
        }
    }

    void KillMinions() {
        transform.DetachChildren();
        foreach (GameObject minion in minions) {
            if (minion != null) {
                minion.GetComponent<Health>().KillEntity();
            }
        }
    }
}