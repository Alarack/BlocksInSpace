using UnityEngine;
using System.Collections;

public class Growth : MonoBehaviour {

    private const int INFINITE_GROWTH = -1;

    [Header("Growth")]
    public float growthAmount;
    public int maxGrowths = INFINITE_GROWTH;
    public float growthCooldown;
    [Header("Shrinkage")]
    public bool willShrink;


    private float lastGrowth;
    public int curGrowths = 0;
    public int curShrinks = 0;

    void Update() {

        if (CanGrow())
            Grow();
        else if (CanShrink()) {
            Shrink();
        }
    }

    public void Grow() {
        curGrowths++;

        transform.localScale = new Vector3(transform.localScale.x + growthAmount, transform.localScale.y + growthAmount, transform.localScale.z + growthAmount);
        lastGrowth = Time.time + growthCooldown;
    }

    public void Shrink() {
        curShrinks++;
        //transform.localScale /= growthAmount;
        

        transform.localScale = new Vector3(transform.localScale.x - growthAmount, transform.localScale.y - growthAmount, transform.localScale.z - growthAmount);

        if (curShrinks == maxGrowths && maxGrowths != INFINITE_GROWTH) {
            Debug.Log("max shrink");
            curGrowths = 0;
            curShrinks = 0;
            willShrink = false;
        }

    }

    bool CanShrink() {
        if(willShrink && curShrinks < curGrowths) {
            return true;
        }
        else {
            return false;
        }
    }

    bool CanGrow() {
        if(curGrowths == maxGrowths) {
            willShrink = true;
            //curShrinks = 0;
            return false;
        }
        else {
            return Time.time > lastGrowth;
        }
    }
}