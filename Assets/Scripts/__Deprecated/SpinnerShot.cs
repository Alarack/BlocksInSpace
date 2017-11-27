using UnityEngine;
using System.Collections;

public class SpinnerShot : Projectile {

    //private bool moveOnce = true;
    //public float moveDuration;
    //public float spinSpeed;
    //public float maxSpin;

    //private bool beginSpawn = false;


    //protected override void Start()
    //{
    //    base.Start();

    //    float moveVar = Random.Range(-0.5f, 0.5f);
    //    moveDuration += moveVar;
    //}
    //protected override void PlayerShotMovement()
    //{
    //    if(moveDuration > 0f)
    //    {
    //        moveDuration -= Time.deltaTime;
    //        base.PlayerShotMovement();
    //    }
    //    else
    //    {
    //        myBody.velocity = Vector2.zero;
    //        if(myBody.angularVelocity < maxSpin)
    //            myBody.angularVelocity += spinSpeed;
    //    }

    //    if (myBody.angularVelocity >= maxSpin && !beginSpawn)
    //    {
    //        beginSpawn = true;
    //        GetComponent<PartingGift>().repeatSpawn = true;
    //    }
    //}

    //protected override void EnemyShotMovement()
    //{
    //    if (moveDuration > 0f)
    //    {
    //        moveDuration -= Time.deltaTime;
    //        base.EnemyShotMovement();
    //    }
    //    else
    //    {
    //        myBody.velocity = Vector2.zero;
    //        if (myBody.angularVelocity < maxSpin)
    //            myBody.angularVelocity += spinSpeed;
    //    }

    //    if (myBody.angularVelocity >= maxSpin && !beginSpawn)
    //    {
    //        beginSpawn = true;
    //        GetComponent<PartingGift>().repeatSpawn = true;
    //    }
    //}
}