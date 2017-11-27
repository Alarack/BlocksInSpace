using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorModGenerator : MonoBehaviour {

    public enum ConstructorState {
        Moveing,
        Building
    }
    [Header("Mask Info")]
    public LayerMask mask;
    [Header("Module Info")]
    public List<GameObject> colorMods = new List<GameObject>();
    [Space(5)]
    public int maxMods;
    public int restockValue;
    public float modWeaponDamage;
    public float modWeaponCooldown;
    public float modHeath;
    [Header("Constructor Drone Info")]
    public GameObject generatorDrone;
    public float buildPause;
    public float droneSpeed;
    [Header("Misc Options")]
    public bool randomBuild = true;


    private List<Vector2> nodes = new List<Vector2>();
    private GameObject drone;
    private BasicMovement droneMovement;
    private bool buildSiteFound;
    private bool isConstructing;
    public ConstructorState state;
    private BasicMovement myMoves;

    void Awake() {
        //transform.position = GameObject.FindGameObjectWithTag("LootSpawn").transform.position;

        if (randomBuild) {
            FindInitialBuildLocations("Feeler");
        }
        else {
            FindInitialBuildLocations("BuildSite");
        }
    }

    void Start() {
        myMoves = GetComponent<BasicMovement>();
        GenerateDrone();

        if(GetComponent<EntityEnemy>() != null) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

    }

    void Update() {
        if (GetComponent<Entity>().colorModules.Count == 0 && maxMods == 0) {
            if (randomBuild) {
                GenerateDrone();
                maxMods = restockValue;
            }
            else {
                GenerateDrone();
                FindInitialBuildLocations("BuildSite");
            }
        }

        if (maxMods > 0) {
            myMoves.direction = BasicMovement.Direction.Still;

            switch (state) {
                case ConstructorState.Moveing:
                    if (nodes.Count > 0)
                        StartCoroutine(MoveConstructor());

                    break;

                case ConstructorState.Building:
                    StartCoroutine(ConstructMod());

                    break;
            }
        }
        else {
            myMoves.direction = BasicMovement.Direction.Up;
        }

    }//End of FixedUpdate

    void GenerateDrone() {
        if (drone == null) {
            //Debug.Log("Drone Doesn't Exist, creating one");
            GameObject activeDrone = Instantiate(generatorDrone, transform.position, transform.rotation) as GameObject;
            drone = activeDrone;
            drone.transform.parent = transform;
            droneMovement = drone.GetComponent<BasicMovement>();
            droneMovement.speed = droneSpeed;
        }
        else if (!drone.activeSelf) {
            //Debug.Log("Drone Exists");
            drone.gameObject.SetActive(true);
        }
    }

    IEnumerator MoveConstructor() {
        if (!buildSiteFound) {
            int targetPos = Random.Range(0, nodes.Count);
            droneMovement.target = nodes[targetPos];
            buildSiteFound = true;
        }

        droneMovement.direction = BasicMovement.Direction.Directed;

        if (drone.transform.position == droneMovement.target) {
            yield return new WaitForSeconds(buildPause);
            buildSiteFound = false;
            state = ConstructorState.Building;
        }
    }

    IEnumerator ConstructMod() {
        if (!isConstructing) {
            Collider2D testPing = Physics2D.OverlapCircle(drone.transform.position, 0.1f, mask);

            if (testPing == null) {
                SpawnMod();
                nodes.Remove(drone.transform.position);
                maxMods--;

                if (maxMods == 0) {
                    SpawnsComplete();

                    drone.gameObject.SetActive(false);
                }

                if (maxMods == 0 && GetComponent<EntityEnemy>() != null) {
                    GetComponent<EntityEnemy>().autoFire = true;
                }

                yield return new WaitForSeconds(buildPause);
                isConstructing = true;
            }
            else {
                nodes.Remove(drone.transform.position);
                isConstructing = true;
            }
        }

        yield return new WaitForSeconds(buildPause);
        isConstructing = false;
        state = ConstructorState.Moveing;
    }

    void SpawnsComplete() {
        foreach (ColorModule c in GetComponent<Entity>().colorModules) {
            if (c.activePrimary != null) {
                c.activePrimary.GetComponent<Weapon>().autoFire = true;

                if(c.myPrimaryWeapon.weaponType == Weapon.WeaponType.Blaster || c.myPrimaryWeapon.weaponType == Weapon.WeaponType.Shotgun) {
                    //c.activePrimary.transform.FindChild("ShotOrigin").rotation = transform.FindChild("ShotOrigin").rotation;
                    c.myPrimaryWeapon.shotOrigin.rotation = transform.FindChild("ShotOrigin").rotation;
                }
                    
            }
        }
    }

    void SpawnMod() {
        int randomMod = Random.Range(0, colorMods.Count);
        GameObject activeModule = Instantiate(colorMods[randomMod], drone.transform.position, Quaternion.identity) as GameObject;
        ColorModule activeColorMod = activeModule.GetComponent<ColorModule>();
        Weapon modWeapon = activeColorMod.GetComponentInChildren<Weapon>();
        ColorModHealth activeModHealth = activeModule.GetComponent<ColorModHealth>();
        activeModHealth.maxHealth = modHeath;

        if (modWeapon != null) {
            modWeapon.damage = modWeaponDamage;
            modWeapon.coolDown = modWeaponCooldown;
        }

        activeColorMod.unitColor = EnumList.RandomColor();

        //activeModule.transform.parent = transform;
        activeModule.transform.SetParent(transform, false);
        

        activeModule.transform.localScale *= GetComponent<Entity>().modScaleFactor;
        activeModule.transform.position = drone.transform.position;


        activeModule.gameObject.tag = gameObject.tag;
        activeModule.layer = EnumList.CheckLayer(gameObject);
        GetComponent<Entity>().colorModules.Add(activeColorMod);

        if (randomBuild) {
            foreach (Feeler f in activeColorMod.feelers) {
                nodes.Add(f.transform.localPosition + drone.transform.position);
            }
        }
    }

    void FindInitialBuildLocations(string locationTag) {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).tag == locationTag) {
                nodes.Add(transform.GetChild(i).transform.position);
            }
        }

        if (!randomBuild)
            maxMods = nodes.Count;
    }
}