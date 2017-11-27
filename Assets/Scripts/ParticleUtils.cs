using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Rigidbody2D))]
public class ParticleUtils : MonoBehaviour {

    public float rotateSpeed;
    public bool flip;
    public bool alternateSpin;
    public float flipInteravl;
    public bool spinTransform = true;
    [Space(10)]
    public bool delayedStart;
    public float initTimeDelay;
    public bool modifySpeedAfterTime;
    public float multiplier;
    public float speedModTimeDelay;

    private static int dir = 1;
    private Rigidbody2D myBody;
    private float flipTimer;

    void Start() {
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
        if (!delayedStart)
            BeginRotation();
    }

    void BeginRotation() {
        if (myBody == null || spinTransform) {
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Time.deltaTime * rotateSpeed);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Time.deltaTime * rotateSpeed, Vector3.forward), Time.deltaTime * rotateSpeed);
            //transform.Rotate(Vector3.forward, 1);
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }

        if (flip) {

            flipTimer += Time.deltaTime;

            if (flipTimer >= flipInteravl) {
                rotateSpeed *= -1;
                flipTimer = 0f;
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