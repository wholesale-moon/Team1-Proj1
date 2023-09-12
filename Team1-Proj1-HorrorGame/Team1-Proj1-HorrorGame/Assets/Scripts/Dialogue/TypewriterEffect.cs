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
	[SerializeField] private bool _noSkip;

	[Space(10)]
	[SerializeField] private GameObject SceneManager;
	[SerializeField] private GameObject contButton;

	private bool isSkipCooldown = false;
	private bool isWriting;

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
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetButton("Controller A"))
		{
			if (!isWriting & !_isScreenText & !_noSkip)
			{
				SceneManager.GetComponent<DialogueManager>().DisplayNextSentence();
			}
			else if (!isSkipCooldown & isWriting & !_noSkip)
			{
				SkipDialogue();
			}
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
	    isWriting = true;
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
			if (_isScreenText)
				SceneManager.GetComponent<SoundManager>().PlayClipByName("DialogueAlt");
			else
				SceneManager.GetComponent<SoundManager>().PlayClipByName("Typewriter");
			yield return new WaitForSeconds(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
		}

		contButton.gameObject.SetActive(true);
		isWriting = false;
	}

	private void SkipDialogue()
	{
		StopAllCoroutines();
		tmpProText.text = writer;
		contButton.gameObject.SetActive(true);
		isWriting = false;
		StartCoroutine(SkipCooldown());
	}

	private IEnumerator SkipCooldown()
	{
		isSkipCooldown = true;
		yield return new WaitForSeconds(0.1f);
		isSkipCooldown = false;
		yield return null;
	}
}
