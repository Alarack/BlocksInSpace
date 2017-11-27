using UnityEngine;
using System.Collections;

public class SpriteSwapper : MonoBehaviour {

    public Sprite[] sprites;
    public bool makeParticles;

    private SpriteRenderer mySprite;
    private EnumList.Colors parentColor;

	void Start () {

        mySprite = GetComponent<SpriteRenderer>();
        parentColor = GetComponentInParent<Weapon>().GetComponentInParent<Entity>().GetColor();
        //parentColor = GetComponentInParent<Entity>().GetColor();

        EnumList.ColorInit(parentColor, mySprite, sprites);

        if (makeParticles) {
            GameObject activeTrail = Instantiate(EnumList.ParticleTrailColor(parentColor.ToString(), "MiddleLaser"), transform.position, Quaternion.identity) as GameObject;
            activeTrail.transform.SetParent(transform, false);
            activeTrail.transform.localPosition = Vector2.zero;
            //activeTrail.transform.localRotation = Quaternion.identity;
        }

    }
}