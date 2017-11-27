using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOptions : MonoBehaviour {

    public static bool enableScreenShake;

    public void ToggleScreenShake() {
        enableScreenShake = !enableScreenShake;

        if(enableScreenShake == true)
            transform.FindChild("ScreenShake").GetComponentInChildren<Text>().text = "Screen Shake: On";
        else
            transform.FindChild("ScreenShake").GetComponentInChildren<Text>().text = "Screen Shake: Off";
    }
}