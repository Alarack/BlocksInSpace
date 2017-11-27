using UnityEngine;
using System.Collections;

public class Fragment : MonoBehaviour {

    public float speed;
    public float points;

    [HideInInspector]
    public EnumList.Colors color;

    private Rigidbody2D myBody;
    private Vector2 initVector;

    void Start() {

        myBody = GetComponent<Rigidbody2D>();
        float rotateSpeed = Random.Range(-500f, 500f);
        myBody.angularVelocity = rotateSpeed;
        initVector = Random.insideUnitCircle;
        myBody.velocity = initVector * speed * Time.deltaTime;
        Invoke("Detonate", 1.5f);
    }

    void Detonate() {
        GameObject boomParticle = Instantiate(EnumList.ExplosionColor(color), transform.position, transform.rotation) as GameObject;
        Destroy(boomParticle, 2f);

        GameManager.score += points;
        Destroy(gameObject);
    }
}