using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : Health {

    private float minXHealthValue;
    private RectTransform healthTransform;
    private bool adjHealthBar;
    private bool isGameOver = false;

    protected override void Start() {
        healthTransform = GameObject.FindWithTag("PlayerHealthBar").transform as RectTransform;
        minXHealthValue = healthTransform.localPosition.x - healthTransform.rect.width;
        base.Start();
    }

    protected override void Update() {
        base.Update();

        if (adjHealthBar) {
            AdjustHealthBar();
        }
    }

    protected override void EntityDying() {
        isDead = true;
        GameObject activeboom = Instantiate(deathExplosion, transform.position, transform.rotation) as GameObject;
        Destroy(activeboom, 2f);
    }

    protected override void EntityDead() {
        if (!isGameOver) {
            isGameOver = true;
            foreach (ColorModule c in GetComponent<Entity>().colorModules) {
                c.GetComponent<Health>().KillEntity();
            }

            StartCoroutine(FindObjectOfType<GameManager>().GameOver());
            GetComponent<EntityMovement>().speed = 0f;
            GetComponent<Renderer>().enabled = false;
        }
    }

    void AdjustHealthBar() {
        Vector3 targetPos = new Vector3(Mathf.Abs(minXHealthValue) * healthDifference + minXHealthValue, 0f, 0f);
        healthTransform.localPosition = Vector3.Lerp(healthTransform.localPosition, targetPos, 0.1f);
        if (healthTransform.localPosition == targetPos) {
            adjHealthBar = false;
        }
    }

    public override void AdjustHealth(float healthAdj, Entity source = null) {
        base.AdjustHealth(healthAdj, source);
        adjHealthBar = true;
    }

    public override void FullHealth() {
        base.FullHealth();
        healthTransform.localPosition = new Vector2(0f, 0f);
        adjHealthBar = false;
    }

    public override void KillEntity() {
        base.KillEntity();
        healthTransform.localPosition = new Vector2(minXHealthValue, 0f);
    }
}