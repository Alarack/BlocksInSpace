using UnityEngine;
using System.Collections;

public class RotateAimHelpers : MonoBehaviour {

    public Transform[] aimHelpers;
    public float loopDegree;
    public bool alternateLeftRight;
    public bool flip;
    public float flipInterval;
    private float flipTimer;
    //private float rotateTimer;

	void Start () {
	
	}

	void Update () {
        foreach(Transform t in aimHelpers) {
            if (alternateLeftRight) {
                if (t.gameObject.name == "Left")
                    t.RotateAround(transform.position, Vector3.forward, loopDegree * Time.deltaTime);

                if (t.gameObject.name == "Right")
                    t.RotateAround(transform.position, Vector3.forward, -loopDegree * Time.deltaTime);
            }
            else {
                t.RotateAround(transform.position, Vector3.forward, loopDegree * Time.deltaTime);
            }

            if (flip) {
                Flip();
            }
        }
	}

    void Flip() {
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval) {
            loopDegree *= -1;
            flipTimer = 0f;
        }
    }
}