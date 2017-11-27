using UnityEngine;
using System.Collections;

public class HealerDrone : Drone {

    [Header("Healer Drone Info")]
    public float healCooldown;
    public float healAmount;
    public GameObject healparticle;

    private float healTimer;
    private Health parentHeatlh;
    private Health myDroneHealth;
    private LineRenderer myLineRenderer;
    private LayerMask mask;
    private GameObject myHeal;
    private GameObject targetHeal;


    protected override void Start() {
        base.Start();
        parentHeatlh = parentWeapon.GetComponentInParent<Entity>().GetComponent<Health>();
        myDroneHealth = GetComponent<Health>();
        mask = 1 << owner.gameObject.layer;
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.useWorldSpace = true;
        myLineRenderer.enabled = false;

        GameObject mytempheal = Instantiate(healparticle) as GameObject;
        mytempheal.transform.parent = transform;
        mytempheal.transform.localPosition = Vector2.zero;
        myHeal = mytempheal;
        myHeal.SetActive(false);

        GameObject targettempheal = Instantiate(healparticle) as GameObject;
        targetHeal = targettempheal;
        myHeal.SetActive(false);
    }

    protected override void Update() {
        base.Update();

        if (parentHeatlh.curHealth < parentHeatlh.maxHealth) {
            myLineRenderer.enabled = true;
            //myHeal.SetActive(true);
            targetHeal.SetActive(true);
            targetHeal.transform.position = owner.gameObject.transform.position;
            Heal();
        }
        else {
            myLineRenderer.enabled = false;
            myHeal.SetActive(false);
            targetHeal.SetActive(false);
        }

        if (myDroneHealth.isDead) {
            Destroy(targetHeal);
            Destroy(myHeal);
        }
    }

    void Heal() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, parentHeatlh.transform.position - transform.position, 30f, mask);
        //Debug.DrawRay(transform.position, parentHeatlh.transform.position - transform.position, Color.green);

        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null && hit.collider == owner.GetComponent<Collider2D>()) {
                healTimer += Time.deltaTime;

                

                myLineRenderer.SetPosition(0, transform.position);
                myLineRenderer.SetPosition(1, hit.point);
                
                if (healTimer >= healCooldown) {
                    parentHeatlh.AdjustHealth(-healAmount);
                    healTimer = 0f;
                }
            }

        }
    }
}