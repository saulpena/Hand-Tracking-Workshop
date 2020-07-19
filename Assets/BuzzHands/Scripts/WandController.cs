using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour {
#if false
    public RingTriggerArea ringTriggerAreaPrefab;
	public RingSegment ringSegmentPrefab;

	public AudioClip buzzSound;
	public AudioClip dingSound;
	public AudioClip passedSound;

	GameObject wand;

	public Material nibMaterial;
	public Material wandMaterial;

	public int totalRingPoints = 45;
	public float ringRadius = 4.5f;

	private SteamVR_TrackedObject trackedObject;
	private SteamVR_Controller.Device device;

	GameObject nib = null;
	GameObject ringCenter = null;

	private float wandLineWidth = .01f;

	private AudioSource source;

	bool shouldVibrate = false;
	bool wandLastActive = false;

	List<WireSegment> currentWire;
	Vector3 previousWireSegmentPosition;
	float wireSegmentDiameter = 0;

	void Start(){
		trackedObject = GetComponent<SteamVR_TrackedObject>();

		source = GetComponent<AudioSource> ();
		nib = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		nib.name = "Nib";
		nib.GetComponent<Renderer> ().material = nibMaterial;

		nib.transform.parent = trackedObject.gameObject.transform;
		nib.transform.localScale = new Vector3 (wandLineWidth, .001f, wandLineWidth);
		nib.transform.localPosition = new Vector3 (0, 0, .01f);
		nib.transform.localRotation = Quaternion.Euler (0, 90, 90);

		wand = GameObject.CreatePrimitive (PrimitiveType.Cylinder);
		wand.name = "Wand";
		wand.GetComponent<Renderer> ().material = wandMaterial;

		wand.transform.parent = trackedObject.gameObject.transform;
		wand.transform.localScale = new Vector3 (wandLineWidth, .05f, wandLineWidth);
		wand.transform.localPosition = new Vector3 (0, 0, .01f);
		wand.transform.localRotation = Quaternion.Euler (90, 90, 90);

		ringCenter = new GameObject ();
		ringCenter.name = "Ring Center";
		ringCenter.transform.localScale = new Vector3 (.01f, .01f, .01f);
		ringCenter.transform.parent = wand.transform;
		ringCenter.transform.localPosition = new Vector3 (0, 2f, 0);

		RingTriggerArea ringTriggerArea = Instantiate(ringTriggerAreaPrefab);
		ringTriggerArea.name = "Ring Trigger Area";
		ringTriggerArea.wandController = this;

		ringTriggerArea.transform.parent = ringCenter.transform;
		ringTriggerArea.transform.localPosition = Vector3.zero;
		float ringDiameter = ringRadius * 2f;
		ringTriggerArea.transform.localScale = new Vector3 (ringDiameter - 1f, .01f, ringDiameter - 1f);

		ActivateWand (true);//false);
	}

	void Update(){

		// HO-R why can't this be done once, in start?
		device = SteamVR_Controller.Input((int)trackedObject.index); 
		if (shouldVibrate) {
			device.TriggerHapticPulse (3000);
		}
		// Grip pressed:
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {

			// HO-R ActivateWand (!wand.activeSelf);
		}
		if (device.GetPressDown (SteamVR_Controller.ButtonMask.Touchpad)) {
			if (wandLastActive) {
				wand.SetActive (false);
			} else {
				nib.SetActive (false);
			}
		}
		if (device.GetPressUp (SteamVR_Controller.ButtonMask.Touchpad)) {
			if (wandLastActive) {
				wand.SetActive (true);
			} else {
				nib.SetActive (true);
			}
		}

		if (!wand.activeSelf) {
			if (device.GetTouchDown (SteamVR_Controller.ButtonMask.Trigger)) {
				currentWire = new List<WireSegment> ();
				GameController.instance.wires.Add (currentWire);

			} else if (device.GetTouch (SteamVR_Controller.ButtonMask.Trigger)) {
	
				Vector3 nibPosition = nib.transform.position;
				if (currentWire.Count == 0 || Vector3.Distance(previousWireSegmentPosition, nibPosition) > wireSegmentDiameter) {
						
					WireSegment wireSegment = Instantiate (GameController.instance.wireSegmentPrefab);
					if (wireSegmentDiameter == 0) {
						wireSegmentDiameter = wireSegment.GetComponent<Renderer> ().bounds.size.x;
					}

					// HO=R Transform nib = trackedObject.transform.FindChild ("Nib");
					wireSegment.transform.position = nibPosition;
					previousWireSegmentPosition = nibPosition;

					currentWire.Add (wireSegment);
					wireSegment.wire = currentWire;
				}
			}
		}
	}

	public void ActivateWand(bool acivateWand) {

		if (acivateWand) {
			if (nib != null) {
				nib.SetActive (false);
			}
			if (wand != null) {
				wand.SetActive (true);
				StartCoroutine (BuildRing());
			}
		} else {
			if (nib != null) {
				nib.SetActive (true);
			}
			if (wand != null) {
				wand.SetActive (false);

				RingSegment[] ringSegments = ringCenter.GetComponentsInChildren<RingSegment> ();
				foreach (RingSegment ringSegment in ringSegments) {
					Destroy (ringSegment.gameObject);
				}
			}
			source.Stop ();
		}
		wandLastActive = acivateWand;
	}

	IEnumerator BuildRing() {

		for (int point = 0; point < totalRingPoints; point++) {
			Vector3 position = GetPointOnArc (Vector3.zero, 360, ringRadius, point, totalRingPoints);
			RingSegment ringSegment = Instantiate (ringSegmentPrefab);
			ringSegment.wandController = this;
			ringSegment.transform.parent = ringCenter.transform;
			ringSegment.transform.localScale = Vector3.one;
			ringSegment.transform.localPosition = position;

			yield return null;
		}
	}

	private Vector3 GetPointOnArc ( Vector3 center, float arcLength, float radius, int nPoint, int totalPoints){

		if (totalPoints < 2) {
			Vector3 position = new Vector3 (center.x, center.y, center.z + radius); // HO-R must test
			return position;
		}

		float step = arcLength / (totalPoints /* - 1*/);
		float ang = step * nPoint - (arcLength / 2);// - 90;//(90 - arcLength / 2);// /*90*/50;

		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y;// + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}

	public void Vibrate() {

		StartCoroutine (PlayBuzzSound());
		shouldVibrate = true;
		StartCoroutine (StopVibrateAfterTime (.3f));
	}

	IEnumerator PlayBuzzSound() {
		if (buzzSound != null) {
			source.PlayOneShot (buzzSound, 2);
		}
		yield return null;
	}

	public void PlayDoneSound() {
		if (dingSound != null) {
			source.PlayOneShot (dingSound, 2);
		}
	}

	public void PlayPassedSound() {
		if (passedSound != null) {
			source.PlayOneShot (passedSound, .5f);
			source.Play ();
		}
	}

	IEnumerator StopVibrateAfterTime(float time) {
		yield return new WaitForSeconds (time);
		shouldVibrate = false;
		source.Stop ();
	}
#endif
}
