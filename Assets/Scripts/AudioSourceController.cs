using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioSourceController : MonoBehaviour
{
	[SerializeField] private float loopInterval = 1.0f;
	[SerializeField] private float rotationSpeed = 10.0f;
	[SerializeField] private float dragSpeed = 3.0f;
	[SerializeField] private MeshRenderer audioVisualizerRenderer;
	[SerializeField] private LayerMask groundLayer;

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

	private Controls controls;
	private AudioListener audioListener;
	private AudioSourceManager audioSourceManager;

	private float panelLerpSpeed = 8.0f;
	private float finishTime = 0.0f;
	private bool startedPlayback = false;
	private float volumeChangeGranularity = 0.05f;

	private float minHeight = 0.3f;
	private float maxHeight = 3.0f;

	public bool isPaused { get; private set; } = false;

	#region Public methods
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
		isPaused = true;
		audioSource.Pause();
	}

	public void Resume()
	{
		isPaused = false;
		audioSource.UnPause();
	}

	public void TogglePause()
	{
		isPaused = !isPaused;

		if (isPaused)
		{
			audioSource.Pause();
		}
		else
		{
			audioSource.UnPause();
		}
	}

	public void IncreaseVolume()
	{
		audioSource.volume = Mathf.Clamp(audioSource.volume + volumeChangeGranularity, 0.0f, 1.0f);
	}

	public void DecreaseVolume()
	{
		audioSource.volume = Mathf.Clamp(audioSource.volume - volumeChangeGranularity, 0.0f, 1.0f);
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
	#endregion

	#region Private methods
	private void ControlPlayback()
	{
		if (isPaused)
		{
			return;
		}

		if (!audioSource.isPlaying && startedPlayback)
		{
			finishTime = Time.time;
			startedPlayback = false;
		}

		if (Time.time > finishTime + loopInterval && !startedPlayback)
		{
			audioSource.Play();
			startedPlayback = true;
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
			volumeText.text = string.Format("Volume: {0:0.00}", audioSource.volume);
			intervalText.text = string.Format("Interval: {0}", loopInterval);
			heightPanel.text = string.Format("Relative height: {0:0.0}m", audioSource.transform.position.y - audioListener.transform.position.y);
			pauseText.text = isPaused ? "Resume" : "Pause";
		}
	}

	private void HandleInput()
	{
		float deltaX = Mouse.current.delta.x.ReadValue();
		float deltaY = Mouse.current.delta.y.ReadValue();

		if (panel.gameObject.activeInHierarchy)
		{
			Vector3 oldPos = transform.position;

			if (controls.Global.EnableMove.ReadValue<float>() > 0.5f)
			{
				transform.position += new Vector3(deltaX, 0.0f, deltaY) * dragSpeed * Time.deltaTime;
			}

			if (controls.Global.EnableHeightAdjustment.ReadValue<float>() > 0.5f)
			{
				transform.position += new Vector3(0.0f, deltaY, 0.0f) * dragSpeed * Time.deltaTime;
				transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minHeight, maxHeight), transform.position.z);

			}

			if (controls.Global.EnableRotate.ReadValue<float>() > 0.5f)
			{
				transform.parent.Rotate(Vector3.up * deltaX * rotationSpeed * Time.deltaTime);
			}

			if (!Physics.Raycast(transform.position, Vector3.down, 100.0f, groundLayer))
			{
				transform.position = oldPos;
			}
		}
	}

	private void VisualizeAudio()
	{
		Color targetColor = Color32.Lerp(Color.white, Color.red, audioLoudnessChecker.NormalizedLoudness);
		audioVisualizerMaterial.color = targetColor;
	}
	#endregion

	#region Unity messages
	private void Awake()
	{
		controls = new Controls();
		controls.Enable();

		controls.Global.VolumeUp.performed += VolumeUp_performed;
		controls.Global.VolumeDown.performed += VolumeDown_performed;
		controls.Global.NextClip.performed += NextClip_performed;
		controls.Global.PreviousClip.performed += PreviousClip_performed;
		controls.Global.Delete.performed += Delete_performed;
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
			startedPlayback = true;
		}
	}

	private void OnDestroy()
	{
		controls.Disable();
	}

	private void Update()
	{
		ControlPlayback();
		UpdateInfoPanel();
		HandleInput();
		VisualizeAudio();
	}
	#endregion

	#region Input events
	private void VolumeUp_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		IncreaseVolume();
	}

	private void VolumeDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		DecreaseVolume();
	}

	private void PreviousClip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		audioSource.clip = audioSourceManager.GetPreviousClip(audioSource.clip);
	}

	private void NextClip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		audioSource.clip = audioSourceManager.GetNextClip(audioSource.clip);
	}

	private void Delete_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
	{
		Destroy(gameObject);
	}
	#endregion
}
