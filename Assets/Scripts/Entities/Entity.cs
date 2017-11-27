using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class Entity : MonoBehaviour {


    public enum ThrusterType {
        Rocket,
        MedThrust,
        ThinThrust,
        BackFire
    }

    [Header("Basic Entity Stats")]
    public EnumList.Colors unitColor;
    public float damageModifier = 1f;
    public float attackSpeedModifier = 1f;
    public bool randomizeAttackSpeed = false;
    public float difficultyValue;
    [Header("Weapon Prefabs")]
    public GameObject primaryWeapon;
    public GameObject primarySpecial;
    public bool deployAuxWeapons = true;
    public List<GameObject> auxWeapons = new List<GameObject>();
    public float weaponScaleFactor = 1f;
    public float auxWeaponScaleFactor = 1f;
    public float weaponShotScaleMod = 1f;
    public float weaponFlashScaleFactor = 1f;
    public float weaponParticleTrailScaleFactor = 1f;
    [Header("Default Modules")]
    public GameObject defaultMod;
    public bool randomDefaultColors;
    public float modScaleFactor = 1f;
    public float modWeaponFireRateMod = 1f;
    public float modHealthFactor;
    public Transform[] defaultModPositions;
    [Header("Thruster Info")]
    public bool useThrusters;
    public float thrusterScaleMod = 1f;
    public Transform[] thrusterLoations;
    public ThrusterType thrusterType;
    [Header("Misc Info")]
    public bool bossEntity;


    [HideInInspector]
    public GameObject activePrimary;
    [HideInInspector]
    public GameObject activeSpecial;
    [HideInInspector]
    public List<GameObject> upgrades = new List<GameObject>();
    [HideInInspector]
    public Weapon myPrimaryWeapon;
    [HideInInspector]
    public Weapon specialWeapon;
    [HideInInspector]
    public List<ColorModule> colorModules = new List<ColorModule>();
    [HideInInspector]
    public List<Weapon> fullWeaponList = new List<Weapon>();
    [HideInInspector]
    public List<Transform> weaponHardPoints = new List<Transform>();
    [HideInInspector]
    public Transform primeWeaponMount;

    protected SpriteRenderer mySprite;

    protected virtual void Awake() {
        if (primaryWeapon != null)
            PrimaryWeaponSetUp();
        if (primarySpecial != null)
            SpecialWeaponSetUp();
        if (auxWeapons.Count > 0 && deployAuxWeapons)
            AuxWeaponSetup();

        if(defaultMod != null)
            SetUpDefaultMods();
    }

    protected virtual void Start() {
        mySprite = GetComponent<SpriteRenderer>();
        //SetUpDefaultMods();

        primeWeaponMount = transform.FindChild("PrimeWeaponMount");

        if(primeWeaponMount != null) {
            primeWeaponMount.gameObject.tag = gameObject.tag;
            primeWeaponMount.gameObject.layer = gameObject.layer;
        }

        if (useThrusters) {
            SetUpThrusters();
        }

        if (randomizeAttackSpeed) {
            float rand = Random.Range(0.2f, 1.8f);
            attackSpeedModifier *= rand;
        }

    }

    protected virtual void Update() {

    }

    public virtual void PrimaryWeaponSetUp() {
        GameObject tmpPrimaryWeapon = Instantiate(primaryWeapon) as GameObject;

        EnumList.InitWeapon(gameObject, tmpPrimaryWeapon, "PrimaryWeaponMount", 0, weaponScaleFactor);
        myPrimaryWeapon = tmpPrimaryWeapon.GetComponent<Weapon>();
        fullWeaponList.Add(tmpPrimaryWeapon.GetComponent<Weapon>());
        activePrimary = tmpPrimaryWeapon;
    }

    public virtual void SpecialWeaponSetUp() {
        GameObject tmpSpecialWeapon = Instantiate(primarySpecial) as GameObject;

        EnumList.InitWeapon(gameObject, tmpSpecialWeapon);
        specialWeapon = tmpSpecialWeapon.GetComponent<Weapon>();
        activeSpecial = tmpSpecialWeapon;
    }

    public virtual void AuxWeaponSetup() {
        foreach (Transform child in transform.FindChild("HardPoints")) {
            if (child.gameObject.tag == "WeaponHardPoint") {
                weaponHardPoints.Add(child);
            }
        }

        for (int i = 0; i < weaponHardPoints.Count; i++) {
            GameObject tmpAuxWeapon = Instantiate(auxWeapons[i]) as GameObject;
            EnumList.InitWeapon(gameObject, tmpAuxWeapon, "WeaponHardPoint", i, auxWeaponScaleFactor);
            fullWeaponList.Add(tmpAuxWeapon.GetComponent<Weapon>());
        }
    }

    public virtual void SetUpThrusters() {

        for (int i = 0; i < thrusterLoations.Length; i++) {

            GameObject thrusterParticle = Instantiate(EnumList.ParticleTrailColor(unitColor.ToString(), thrusterType.ToString()), thrusterLoations[i].position, thrusterLoations[i].rotation) as GameObject;
            thrusterParticle.transform.parent = thrusterLoations[i];

            EnumList.ScaleParticleEffect(thrusterParticle, thrusterScaleMod);
        }
    }


    protected virtual void SetUpDefaultMods() {
        if (defaultModPositions.Length > 0) {

            for (int i = 0; i < defaultModPositions.Length; i++) {
                GameObject activeModule = Instantiate(defaultMod) as GameObject;
                ColorModule activeColorMod = activeModule.GetComponent<ColorModule>();

                //TODO: Consider moving this bit over to the ColorMod script rather than in Entity.
                Weapon[] modWeapons = activeModule.GetComponentsInChildren<Weapon>();

                foreach(Weapon w in modWeapons) {
                    w.coolDown *= modWeaponFireRateMod;
                }

                //activeModule.transform.parent = transform;
                activeModule.transform.SetParent(transform, false);
                activeModule.transform.localScale *= modScaleFactor;

                activeModule.transform.position = defaultModPositions[i].position;
                activeModule.transform.rotation = defaultModPositions[i].rotation;
                activeModule.gameObject.tag = gameObject.tag;
                activeModule.layer = gameObject.layer;
                activeColorMod.damageModifier = damageModifier;

                if (activeColorMod.unitColor != EnumList.Colors.Grey) {
                    if (randomDefaultColors) {
                        activeColorMod.unitColor = EnumList.RandomColor();
                    }
                    else {
                        activeColorMod.unitColor = GetColor();
                    }
                }

                if (modHealthFactor > 0) {
                    activeColorMod.GetComponent<Health>().maxHealth += modHealthFactor;
                }

                colorModules.Add(activeColorMod);
            }
        }
    }

    public EnumList.Colors GetColor() {
        return unitColor;
    }
}