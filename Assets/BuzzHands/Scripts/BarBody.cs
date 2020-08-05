using UnityEngine;
using System.Collections;

public class BarBody : MonoBehaviour {

	public ElectricBar electricBar;

	void OnTriggerEnter(Collider other) {
		if (CompareTag("Electric") && other.gameObject.CompareTag("Shocker")) {
			electricBar.Shocked ();
			electricBar.BlowOutABulb ();
		}
        else if (CompareTag("Electric"))// && !other.gameObject.CompareTag("Head") && !other.gameObject.CompareTag("Tail"))
        {
            Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
            //electricBar.Shocked();
        }
	}

}
