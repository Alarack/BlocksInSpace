using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


    public static float shake = 0f;
    public static float shakeAmount = 0.7f;

    [HideInInspector]
    public float decreaseFactor = 1.5f;
    [HideInInspector]
    public Vector3 initPos;

    void Start() {
        initPos = transform.position;
    }

    void Update() {

        if (GameOptions.enableScreenShake) {
            if (shake > 0) {
                transform.position = transform.position + Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;
            }
            else {
                shake = 0f;
                transform.position = initPos;
            }
        }
    }

    public static void ShakeCam(float amount, float shakeTime) {
        shake = shakeTime;
        shakeAmount = amount;
    }
}