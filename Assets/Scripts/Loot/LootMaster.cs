using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[RequireComponent (typeof(Loot))]
public class LootMaster : MonoBehaviour {

    [System.Serializable]
    public class SubLoot {
        [Header("SubLoot Drop Info")]
        public GameObject[] drops;
        public int dropChance;

        private Entity myOwner;

        public void SetOwner(Entity owner) {
            myOwner = owner;
        }

        public void DropLoot(Vector2 dropLocation, Quaternion dropRotation) {
            int dropIndex = Random.Range(0, drops.Length);

            GameObject activeDrop = Instantiate(drops[dropIndex], dropLocation, dropRotation) as GameObject;
            Rigidbody2D activeBody = activeDrop.GetComponent<Rigidbody2D>();

            if (activeDrop.GetComponent<ColorShipModulePickup>() != null && myOwner != null)
                activeDrop.GetComponent<ColorShipModulePickup>().color = myOwner.unitColor;
            else if (activeDrop.GetComponent<ColorShipModulePickup>() != null) {
                activeDrop.GetComponent<ColorShipModulePickup>().color = EnumList.RandomColor();
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
    }//End of SubLoot Class

    [Header("MasterLoot Drop Info")]
    public int numDrops;
    public bool doubleDip = false;
    [Header("List of Drops")]
    public SubLoot[] masterDrops;

    private Entity owner;
    private List<int> dropChances = new List<int>();
    private SortedDictionary<int, SubLoot> dropDictionary = new SortedDictionary<int, SubLoot>();

    void Awake() {
        owner = GetComponent<Entity>();

        foreach (SubLoot l in masterDrops) {
            l.SetOwner(owner);
        }
    }

    void Start() {
        CheckLootChances();

        for (int i = 0; i < masterDrops.Length; i++) {
            dropDictionary.Add(masterDrops[i].dropChance, masterDrops[i]);
        }
    }

    void Update() {

        if (owner.GetComponent<Health>().isDead) {
            for (int i = 0; i < numDrops; i++) {
                foreach (KeyValuePair<int, SubLoot> entry in dropDictionary) {
                    if (dropChances.Count > 0) {
                        if (entry.Key == dropChances.Max() && entry.Value.CheckDrop()) {
                            entry.Value.DropLoot(transform.position, transform.rotation);

                            if (!doubleDip) //TODO: This needs more work. I not only need to prevent the entry from leaving, but I need to reduce it, so it won't always drop the same thing.
                                dropChances.Remove(entry.Key);

                            break;
                        }
                        else if (entry.Key == dropChances.Max() && !entry.Value.CheckDrop()) {
                            if (!doubleDip)
                                dropChances.Remove(entry.Key);

                            numDrops++;
                            break;
                        }
                    }
                }
            }
        }
    }//End of Update

    void CheckLootChances() {
        foreach (SubLoot l in masterDrops) {
            dropChances.Add(l.dropChance);
        }
    }
}