using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float loopInterval = 1.0f;

	private AudioSource audioSource;

	private float finishTime = 0.0f;
	private bool started = false;

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
	}
}
