using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float loopInterval = 1.0f;
	[SerializeField] private float dragSpeed = 3.0f;

	[Header("Info Panel")]
	[SerializeField] private RectTransform panel;
	[SerializeField] private TextMeshProUGUI clipNameText;
	[SerializeField] private TextMeshProUGUI distanceText;

	private AudioSource audioSource;
	private AudioListener audioListener;
	private AudioSourceManager audioSourceManager;

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

	private void ControlPlayback()
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
	}

	private void UpdateInfoPanel()
	{
		if (panel.gameObject.activeInHierarchy)
		{
			panel.transform.position = Camera.main.WorldToScreenPoint(audioSource.transform.position);
			clipNameText.text = "Clip: " + audioSource.clip.name;
			distanceText.text = string.Format("Distance: {0:0.00}m", Vector3.Distance(audioSource.transform.position, audioListener.transform.position));
		}
	}

	private void HandleInput()
	{
		if (panel.gameObject.activeInHierarchy)
		{
			if (Input.GetKey(KeyCode.M))
			{
				transform.position += new Vector3(Input.mousePositionDelta.x, 0.0f, Input.mousePositionDelta.y) * dragSpeed * Time.deltaTime;
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				audioSource.clip = audioSourceManager.GetPreviousClip(audioSource.clip);
			}

			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				audioSource.clip = audioSourceManager.GetNextClip(audioSource.clip);
			}
		}
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		audioListener = GameObject.FindGameObjectWithTag("AudioListener").GetComponent<AudioListener>();
		audioSourceManager = GameObject.FindGameObjectWithTag("AudioSourceManager").GetComponent<AudioSourceManager>();

		if (audioSource.playOnAwake)
		{
			started = true;
		}
	}

	private void Update()
	{
		ControlPlayback();
		UpdateInfoPanel();
		HandleInput();
	}
}
