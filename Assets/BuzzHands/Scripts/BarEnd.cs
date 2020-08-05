using UnityEngine;
using System.Collections;

public class BarEnd : MonoBehaviour {

	public ElectricBar electricBar;

	void OnTriggerEnter(Collider other) {

		RingTriggerArea ringTriggerArea = other.gameObject.GetComponent<RingTriggerArea> ();
		if (ringTriggerArea != null) {
			electricBar.PassedBarEnd (this, ringTriggerArea);
		}
	}
}
