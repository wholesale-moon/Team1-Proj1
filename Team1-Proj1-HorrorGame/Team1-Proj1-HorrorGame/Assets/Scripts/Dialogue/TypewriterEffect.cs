using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Text")]
	[SerializeField] TMP_Text tmpProText;
	string writer;
	[SerializeField] private Coroutine coroutine;

	[Header("Settings")]
	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;
	[Space(10)] [SerializeField] private bool startOnEnable = false;
	[Space(10)] [SerializeField] private bool _isScreenText;

	[Space(10)]
	[SerializeField] private GameObject SceneManager;
	[SerializeField] private GameObject contButton;

	// is there a way to ignore rich text tags?
	
	void Awake()
	{
		if (tmpProText != null)
		{
			writer = tmpProText.text;
		}
	}

	void Start()
	{
		// if (!clearAtStart ) return;	
		if (tmpProText != null)
		{
			tmpProText.text = "";
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			if (!_isScreenText)
				SkipDialogue();
		}
	}

	private void OnEnable()
	{
		if (tmpProText != null)
		{
			writer = tmpProText.text;
		}

		contButton.SetActive(false);
		if(startOnEnable) StartTypewriter();
	}

	private void StartTypewriter()
	{
		StopAllCoroutines();
		
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
			SceneManager.GetComponent<SoundManager>().PlayClipByName("Typewriter");
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
		}

		contButton.gameObject.SetActive(true);
	}

	private void SkipDialogue()
	{
		StopAllCoroutines();
		tmpProText.text = writer;
		contButton.gameObject.SetActive(true);
	}
}
