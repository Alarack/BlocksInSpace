using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneBay : Upgrade {

    public List<GameObject> drones = new List<GameObject>();
    public int maxDrones = 1;
    private List<GameObject> currentDrones = new List<GameObject>();

    void CreateDrone() {
        int rand = Random.Range(0, drones.Count);
        GameObject activeDrone = Instantiate(drones[rand], transform.position, transform.rotation) as GameObject;

        Drone droneScript = activeDrone.GetComponent<Drone>();
        droneScript.owner = GetComponent<Entity>();

        if (droneScript.owner != null) {
            droneScript.unitColor = droneScript.owner.GetColor();
            activeDrone.gameObject.tag = droneScript.owner.gameObject.tag;
            activeDrone.gameObject.layer = droneScript.owner.gameObject.layer;
        }
        currentDrones.Add(activeDrone);
    }

    public bool CheckCurrentDrones() {
        for (int i = currentDrones.Count - 1; i > -1; i--) {
            if (currentDrones[i] == null)
                currentDrones.RemoveAt(i);
        }
        if (currentDrones.Count < maxDrones) {
            return true;
        }
        else {
            return false;
        }
    }

    protected override void ApplyIntervalEffect() {
        if (CheckCurrentDrones() && !player.GetComponent<Health>().isDead) {
            CreateDrone();
        }
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<DroneBay>();
    }

    public override void StackEffect() {
        maxDrones += 1;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<DroneBay>();
        DroneBay activeDroneBay = target.gameObject.GetComponent<DroneBay>();
        activeDroneBay.drones = drones;
        activeDroneBay.currentDrones = currentDrones;
        activeDroneBay.maxDrones = maxDrones;
        activeDroneBay.upgradeIcon = upgradeIcon;
    }
}