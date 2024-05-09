using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class AudioSourceManager : MonoBehaviour
{
	[SerializeField] private List<AudioClip> availableClips;
	[SerializeField] private TextMeshProUGUI globalHintText;

	[Header("Placement")]
	[SerializeField] private GameObject audioControllerPrefab;
	[SerializeField] private GameObject placementIndicatorPrefab;
	[SerializeField] private LayerMask placementSurfaceLayer;
	[SerializeField] private TextMeshProUGUI placementButtonText;
	[SerializeField] private TextMeshProUGUI deletionButtonText;

	[Header("Selection")]
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private ObjectSelector objectSelector;

	private string userAudioFolder = "UserAudio";

	private List<AudioSourceController> selectedControllers = new List<AudioSourceController>();
	private List<AudioSourceController> allControllers = new List<AudioSourceController>();

	private GameObject placementIndicator;
	private bool isPlacingController;
	private bool isDeletingController;
	private bool usedMultipleActions = false;

	private Controls controls;
	private bool multiAction;

	public void TogglePlacement()
	{
		EnablePlacement(!isPlacingController);
	}

	public void ToggleDeletion()
	{
		EnableDeletion(!isDeletingController);
	}

	public void EnablePlacement(bool enable)
	{
		usedMultipleActions = false;
		isPlacingController = enable;

		if (isPlacingController)
		{
			placementButtonText.text = "Cancel";
			placementButtonText.fontStyle = FontStyles.Italic;

			if (placementIndicator == null)
			{
				placementIndicator = Instantiate(placementIndicatorPrefab, Vector3.zero, Quaternion.identity);
			}

			DeselectControllers();
		}
		else
		{
			placementButtonText.text = "Create";
			placementButtonText.fontStyle = FontStyles.Normal;

			if (placementIndicator)
			{
				Destroy(placementIndicator);
			}
		}

		if (enable)
		{
			EnableDeletion(false);
		}
		EnableSelection(!enable);

		globalHintText.text = enable ? "Hold Control to place multiple objects." : "";
	}

	public void EnableDeletion(bool enable)
	{
		usedMultipleActions = false;
		isDeletingController = enable;

		if (isDeletingController)
		{
			deletionButtonText.text = "Cancel";
			deletionButtonText.fontStyle = FontStyles.Italic;
		}
		else
		{
			deletionButtonText.text = "Delete";
			deletionButtonText.fontStyle = FontStyles.Normal;

			globalHintText.text = string.Empty;
		}

		if (enable)
		{
			EnablePlacement(false);
		}
		EnableSelection(!enable);

		globalHintText.text = enable ? "Hold Control to delete multiple objects." : "";
	}

	public void EnableSelection(bool enable)
	{
		DeselectControllers();
		objectSelector.enabled = enable;
	}

	public void PauseAllControllers(bool pause)
	{
		foreach (var controller in allControllers)
		{
			if (pause)
			{
				controller.Pause();
			}
			else
			{
				controller.Resume();
			}
		}
	}

	public AudioClip GetNextClip(AudioClip currentClip)
	{
		if (availableClips.Count == 0)
		{
			return null;
		}

		if (!availableClips.Contains(currentClip))
		{
			return availableClips[0];
		}

		int index = availableClips.IndexOf(currentClip);
		return GetClipByIndex(index + 1);
	}

	public AudioClip GetPreviousClip(AudioClip currentClip)
	{
		if (availableClips.Count == 0)
		{
			return null;
		}

		if (!availableClips.Contains(currentClip))
		{
			return availableClips[0];
		}

		int index = availableClips.IndexOf(currentClip);
		return GetClipByIndex(index - 1);
	}

	public AudioClip GetClipByIndex(int index)
	{
		if (index < 0)
		{
			index = availableClips.Count - 1;
		}
		else
		{
			index %= availableClips.Count;
		}

		return availableClips[index];
	}

	public IEnumerator LoadUserAudioClips()
	{
		string folderPath = Path.Combine(Application.persistentDataPath, userAudioFolder);
		List<AudioClip> userAudioClips = new List<AudioClip>();

		if (Directory.Exists(folderPath))
		{
			string[] fileEntries = Directory.GetFiles(folderPath);

			foreach (var filePath in fileEntries)
			{
				if (filePath.EndsWith(".wav") || filePath.EndsWith(".mp3") || filePath.EndsWith(".ogg"))
				{
					using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.UNKNOWN))
					{
						yield return request.SendWebRequest();

						if (request.result == UnityWebRequest.Result.Success)
						{
							AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
							clip.name = Path.GetFileNameWithoutExtension(filePath);
							userAudioClips.Add(clip);
						}
						else
						{
							Debug.LogError(request.error);
						}
					}
				}
			}
		}

		foreach (var clip in userAudioClips)
		{
			availableClips.Add(clip);
		}
	}


	private void DeselectControllers()
	{
		if (selectedControllers != null && selectedControllers.Count > 0)
		{
			for (int i = selectedControllers.Count - 1; i >= 0; i--)
			{
				if (selectedControllers[i] != null)
				{
					selectedControllers[i].EnableInfoPanel(false);
				}

				selectedControllers.Remove(selectedControllers[i]);
			}
		}
	}

	private void HandlePlacement()
	{
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placementSurfaceLayer))
		{
			placementIndicator.transform.position = hit.point;

			if (controls.Global.Select.WasReleasedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
			{
				var ascParent = Instantiate(audioControllerPrefab, Vector3.zero, Quaternion.identity);

				var controller = ascParent.GetComponentInChildren<AudioSourceController>();
				controller.transform.position = hit.point + Vector3.up * 1.0f;

				allControllers.Add(controller);
				selectedControllers.Add(controller);

				usedMultipleActions = true;
				if (!multiAction)
				{
					EnablePlacement(false);
				}
			}
		}
		if (usedMultipleActions && controls.Global.MultiAction.WasReleasedThisFrame())
		{
			EnablePlacement(false);
		}
	}

	private void HandleDeletion()
	{
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, audioSourceLayerMask))
		{
			if (controls.Global.Select.WasReleasedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
			{
				var controller = hit.collider.gameObject.GetComponentInParent<AudioSourceController>();

				allControllers.Remove(controller);
				selectedControllers.Remove(controller);
				Destroy(controller.gameObject);

				usedMultipleActions = true;
				if (!multiAction || allControllers.Count == 0)
				{
					EnableDeletion(false);
				}
			}
		}
		if (usedMultipleActions && controls.Global.MultiAction.WasReleasedThisFrame())
		{
			EnableDeletion(false);
		}
	}

	private void HandleSelection()
	{
		var selection = objectSelector.SelectedObjects;
		if (selection.Count > 0)
		{
			if (!multiAction)
			{
				DeselectControllers();
			}

			foreach (var o in selection)
			{
				var audioSourceController = o.GetComponent<AudioSourceController>();
				if (audioSourceController == null) audioSourceController = o.GetComponentInParent<AudioSourceController>();

				if (audioSourceController)
				{
					audioSourceController.EnableInfoPanel(true);
					selectedControllers.Add(audioSourceController);
				}
			}
		}
		else
		{
			DeselectControllers();
		}
	}

	private void Awake()
	{
		controls = new Controls();
		controls.Global.Enable();
	}

	private void Start()
	{
		StartCoroutine(LoadUserAudioClips());
	}

	private void Update()
	{
		multiAction = controls.Global.MultiAction.ReadValue<float>() > 0.5f;

		if (isPlacingController)
		{
			HandlePlacement();
		}
		else if (isDeletingController)
		{
			HandleDeletion();
		}
		else if (objectSelector.CompletedSelection)
		{
			HandleSelection();
		}
		else
		{
			string hint = "(R) Rotate - (M) Move - (H) Adjust height - (Delete) Delete selected objects";
			globalHintText.text = selectedControllers.Count > 0 ? hint : "";
		}

		allControllers = FindObjectsByType<AudioSourceController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();

		for (int i = selectedControllers.Count - 1; i >= 0; i--)
		{
			if (selectedControllers[i] == null)
			{
				selectedControllers.RemoveAt(i);
			}
		}
	}
}
