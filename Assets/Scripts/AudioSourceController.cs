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
	[SerializeField] private TextMeshProUGUI volumeText;
	[SerializeField] private TextMeshProUGUI heightPanel;

	private AudioSource audioSource;
	private AudioLoudnessChecker audioLoudnessChecker;
	private Material audioVisualizerMaterial;

	private AudioListener audioListener;
	private AudioSourceManager audioSourceManager;

	private float finishTime = 0.0f;
	private bool started = false;

	public void ChangeVolume(float value)
	{
		audioSource.volume = Mathf.Clamp(audioSource.volume + value, 0.0f, 1.0f);
	}

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

			clipNameText.text = string.Format("Clip: {0}", audioSource.clip.name);
			distanceText.text = string.Format("Distance: {0:0.00}m", Vector3.Distance(audioSource.transform.position, audioListener.transform.position));
			volumeText.text = string.Format("Volume: {0:0.0}", audioSource.volume);
			heightPanel.text = string.Format("Relative height: {0:0.0}m", audioSource.transform.position.y - audioListener.transform.position.y);
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
				transform.LookAt(audioListener.transform);
				Vector3 crossProduct = Vector3.Cross(transform.forward, Vector3.up);
				transform.position += Input.mousePositionDelta.x * crossProduct * dragSpeed * Time.deltaTime;
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
