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
	[SerializeField] private TextMeshProUGUI distanceText;

	private AudioSource audioSource;
	private AudioListener audioListener;

	private float finishTime = 0.0f;
	private bool started = false;

	public void EnableInfoPanel(bool enabled)
	{
		if (enabled)
		{
			panel.transform.position = Camera.main.WorldToScreenPoint(audioSource.transform.position);
		}

		panel.gameObject.SetActive(enabled);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioListener = GameObject.FindGameObjectWithTag("AudioListener").GetComponent<AudioListener>();

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
			distanceText.text = string.Format("Distance: {0:0.00}m", Vector3.Distance(audioSource.transform.position, audioListener.transform.position));
		}
	}
}
