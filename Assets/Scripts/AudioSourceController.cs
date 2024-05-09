using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float loopInterval = 1.0f;
	[SerializeField] private float dragSpeed = 3.0f;
	[SerializeField] private MeshRenderer audioVisualizerRenderer;

	[Header("Info Panel")]
	[SerializeField] private RectTransform panel;
	[SerializeField] private TextMeshProUGUI clipNameText;
	[SerializeField] private TextMeshProUGUI distanceText;
	[SerializeField] private TextMeshProUGUI intervalText;
	[SerializeField] private TextMeshProUGUI volumeText;
	[SerializeField] private TextMeshProUGUI heightPanel;
	[SerializeField] private TextMeshProUGUI pauseText;

	private AudioSource audioSource;
	private AudioLoudnessChecker audioLoudnessChecker;
	private Material audioVisualizerMaterial;

	private AudioListener audioListener;
	private AudioSourceManager audioSourceManager;

	private float panelLerpSpeed = 8.0f;
	private float finishTime = 0.0f;
	private bool started = false;

	public bool Paused { get; set; } = false;

	public void PlayPreviousClip()
	{
		audioSource.clip = audioSourceManager.GetNextClip(audioSource.clip);
	}

	public void PlayNextClip()
	{
		audioSource.clip = audioSourceManager.GetPreviousClip(audioSource.clip);
	}

	public void Pause()
	{
		Paused = true;
		audioSource.Pause();
	}

	public void Resume()
	{
		Paused = false;
		audioSource.UnPause();
	}

	public void TogglePause()
	{
		Paused = !Paused;

		if (Paused)
		{
			audioSource.Pause();
		}
		else
		{
			audioSource.UnPause();
		}
	}

	public void ChangeVolume(float value)
	{
		audioSource.volume = Mathf.Clamp(audioSource.volume + value, 0.0f, 1.0f);
	}

	public void ChangeInterval(float value)
	{
		loopInterval += value;

		if (loopInterval < 0.0f)
		{
			loopInterval = 0.0f;
		}
	}

	public void EnableInfoPanel(bool enabled)
	{
		if (enabled)
		{
			panel.transform.position = Camera.main.WorldToScreenPoint(transform.position);
		}

		panel.gameObject.SetActive(enabled);
	}

	private void ControlPlayback()
	{
		if (Paused)
		{
			return;
		}

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
			Vector3 cameraPos = Camera.main.transform.position;
			cameraPos.y = transform.position.y;

			float xDif = transform.position.x - cameraPos.x;
			float offset = panel.sizeDelta.x;
			if (xDif < 0.0f) offset *= -1;

			Vector3 targetPos = Camera.main.WorldToScreenPoint(audioSource.transform.position);
			targetPos.x += offset;

			panel.transform.position = Vector3.Lerp(panel.transform.position, targetPos, Time.deltaTime * panelLerpSpeed);

			clipNameText.text = string.Format("Clip: {0}", audioSource.clip.name);
			distanceText.text = string.Format("Distance: {0:0.00}m", Vector3.Distance(audioSource.transform.position, audioListener.transform.position));
			volumeText.text = string.Format("Volume: {0:0.0}", audioSource.volume);
			intervalText.text = string.Format("Interval: {0}", loopInterval);
			heightPanel.text = string.Format("Relative height: {0:0.0}m", audioSource.transform.position.y - audioListener.transform.position.y);
			pauseText.text = Paused ? "Resume" : "Pause";
		}
	}

	private void HandleInput()
	{
		if (panel.gameObject.activeInHierarchy)
		{
			if (Input.GetKey(KeyCode.M) || Input.GetMouseButton(1))
			{
				transform.position += new Vector3(Input.mousePositionDelta.x, 0.0f, Input.mousePositionDelta.y) * dragSpeed * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.H) || Input.GetMouseButton(2))
			{
				transform.position += new Vector3(0.0f, Input.mousePositionDelta.y, 0.0f) * dragSpeed * Time.deltaTime;
			}

			if (Input.GetKey(KeyCode.R))
			{
				Vector3 dir = (audioListener.transform.position - transform.position).normalized;
				Vector3 cross = Vector3.Cross(dir, Vector3.up);
				transform.position += Input.mousePositionDelta.x * cross * dragSpeed * Time.deltaTime;
			}

			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				audioSource.clip = audioSourceManager.GetPreviousClip(audioSource.clip);
			}

			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				audioSource.clip = audioSourceManager.GetNextClip(audioSource.clip);
			}

			if (Input.GetKeyDown(KeyCode.Delete))
			{
				Destroy(gameObject);
			}

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				ChangeVolume(0.1f);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				ChangeVolume(-0.1f);
			}
		}
	}

	private void VisualizeAudio()
	{
		Color targetColor = Color32.Lerp(Color.white, Color.red, audioLoudnessChecker.NormalizedLoudness);
		audioVisualizerMaterial.color = targetColor;
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioLoudnessChecker = GetComponent<AudioLoudnessChecker>();
		audioVisualizerMaterial = audioVisualizerRenderer.material;

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
		VisualizeAudio();
	}
}
