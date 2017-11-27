using UnityEngine;
using System.Collections;

public class CrelenatorMovement : BasicMovement {

    [Header("Crenelator Direction Time")]
    public float upDuration;
    public float rightDuration;
    public float leftDuration;

    private bool isRight = false;
    //TODO: Find a way to avoid CoRoutines so the movement can be put in FixedUpdate
    protected override void Move() {
        base.Move();

        switch (direction) {
            case Direction.Up:
                StartCoroutine(MoveUp(isRight));

                break;

            case Direction.Right:
                StartCoroutine(MoveRight());

                break;

            case Direction.Left:
                StartCoroutine(MoveLeft());

                break;
        }
    }

    IEnumerator MoveUp(bool right) {
        yield return new WaitForSeconds(upDuration);

        if (right)
            direction = Direction.Right;
        else
            direction = Direction.Left;
    }

    IEnumerator MoveRight() {
        yield return new WaitForSeconds(rightDuration);
        isRight = false;

        direction = Direction.Up;
    }

    IEnumerator MoveLeft() {
        yield return new WaitForSeconds(leftDuration);
        isRight = true;
        direction = Direction.Up;
    }
}