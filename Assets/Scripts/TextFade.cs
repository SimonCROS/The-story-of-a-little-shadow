using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
	[SerializeField]
	TextMeshPro text;

	public void FadeIn()
	{
		StartCoroutine(Fade());
	}

	private IEnumerator Fade()
	{
		for (float alpha = 0f; alpha < 1f; alpha += 0.01f)
		{
			text.alpha = alpha;
			yield return new WaitForSeconds(.1f);
		}
	}
}
