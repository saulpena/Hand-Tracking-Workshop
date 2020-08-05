using UnityEngine;
using System.Collections;

public class ElectricBar : MonoBehaviour {

	// public GameLane gameLane;

	public Transform baseObject;
	public Transform barBody;
	public Transform backEnd;
	public Transform frontEnd;

	public AudioClip dingSound;
	public AudioClip passedSound;

	public Material passedMaterial;
	public Material doneMaterial;

	string passedEndTag = null;
	bool hasBeenShocked = false;

	// factor this, as it's same in WireSegment
	Renderer barBodyRenderer;
	private Color startColorForFade;
	private Color endColorForFade;
	private float startTime;
	public bool isDone = false;
	private float fadeSpeed = 2;

	void Awake () {

		barBodyRenderer = barBody.GetComponent<Renderer> ();
	}

	public void SetLength(float length) {
		Vector3 barBodyScale = barBody.localScale;
		barBodyScale.y = length;
		barBody.localScale = barBodyScale;
		Vector3 endPosition = backEnd.localPosition;
		endPosition.z = length;
		backEnd.localPosition = endPosition;
		endPosition.z = -length;
		frontEnd.localPosition = endPosition;
	}

	public void SetRotation(Quaternion rotation) {
		baseObject.localRotation = rotation;
	}

	void Update () {
		if (!hasBeenShocked /* && !GameManager.instance.frozen*/) {

			float speed = GameManager.instance.electricBarSpeed * GameManager.instance.electricBarSpeedMultiplier;
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			if (transform.position.z > 10f) {
				Destroy (gameObject);
			} else if (isDone) {

				float fracFade = (Time.time - startTime) / fadeSpeed;
				barBodyRenderer.material.color = Color.Lerp (startColorForFade, endColorForFade, fracFade);

				if (barBodyRenderer.material.color == endColorForFade) {
					Destroy (gameObject/*.transform.parent.gameObject*/);
					// HO-R when all are done destroy the parent
				}
			}
		}
	}

	public void PassedBarEnd(BarEnd barEnd, RingTriggerArea ringTriggerArea) {

		if (passedEndTag == null) {
			barBody.GetComponent<Renderer> ().material = passedMaterial;
			ringTriggerArea.PlayPassedSound ();

			passedEndTag = barEnd.tag;
		} else {
			
			if (!barEnd.CompareTag(passedEndTag)) {

				barBody.GetComponent<Renderer> ().material = doneMaterial;
				ringTriggerArea.PlayDoneSound ();


				float length = barBody.localScale.y;
				int score = (length > .4f) ? 20 : 10;
				if (baseObject.localRotation != Quaternion.identity) {
					score += 5;
				}
				//gameLane.UpdateScore (score);

				isDone = true;

				startColorForFade = barBodyRenderer.material.color;
				endColorForFade = startColorForFade;
				endColorForFade.a = 0;

				startTime = Time.time;

			}
				
			passedEndTag = null;
		}
	}

	public void Shocked() {

		if (!hasBeenShocked) {
			hasBeenShocked = true;

			if (barBody != null) {
				GameObject parentGameObject = barBody.parent.gameObject;
				barBody.parent = null;
				Rigidbody rb = barBody.GetComponent<Rigidbody> ();

				rb.useGravity = true;
				rb.isKinematic = false;
				barBody.tag = "Untagged";

				Destroy (frontEnd.gameObject);
				Destroy (backEnd.gameObject);
				Destroy (parentGameObject);
				Destroy (gameObject);

				//gameLane.deadElectricBarBodies.Add (barBody);
			}
		}
	}

	public void BlowOutABulb() {
		//gameLane.BlowOutABulb ();
	}
		
	/* HO-R
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
	*/
}
