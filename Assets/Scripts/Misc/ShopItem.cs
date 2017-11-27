using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour {

    [Header("Iten Info")]
    public GameObject item;
    public GameObject itemIcon;
    public string itemName;

    private GameObject shopMenu;
    private Text shopMenuText;
    private EntityPlayer player;
    private Upgrade[] upgrades;

    private MasterSpawner spawner;

    void Awake() {
        shopMenu = GameObject.FindGameObjectWithTag("Shop");
        shopMenuText = shopMenu.GetComponentInChildren<Text>();

        if (GameObject.FindGameObjectWithTag("Spawner") != null)
            spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MasterSpawner>();
    }

    void Start() {
        player = FindObjectOfType<EntityPlayer>();
        upgrades = GetComponents<Upgrade>();

        if (itemIcon != null) {
            GameObject activeIcon = Instantiate(itemIcon) as GameObject;
            activeIcon.transform.SetParent(transform, false);
            activeIcon.transform.localPosition = Vector2.zero;
        }
    }

    public void ShowName() {
        shopMenuText.text = itemName;
    }

    public void HideName() {
        shopMenuText.text = "Choose One";
    }

    public void SpawnItem() {
        if (item != null) {
            //GameObject activeItem = Instantiate(item, Vector3.zero, Quaternion.identity) as GameObject;
        }

        if (upgrades.Length > 0) {
            foreach (Upgrade upgrade in upgrades) {
                upgrade.Apply(player);
                player.upgrades.Add(upgrade.upgradeIcon);

            }
            if (spawner != null) {
                if(spawner.waves.Length > 0)
                    spawner.waves[spawner.waveCount].upgradeWave = false;
                else if(spawner.GetComponent<RandomSpawner>().randomWaves.Count > 0)
                    spawner.GetComponent<RandomSpawner>().randomWaves[spawner.waveCount].upgradeWave = false;
            }
                
            //spawner.waves[spawner.waveCount].IsWaveFinished(true);
        }

        //activeItem.transform.parent = player.transform;
        //activeItem.transform.localPosition = Vector2.zero;

        shopMenu.SetActive(false);
        //Time.timeScale = 1f;
    }
}