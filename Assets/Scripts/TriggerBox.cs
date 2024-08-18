using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBox : MonoBehaviour
{
    [SerializeField]
    bool TriggerOnce = false;

	[SerializeField]
	bool Loop = false;

	[SerializeField]
    string TriggerObject = "Shadow Caster";

	[SerializeField]
	AudioSource Source = null;

	[SerializeField]
    AudioClip Clip = null;

    [SerializeField]
    TextFade Text = null;

    [SerializeField]
    string nextScene = null;

	private void OnTriggerEnter(Collider other)
	{
		if (other != null)
        {
            Debug.Log("Collided with " + other.gameObject.name);
            if (other.gameObject.name == TriggerObject)
            {
                if (!string.IsNullOrEmpty(nextScene))
                {
                    SceneManager.LoadScene(nextScene);
                    return;
                }

                if (Clip != null)
                {
                    if (Source == null)
                    {
                        Debug.LogError("No audioSource linked to the triggerbox");
                        return;
                    }
                    else
                    {
                        Source.clip = Clip;
                        Source.loop = Loop;
                        Source.Play();
                    }
                }

				if (Text != null)
                {
                    Text.FadeIn();
                }
                if (TriggerOnce)
                    Destroy(this);
            }
		}
	}
}

