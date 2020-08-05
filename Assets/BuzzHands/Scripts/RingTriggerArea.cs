using UnityEngine;
using System.Collections;

public class RingTriggerArea : MonoBehaviour {

    public AudioClip buzzSound;
    public AudioClip dingSound;
    public AudioClip passedSound;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

    }

    public void PlayBuzzSound()
    {
        StartCoroutine(Buzz());
    }

    IEnumerator Buzz()
    {
        if (buzzSound != null)
        {
            source.PlayOneShot(buzzSound, 2);
        }
        yield return null;
    }

    public void PlayDoneSound()
    {
        if (dingSound != null)
        {
            source.PlayOneShot(dingSound, 2);
        }
    }

    public void PlayPassedSound()
    {
        if (passedSound != null)
        {
            source.PlayOneShot(passedSound, .5f);
            source.Play();
        }
    }

}
