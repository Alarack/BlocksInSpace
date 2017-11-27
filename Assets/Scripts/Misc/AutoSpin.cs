using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody2D))]
public class AutoSpin : MonoBehaviour {

    public float rotateSpeed;
    public bool flip;
    public bool alternateSpin;
    public float flipInteravl;
    public bool spinTransform;
    public bool correctChildWeaponRotation = false;
    [Space(10)]
    public bool delayedStart;
    public float initTimeDelay;
    public bool modifySpeedAfterTime;
    public float multiplier;
    public float speedModTimeDelay;

    private Entity self;
    private static int dir = 1;
    private Rigidbody2D myBody;
    private float flipTimer;

    void Start() {
        self = GetComponent<Entity>();
        myBody = GetComponent<Rigidbody2D>();

        if (myBody != null && !spinTransform)
            myBody.angularVelocity = rotateSpeed;

        if (alternateSpin) {
            dir = EnumList.Alternator(dir);

            if (dir == 1)
                rotateSpeed *= -1;
            else
                rotateSpeed = Mathf.Abs(rotateSpeed);
        }
        else {
            rotateSpeed = Mathf.Abs(rotateSpeed);
        }

        if (delayedStart) {
            Invoke("ToggleDelay", initTimeDelay);
        }

        if (modifySpeedAfterTime) {
            Invoke("ModifySpeed", speedModTimeDelay);
        }
    }

    void Update() {
        if(!delayedStart)
            BeginRotation();
    }

    void BeginRotation() {
        if (myBody == null || spinTransform) {
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Time.deltaTime * rotateSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Time.deltaTime * rotateSpeed, Vector3.forward), Time.deltaTime * rotateSpeed);
            //transform.Rotate(Vector3.forward, 1);
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            if (correctChildWeaponRotation)
                CorrectChildWeaponRotation();
        }

        if (flip) {

            flipTimer += Time.deltaTime;

            if (flipTimer >= flipInteravl) {
                rotateSpeed *= -1;
                flipTimer = 0f;
            }

        }
    }

    void CorrectChildWeaponRotation() {
        //Quaternion storedRotation = transform.rotation;
        //transform.rotation = TargetUtils.SmoothRotation(target, transform, turnSpeed);

        //Rotation Correction for Child Weapon
        if (self != null && self.activePrimary != null) {
            //Quaternion diff = Quaternion.Inverse(transform.rotation) * storedRotation;
            if(self.transform.FindChild("PrimeWeaponMount") != null)
                self.transform.FindChild("PrimeWeaponMount").Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            else {
                self.activePrimary.transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }

        foreach(ColorModule c in self.colorModules) {
            if(c.transform.FindChild("PrimeWeaponMount") != null) {
                c.transform.FindChild("PrimeWeaponMount").Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            }
        }

    }


    void ModifySpeed() {
        rotateSpeed *= multiplier;
    }

    void ToggleDelay() {
        delayedStart = false;
    }


}