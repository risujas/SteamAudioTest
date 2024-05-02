using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private List<AudioClip> availableClips;
	[SerializeField] private ObjectSelector objectSelector;

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

	private void HandleInput()
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
		HandleInput();
	}
}
