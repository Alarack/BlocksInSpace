using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorModule : Entity {


    [Header("Feeler Prefabs")]
    public Feeler[] feelers;
    [Header("Sprite Stuff")]
    public Sprite[] sprites;


    //public Weapon myWeapon;
    [HideInInspector]
    public Entity mainShip;
    [HideInInspector]
    public List<ColorModule> neighbors = new List<ColorModule>();

    protected override void Awake() {
        base.Awake();
        CheckAndAddNeighbors();
    }

    protected override void Start() {
        base.Start();
        EnumList.ConfirmWeaponLayer(gameObject, activePrimary, activeSpecial); //TODO: This is a hack. Find out why Color Mod Weapons are not getting the correct Layer. It has to do with Weapons being made in Awake.

        if (gameObject.tag == "Player")
            mainShip = GetComponentInParent<EntityPlayer>();
        else
            mainShip = GetComponentInParent<EntityEnemy>();

        EnumList.ColorInit(unitColor, mySprite, sprites);
        if (mainShip == null)
            Debug.Log("Can't Find Ship");
        else {
            if (fullWeaponList.Count > 0 )
               mainShip.fullWeaponList.AddRange(fullWeaponList);

            CheckMatch();
        }

        if(activePrimary != null) {
            if(myPrimaryWeapon.weaponType == Weapon.WeaponType.Laser) {
                activePrimary.transform.localScale = (activePrimary.transform.localScale / transform.parent.localScale.x);
            }
        }
    }

    public void AddNeighbor(ColorModule neighbor) {
        if (!neighbors.Contains(neighbor))
            neighbors.Add(neighbor);
    }

    public void RemoveNeighbor(ColorModule neighbor) {
        neighbors.Remove(neighbor);
    }

    public bool IsNeighborsWith(ColorModule node) {
        if (neighbors.Contains(node))
            return true;
        else
            return false;
    }

    public void CheckAndAddNeighbors() {
        foreach (Feeler feeler in feelers) {
            ColorModule colorScript = feeler.CheckArea();
            if (colorScript != null)
                AddNeighbor(colorScript);
        }
    }

    public void RemoveSelf() {
        MatchReward();

        mainShip.fullWeaponList.Remove(myPrimaryWeapon);
        mainShip.colorModules.Remove(this);

        foreach (ColorModule mod in neighbors) {
            if (mod.neighbors.Contains(this)) {
                mod.RemoveNeighbor(this);
            }
        }
        int numFrags = Random.Range(1, 5);
        for (int i = 0; i < numFrags; i++) {
            GameObject activeFragment = Instantiate(EnumList.FragmentColor(unitColor), transform.position, Quaternion.identity) as GameObject;
            activeFragment.GetComponent<Fragment>().color = GetColor();
        }

        Destroy(this.gameObject);
    }

    public void CheckMatch() {
        foreach (ColorModule mod1 in neighbors) {
            if (mod1.unitColor == this.unitColor) {
                foreach (ColorModule mod2 in mod1.neighbors) {
                    if (mod2.unitColor == this.unitColor && mod2 != this) {
                        mod2.Invoke("RemoveSelf", 0.2f);
                        mod1.Invoke("RemoveSelf", 0.4f);
                        Invoke("RemoveSelf", 0.6f);
                    }
                }
            }
        }
    }

    void MatchReward() {
        switch (unitColor) {
            case EnumList.Colors.Red:
                mainShip.damageModifier += 0.01f;

                break;

            case EnumList.Colors.Yellow:

                foreach (ColorModule cm in mainShip.colorModules) {
                    cm.GetComponent<Health>().FullHealth();
                }

                break;

            case EnumList.Colors.Green:
                mainShip.GetComponent<Health>().AdjustHealth(-(Mathf.Round(GetComponent<Health>().maxHealth / 3)));

                break;
        }
    }
}