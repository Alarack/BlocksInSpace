using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Carrier : EntityEnemy {

    [Header("Carrier Info")]
    public GameObject[] minions;
    public Transform[] minionStartPositions;
    public float spawnDelay;
    public float waveDelay;
    //public float activationDelay;
    public float minionSpeed;
    public float minionScaleMod = 1f;

    private List<GameObject> currentSpawns = new List<GameObject>();
    private bool[] checkPositions;
    private bool activeMinions;

    protected override void Start() {
        base.Start();
        autoFire = false;
        checkPositions = new bool[minionStartPositions.Length];
        StartCoroutine(SpawnMinions());

        foreach (Weapon w in fullWeaponList) {
            w.shotScale *= 2;
        }

    }

    void FixedUpdate() {
        //Check if all Minions are in their assigned positions. If not, move each minion to its assigned postion and flag the checkPositions array true for that minion.
        if (!EnumList.CheckBoolArray(checkPositions)) {
            for (int i = 0; i < currentSpawns.Count; i++) {
                AssignMinionLocations(currentSpawns[i], minionStartPositions[i]);

                if ((currentSpawns[i].transform.position == minionStartPositions[i].position)) {
                    checkPositions[i] = true;
                }
            }
        }
        // If all minions are at their assigned locations, check to see if it's time to activate them. If so, activate the minions.
        else if (!activeMinions) {
            StartCoroutine(ActivateMinions());
        }
        //Check if all the currently spawned minions are dead. If they are, set all the bools in the check possitions array to false, then spawn a new set of minions.
        if (CheckCurrentEnemies() && EnumList.CheckBoolArray(checkPositions)) {
            for (int i = 0; i < checkPositions.Length; i++) {
                checkPositions[i] = false;
            }

            StartCoroutine(SpawnMinions());
        }
    }//End of Fixed Update

    //Move a given minion to a specified location.
    void AssignMinionLocations(GameObject minion, Transform location) {
        minion.transform.position = Vector3.MoveTowards(minion.transform.position, location.position, 0.2f);
        minion.transform.rotation = location.rotation;
    }

    //Turn on each minion's Collider, set thier speed, and turn on their weapons.
    IEnumerator ActivateMinions() {
        activeMinions = true;
        autoFire = true;
        yield return new WaitForSeconds(1f);
        foreach (GameObject minion in currentSpawns) {
            minion.GetComponent<BasicMovement>().speed = minionSpeed;
            minion.GetComponent<EntityEnemy>().autoFire = true;
            minion.GetComponent<Collider2D>().enabled = true;

            if (minion.GetComponent<LookAtTarget>() != null)
                minion.GetComponent<LookAtTarget>().activateLookAt = true;
        }
        GetComponent<Collider2D>().enabled = true;
    }
    //Check if any given spawned enemy is dead. If an enemy is dead, remove it from the currentSpawns array. If all enemies are dead, activeMinions becomes false.
    public bool CheckCurrentEnemies() {
        for (int i = currentSpawns.Count - 1; i > -1; i--) {
            if (currentSpawns[i] == null)
                currentSpawns.RemoveAt(i);
        }
        if (currentSpawns.Count == 0) {
            activeMinions = false;
            autoFire = false;
            return true;
        }

        else
            return false;
    }
    //Spawn one minion for each each deginated point. Shut off each minion's Collider, weapons, and set their speed to 0 until it's time to activate them.
    IEnumerator SpawnMinions() {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(waveDelay);
        for (int i = 0; i < minionStartPositions.Length; i++) {
            int spawnIndex = Random.Range(0, minions.Length);
            GameObject activeMinion = Instantiate(minions[spawnIndex], transform.position, transform.rotation) as GameObject;

            EnumList.Colors randomColor = (EnumList.Colors)Random.Range(0, 3);
            activeMinion.GetComponent<EntityEnemy>().unitColor = randomColor;

            activeMinion.GetComponent<BasicMovement>().speed = 0f;
            activeMinion.GetComponent<EntityEnemy>().autoFire = false;
            activeMinion.GetComponent<EntityEnemy>().shootDown = false;
            activeMinion.GetComponent<Collider2D>().enabled = false;
            activeMinion.transform.localScale *= minionScaleMod;

            if (activeMinion.GetComponent<LookAtTarget>() != null)
                activeMinion.GetComponent<LookAtTarget>().activateLookAt = false;


            currentSpawns.Add(activeMinion);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}