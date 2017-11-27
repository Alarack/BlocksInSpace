using UnityEngine;
using System.Collections;

public class ShieldGen : Upgrade {

    [Header("Shield Gen Stats")]
    public GameObject shield;
    public float shieldMaxHealth;
    public float stackAmmount;
    public float shieldRegenValue;

    private GameObject activeShield;

    protected override void Start() {
        base.Start();

        if (player != null)
            CreateShield();
    }

    protected override void Update() {
        if (activeShield == null)
            base.Update();
    }

    void CreateShield() {
        GameObject tmpShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
        tmpShield.GetComponent<Health>().maxHealth = shieldMaxHealth;
        tmpShield.GetComponent<Health>().curHealth = shieldMaxHealth;
        tmpShield.transform.parent = transform;
        tmpShield.transform.localPosition = Vector2.zero;

        activeShield = tmpShield;
    }

    protected override void ApplyIntervalEffect() {
        if (activeShield != null) {
            activeShield.GetComponent<Health>().AdjustHealth(-shieldRegenValue);
        }
        else {
            CreateShield();
        }
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<ShieldGen>();
    }

    public override void StackEffect() {
        shieldMaxHealth += stackAmmount;

        if (effectInterval > 5f) {
            effectInterval -= 1f;
        }
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<ShieldGen>();
        ShieldGen newShield = target.gameObject.GetComponent<ShieldGen>();
        newShield.effectInterval = effectInterval;
        newShield.shield = shield;
        newShield.shieldMaxHealth = shieldMaxHealth;
        newShield.stackAmmount = stackAmmount;
        newShield.activeShield = activeShield;
        newShield.upgradeIcon = upgradeIcon;
    }
}