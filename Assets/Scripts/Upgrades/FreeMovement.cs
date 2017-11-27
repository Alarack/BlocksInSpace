using UnityEngine;
using System.Collections;

public class FreeMovement : Upgrade {

    private PlayerMovement playerMoves;

    protected override void Start() {
        base.Start();
        if (player != null) {
            playerMoves = player.GetComponent<PlayerMovement>();
            player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    void FixedUpdate() {
        if (player != null) {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Input.GetAxisRaw("Vertical") * playerMoves.speed * Time.deltaTime);
        }
    }

    protected override Upgrade GetTargetStatus<T>(T target) {
        return target.GetComponent<FreeMovement>();
    }

    public override void StackEffect() {
        if (effectInterval > 3f)
            effectInterval -= 1f;
    }

    protected override void AddToTarget<T>(T target) {
        target.gameObject.AddComponent<FreeMovement>();
        FreeMovement activeFreMoves = target.gameObject.GetComponent<FreeMovement>();
        activeFreMoves.playerMoves = playerMoves;
        activeFreMoves.upgradeIcon = upgradeIcon;
    }
}