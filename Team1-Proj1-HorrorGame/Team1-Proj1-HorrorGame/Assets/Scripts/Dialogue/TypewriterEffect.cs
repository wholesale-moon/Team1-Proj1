using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] Text text;
	[SerializeField] TMP_Text tmpProText;
	string writer;
	[SerializeField] private Coroutine coroutine;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;
	[Space(10)] [SerializeField] private bool startOnEnable = false;

	[SerializeField] Button contButton;
	[SerializeField] AudioSource audio;

	// is there a way to ignore rich text tags?
	
	void Awake()
	{
		if(text != null)
		{
			writer = text.text;
		}
		
		if (tmpProText != null)
		{
			writer = tmpProText.text;
		}
	}

	void Start()
	{
		// if (!clearAtStart ) return;
		if(text != null)
		{
			text.text = "";
		}
		
		if (tmpProText != null)
		{
			tmpProText.text = "";
		}
	}

	private void OnEnable()
	{
		if(text != null)
		{
			writer = text.text;
		}
		
		if (tmpProText != null)
		{
			writer = tmpProText.text;
		}

		contButton.gameObject.SetActive(false);
		if(startOnEnable) StartTypewriter();
	}

	private void StartTypewriter()
	{
		StopAllCoroutines();

		if(text != null)
		{
			text.text = "";

			StartCoroutine("TypeWriterText");
		}
		
		if (tmpProText != null)
		{
			tmpProText.text = "";

			StartCoroutine("TypeWriterTMP");
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	IEnumerator TypeWriterText()
	{
		text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (text.text.Length > 0)
			{
				text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
			}
			text.text += c;
			text.text += leadingChar;
			yield return new WaitForSeconds(timeBtwChars);
		}

		if(leadingChar != "")
        {
			text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
		}

		yield return null;
	}

	IEnumerator TypeWriterTMP()
    {
	    tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

		foreach (char c in writer)
		{
			if (tmpProText.text.Length > 0)
			{
				tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
			}
			tmpProText.text += c;
			tmpProText.text += leadingChar;
			audio.Play();
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
		}

		contButton.gameObject.SetActive(true);
	}
}
