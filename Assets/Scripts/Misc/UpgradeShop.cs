using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UpgradeShop : MonoBehaviour {

    public GameObject[] shopItems;
    public Transform[] itemLocations;
    [Header("Sounds")]
    public SoundEntry onOpenSound;

    private MasterSpawner spawnerScript;

    //Consider a way to determine what kinds of items are generated. Maybe new ones unlock over time, or maybe it's based on the current wave?

    void OnEnable() {
        CleanUpOldItems();
        GenerateShopItems();

        if (onOpenSound != null)
            onOpenSound.PlaySound();

        if(spawnerScript != null)
            spawnerScript.beginSpawing = false;
    }

    void OnDisable() {
        if(spawnerScript != null)
            spawnerScript.beginSpawing = true;
    }

    void Start() {
        if(GameObject.FindGameObjectWithTag("Spawner") != null)
            spawnerScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MasterSpawner>();
    }

    void Update() {
        if(CameraController.shake > 0f) {
            CameraController.shake = 0f;
        }
    }

    void GenerateShopItems() {
        List<GameObject> randomItems = new List<GameObject>();
        GameObject[] tempItems = randomItems.Distinct().ToArray();

        while (tempItems.Length < itemLocations.Length) {
            for (int i = 0; i < shopItems.Length; i++) {
                int rand = Random.Range(0, shopItems.Length);
                randomItems.Add(shopItems[rand]);
            }

            tempItems = randomItems.Distinct().ToArray();
        }

        if (tempItems.Length < itemLocations.Length) {
            Debug.LogError("Shit, we didn't generate one item for each item location.");
        }
        else {
            for (int i = 0; i < itemLocations.Length; i++) {
                GameObject activeItem = Instantiate(tempItems[i], itemLocations[i].position, itemLocations[i].rotation) as GameObject;
                activeItem.transform.SetParent(itemLocations[i], false);
                activeItem.transform.localPosition = Vector2.zero;
            }
        }
    }

    void CleanUpOldItems() {
        ShopItem[] allItems = FindObjectsOfType<ShopItem>();

        if (allItems.Length > 0) {
            foreach (ShopItem item in allItems) {
                Destroy(item.gameObject);
            }
        }
    }
}