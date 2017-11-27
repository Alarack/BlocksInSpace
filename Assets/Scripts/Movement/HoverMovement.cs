using UnityEngine;
using System.Collections;

public class HoverMovement : BasicMovement {

    public float maxVelocityX;
    public float maxVelocityY;

    protected override void Move() {
        base.Move();
        int rand = Random.Range(0, 3);

        switch (rand) {
            case 0:

                direction = Direction.Up;

                break;

            case 1:

                direction = Direction.Right;

                break;

            case 2:

                direction = Direction.Left;

                break;

            case 3:

                direction = Direction.Down;

                break;
        }
    }
}