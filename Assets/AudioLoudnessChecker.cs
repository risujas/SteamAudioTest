using NUnit.Framework.Interfaces;
using UnityEngine;

public class AudioLoudnessChecker : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private float updateStep = 0.1f;
	[SerializeField] private int sampleDataLength = 1024;

	private float peakLoudness = 0.0f;

	private float currentUpdateTime = 0f;
	private float[] sampleData;

	public float Loudness { get; private set; }

	public float NormalizedLoudness
	{
		get
		{
			return Mathf.Clamp01(Loudness / peakLoudness);
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
			if (Loudness >= peakLoudness)
			{
				peakLoudness = Loudness;
			}
		}
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
