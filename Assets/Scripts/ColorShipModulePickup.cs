using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorShipModulePickup : MonoBehaviour {

    public EnumList.Colors color;
    public GameObject module;
    public Sprite[] sprites;

    [Header("Sounds")]
    public string collectionSoundName;
    public float soundVolume = 0.025f;

    private EntityPlayer player;
    private SpriteRenderer mySprite;

    void Start ()
    {
        player = FindObjectOfType<EntityPlayer>();
        mySprite = GetComponent<SpriteRenderer>();
        EnumList.ColorInit(color, mySprite, sprites);
    }

    void Update()
    {
        if (transform.position.y < -10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Feeler" && other.transform.parent.gameObject.tag == "Player")
        {

            if (collectionSoundName != "") {
                SoundManager.PlaySound(collectionSoundName, soundVolume);
            }

            float offsetX = other.transform.position.x;
            float offsetY = other.transform.position.y;
            Vector2 targetPos = new Vector2(offsetX, offsetY);

            GameObject activeModule = Instantiate(module, targetPos, Quaternion.identity) as GameObject;
            ColorModule activeColorMod = activeModule.GetComponent<ColorModule>();
            activeColorMod.unitColor = color;
            activeModule.transform.parent = player.gameObject.transform;
            activeModule.gameObject.tag = "Player";
            activeModule.layer = 10;
            player.colorModules.Add(activeColorMod);
                
            Destroy(gameObject);
        }
    }
}