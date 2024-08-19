using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    [SerializeField]
    Color SpotLightColor = Color.white;

    [SerializeField]
    Light spotLight;

    [SerializeField]
    private GameObject Sword;

    [SerializeField]
    private List<MonoBehaviour> enableMonoBehaviours = new List<MonoBehaviour>();

    public void LoadScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other == null)
        {
            return;
        }

        if (other.gameObject.name != TriggerObject)
        {
            return;
        }

        if (!string.IsNullOrEmpty(nextScene))
        {
            LoadScene(nextScene);
            return;
        }

        if (SpotLightColor != Color.white)
        {
            spotLight.color = SpotLightColor;
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

        if (Sword != null)
        {
            other.gameObject.SendMessage("UnlockSword");
            Sword.SetActive(false);
        }

        foreach (var mb in enableMonoBehaviours)
        {
            mb.enabled = true;
        }

        if (TriggerOnce)
        {
            Destroy(this);
        }
	}
}

