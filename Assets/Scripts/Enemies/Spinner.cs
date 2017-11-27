using UnityEngine;
using System.Collections;

public class Spinner : EntityEnemy {

    private SpinnerMovement myMovement;

    protected override void Start() {
        base.Start();
        myMovement = GetComponent<SpinnerMovement>();
    }

    public override void FireAllWeapons() {
        if (myMovement.mobState == SpinnerMovement.State.SpinningUp)
            base.FireAllWeapons();
    }
}