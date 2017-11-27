using UnityEngine;
using System.Collections;

public class SwayStar : Projectile {

    public float swayForce;
    public float swayTimer;

    private float lastSway;

    protected override void Start()
    {
        base.Start();

        myBody.angularVelocity = Random.Range(-500f, 500f);
        int leftOrRight = Random.Range(0, 2);

        if (leftOrRight == 0)
            swayForce *= 1;
        else
            swayForce *= -1;
    }

    protected override void FixedUpdate () {
        base.FixedUpdate();

        if (CanSway())
            Sway();

        myBody.AddForce(Vector2.right * swayForce * Time.deltaTime);
	}

    bool CanSway()
    {
        return Time.time > lastSway;
    }

    void Sway()
    {
        swayForce *= -1;
        myBody.velocity = new Vector2(0f, myBody.velocity.y);
        lastSway = Time.time + swayTimer;
    }
}