using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float loopInterval = 1.0f;

	[Header("Info Panel")]
	[SerializeField] private Canvas canvas;
	[SerializeField] private RectTransform panel;
	[SerializeField] private TextMeshProUGUI clipNameText;

	private AudioSource audioSource;
	private float finishTime = 0.0f;
	private bool started = false;

	public void EnableInfoPanel(bool enabled)
	{
		panel.gameObject.SetActive(enabled);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		if (audioSource.playOnAwake)
		{
			started = true;
		}
	}

	private void Update()
	{
		if (!audioSource.isPlaying && started)
		{
			finishTime = Time.time;
			started = false;
		}

		if (Time.time > finishTime + loopInterval && !started)
		{
			audioSource.Play();
			started = true;
		}

		if (panel.gameObject.activeInHierarchy)
		{
			panel.transform.position = Camera.main.WorldToScreenPoint(audioSource.transform.position);
			clipNameText.text = "Clip: " + audioSource.clip.name;
		}
	}
}
