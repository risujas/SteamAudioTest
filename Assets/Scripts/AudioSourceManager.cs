using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
	[Header("Clips")]
	[SerializeField] private List<AudioClip> availableClips;

	[Header("Placement")]
	[SerializeField] private GameObject audioControllerPrefab;
	[SerializeField] private GameObject placementIndicatorPrefab;

	[Header("Selection")]
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private ObjectSelector objectSelector;

	private List<AudioSourceController> selectedControllers = new List<AudioSourceController>();

	public bool isPlacingController { get; private set; }

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

	}

	private void HandleSelection()
	{
		if (objectSelector.CompletedSelection)
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
	}

	private void Update()
	{
		if (isPlacingController)
		{
			HandlePlacement();
		}
		else
		{
			HandleSelection();
		}
	}
}
