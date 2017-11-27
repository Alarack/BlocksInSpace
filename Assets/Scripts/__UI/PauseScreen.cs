using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PauseScreen : MonoBehaviour {

    public GameObject upgradeContainer;
    public GameObject optionsScreen;

    private EntityPlayer player;
    private List<Upgrade> playerUpgrades = new List<Upgrade>();

    void Awake() {
        player = FindObjectOfType<EntityPlayer>();
        playerUpgrades = player.GetComponents<Upgrade>().ToList();
    }

    void OnEnable() {
        CheckAndAddUpgrades();
        CreateUpgradeIcons();

        if(optionsScreen.activeSelf)
            optionsScreen.SetActive(false);
    }

    public void ToggleOptionsScreen() {
        if (!optionsScreen.activeSelf)
            optionsScreen.SetActive(true);
        else
            optionsScreen.SetActive(false);
    }

    void CheckAndAddUpgrades() {
        List<Upgrade> tempUpgrades = player.GetComponents<Upgrade>().ToList();

        foreach (Upgrade temp in tempUpgrades) {
            if (!playerUpgrades.Contains(temp)) {
                playerUpgrades.Add(temp);
            }
        }
    }

    void CreateUpgradeIcons() {
        for (int i = 0; i < playerUpgrades.Count; i++) {
            if (!playerUpgrades[i].iconCreated) {
                GameObject upgradeIcon = Instantiate(playerUpgrades[i].upgradeIcon) as GameObject;
                playerUpgrades[i].iconCreated = true;
                upgradeIcon.transform.SetParent(upgradeContainer.transform, false);
                upgradeIcon.transform.localPosition = Vector2.zero;
            }
        }
    }
}