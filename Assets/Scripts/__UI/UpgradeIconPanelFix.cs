using UnityEngine;
using System.Collections;

public class UpgradeIconPanelFix : MonoBehaviour {

    public GameObject panel;

    private PauseScreen pauseScreen;

    void OnEnable() {
        pauseScreen = FindObjectOfType<PauseScreen>();

        if (pauseScreen == null) {
            panel.SetActive(false);
        }
    }
}