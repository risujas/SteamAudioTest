using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float interval = 1.0f;

	private AudioSource audioSource;

	private float finishTime = 0.0f;
	private bool started = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!audioSource.isPlaying && started)
		{
			finishTime = Time.time;
			started = false;
		}

		if (Time.time > finishTime + interval && !started)
		{
			audioSource.Play();
			started = true;
		}
	}
}
