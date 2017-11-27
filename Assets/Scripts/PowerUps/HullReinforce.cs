using UnityEngine;
using System.Collections;

public class HullReinforce : PowerUp {

    [Header("Hull Reinforce Info")]
    public float healValue;

    protected override void PowerUpEffect() {
        FindObjectOfType<PlayerHealth>().AdjustHealth(-healValue);
    }
}