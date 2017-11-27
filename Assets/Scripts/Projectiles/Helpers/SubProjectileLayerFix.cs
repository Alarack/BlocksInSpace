using UnityEngine;
using System.Collections;

public class SubProjectileLayerFix : MonoBehaviour {

	void Start () {

        gameObject.layer = transform.parent.gameObject.layer;
	
	}
	
}