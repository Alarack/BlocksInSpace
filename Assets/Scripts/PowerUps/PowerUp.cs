using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PowerUp : MonoBehaviour {
    //TODO: Do this entire thing over. This is some noob shit right here.
    [Header("PowerUp Duration Info")]
    public float duration;
    public bool forever;

    protected Weapon[] weapons;
    protected EntityPlayer player;

    protected virtual void Start() {
        player = FindObjectOfType<EntityPlayer>();
        weapons = GetComponents<Weapon>();
    }

    protected virtual void Update() {
        if (transform.position.y < -10f) {
            Destroy(gameObject);
        }
    }

    protected abstract void PowerUpEffect();

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            gameObject.tag = other.gameObject.tag;
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0f);
            GetComponent<Collider2D>().enabled = false;

            AlignEffect(other);
            PowerUpEffect();

            if (!forever)
                Destroy(gameObject, duration);
        }
    }

    protected virtual void AlignEffect(Collider2D other) {
        Destroy(GetComponent<Rigidbody2D>());
        transform.parent = other.transform;
        transform.rotation = other.transform.rotation;
        transform.localPosition = Vector2.zero;
    }
}