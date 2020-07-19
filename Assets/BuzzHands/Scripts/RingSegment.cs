using UnityEngine;
using System.Collections;

public class RingSegment : MonoBehaviour {

	public WandController wandController;
	public ParticleSystem buzzFlash;

    void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Electric")) {
			if (buzzFlash != null) {
				buzzFlash.Play ();
                /*
				if (wandController) {
					wandController.Vibrate ();
					if (other.gameObject.name != "BarBody") {
						StartCoroutine (ExecuteAfterTime (.5f));
					}
				}
                */
			}
		}
	}

    /*
	IEnumerator ExecuteAfterTime(float time) {
		yield return new WaitForSeconds (time);
		wandController.ActivateWand (false);
	}
    */

}
