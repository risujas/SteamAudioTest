using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioSourceManager : MonoBehaviour
{
	[Header("Clips")]
	[SerializeField] private List<AudioClip> availableClips;

	[Header("Placement")]
	[SerializeField] private GameObject audioControllerPrefab;
	[SerializeField] private GameObject placementIndicatorPrefab;
	[SerializeField] private LayerMask placementSurfaceLayer;
	[SerializeField] private TextMeshProUGUI placementButtonText;
	[SerializeField] private TextMeshProUGUI deletionButtonText;

	[Header("Selection")]
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private ObjectSelector objectSelector;

	private List<AudioSourceController> selectedControllers = new List<AudioSourceController>();
	private GameObject placementIndicator;
	private bool isPlacingController;
	private bool isDeletingController;

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
		isPlacingController = enable;

		if (isPlacingController)
		{
			placementButtonText.text = "Cancel";
			placementButtonText.fontStyle = FontStyles.Italic;

			if (placementIndicator == null)
			{
				placementIndicator = Instantiate(placementIndicatorPrefab, Vector3.zero, Quaternion.identity);
			}
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
	}

	public void EnableDeletion(bool enable)
	{
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
		}

		if (enable)
		{
			EnablePlacement(false);
		}
	}

	public void PauseAllControllers(bool pause)
	{
		var allControllers = FindObjectsByType<AudioSourceController>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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
		if (!availableClips.Contains(currentClip))
		{
			return availableClips[0];
		}

		int index = availableClips.IndexOf(currentClip);
		return GetClipByIndex(index + 1);
	}

	public AudioClip GetPreviousClip(AudioClip currentClip)
	{
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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, placementSurfaceLayer))
		{
			placementIndicator.transform.position = hit.point;

			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				var controller = Instantiate(audioControllerPrefab, hit.point + Vector3.up, Quaternion.identity).GetComponent<AudioSourceController>();
				controller.EnableInfoPanel(true);
				selectedControllers.Add(controller);

				EnablePlacement(false);
			}
		}
	}

	private void HandleDeletion()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, audioSourceLayerMask))
		{
			if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				var controller = hit.collider.gameObject.GetComponentInParent<AudioSourceController>();
				Destroy(controller.gameObject);
				EnableDeletion(false);
			}
		}
	}

	private void HandleSelection()
	{
		bool multiSelect = Input.GetKey(KeyCode.LeftControl);

		var selection = objectSelector.SelectedObjects;
		if (selection.Count > 0)
		{
			if (!multiSelect)
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

	private void Update()
	{
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
	}
}
