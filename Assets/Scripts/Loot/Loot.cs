using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loot : MonoBehaviour {

    public GameObject[] drops;
    public int dropChance;

    private EntityEnemy owner;

    void Start() {
        owner = GetComponent<EntityEnemy>();
    }

    public void DropLoot(Vector2 dropLocation, Quaternion dropRotation) {
        int dropIndex = Random.Range(0, drops.Length);

        GameObject activeDrop = Instantiate(drops[dropIndex], dropLocation, dropRotation) as GameObject;
        Rigidbody2D activeBody = activeDrop.GetComponent<Rigidbody2D>();

        if (activeDrop.GetComponent<ColorShipModulePickup>() != null && owner != null)
            activeDrop.GetComponent<ColorShipModulePickup>().color = owner.unitColor;
        else if (activeDrop.GetComponent<ColorShipModulePickup>() != null) {
            EnumList.Colors randomColor = (EnumList.Colors)Random.Range(0, 3);
            activeDrop.GetComponent<ColorShipModulePickup>().color = randomColor;
        }

        float xVar = Random.Range(-2f, 2f);
        float yVar = Random.Range(3f, 5f);
        float rotateVar = Random.Range(-200f, 200f);

        activeBody.velocity = new Vector2(xVar, yVar);
        activeBody.angularVelocity = rotateVar;
    }

    public bool CheckDrop() {
        int roll = Random.Range(0, 101);
        return roll < dropChance;
    }
}