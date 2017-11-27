using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : EntityMovement {

    protected override void Move() {
        myBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, GetComponent<Rigidbody2D>().velocity.y);
    }
}