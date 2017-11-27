using UnityEngine;
using System.Collections;

public class SwayMovement : BasicMovement {

    [Header("Sway Movement Info")]
    public float swayForce;
    public float swayTimer;

    public bool randomSpin;

    private float lastSway;

    protected override void Start() {
        base.Start();

        if(randomSpin)
            myBody.angularVelocity = Random.Range(-500f, 500f);

        int leftOrRight = Random.Range(0, 2);

        if (leftOrRight == 0)
            swayForce *= 1;
        else
            swayForce *= -1;
    }

    protected override void Move() {
        base.Move();

        if (CanSway())
            Sway();

        myBody.AddForce(Vector2.right * swayForce * Time.deltaTime);
    }

    bool CanSway() {
        return Time.time > lastSway;
    }

    void Sway() {
        swayForce *= -1;
        //myBody.velocity = new Vector2(0f, myBody.velocity.y);
        lastSway = Time.time + swayTimer;
    }
}