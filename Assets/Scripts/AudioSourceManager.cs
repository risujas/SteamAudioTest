using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private List<AudioClip> availableClips;
	[SerializeField] private BoxSelector boxSelector;

	private List<AudioSourceController> selectedControllers = new List<AudioSourceController>();

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

	private void HandleInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			bool multiSelect = Input.GetKey(KeyCode.LeftControl);

			var boxSelection = boxSelector.SelectedObjects;
			if (boxSelection.Count > 0)
			{
				if (!multiSelect)
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

				foreach (var o in boxSelection)
				{
					var audioSourceController = o.GetComponent<AudioSourceController>();
					if (audioSourceController)
					{
						audioSourceController.EnableInfoPanel(true);
						selectedControllers.Add(audioSourceController);
					}
				}
			}
		}
	}

	private void Update()
	{
		HandleInput();
	}
}
