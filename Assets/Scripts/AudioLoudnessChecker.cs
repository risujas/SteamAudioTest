using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;

public class AudioLoudnessChecker : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private float updateStep = 0.1f;
	[SerializeField] private int sampleDataLength = 1024;
	[SerializeField] private float peakLoudnessUpdates = 32;

	private List<float> loudnessValues = new List<float>();
	private float peakLoudness = 0.0f;

	private float currentUpdateTime = 0f;
	private float[] sampleData;

	public float Loudness { get; private set; }

	public float NormalizedLoudness
	{
		get
		{
			return peakLoudness == 0.0f ? 0.0f : Mathf.Clamp01(Loudness / peakLoudness);
		}
	}

	private void CalculateLoudness()
	{
		currentUpdateTime += Time.deltaTime;
		if (currentUpdateTime >= updateStep)
		{
			currentUpdateTime = 0f;
			audioSource.GetOutputData(sampleData, 0);

			Loudness = 0f;
			foreach (var sample in sampleData)
			{
				Loudness += Mathf.Abs(sample);
			}

			Loudness /= sampleDataLength;

			HandlePeakLoudness();
		}
	}

	private void HandlePeakLoudness()
	{
		loudnessValues.Add(Loudness);
		if (loudnessValues.Count > peakLoudnessUpdates)
		{
			loudnessValues.RemoveAt(0);
		}

		float highest = 0.0f;
		foreach (var x in loudnessValues)
		{
			if (x > highest)
			{
				highest = x;
			}
		}

		peakLoudness = highest;
	}

	private void Awake()
	{
		sampleData = new float[sampleDataLength];
	}

	private void Update()
	{
		CalculateLoudness();
	}
}
