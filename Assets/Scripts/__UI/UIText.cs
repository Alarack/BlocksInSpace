using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIText : MonoBehaviour {

    public Text[] texts;
    //private Entity player;

	void Start () {

        //player = FindObjectOfType<EntityPlayer>();
        texts = GetComponentsInChildren<Text>();
	}

	void Update () {
        //texts[0].text = "Health: " + player.GetComponent<Health>().curHealth.ToString();
        texts[0].text = "Score:" + GameManager.score;
        texts[1].text = ":" + GameManager.missiles;
    }
}