using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private List<AudioClip> availableClips;

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

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			bool multiSelect = Input.GetKey(KeyCode.LeftControl);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, audioSourceLayerMask))
			{
				var audioSourceController = hit.collider.gameObject.GetComponentInParent<AudioSourceController>();
				if (audioSourceController)
				{
					if (!multiSelect)
					{
						for (int i = selectedControllers.Count - 1; i >= 0; i--)
						{
							selectedControllers[i].EnableInfoPanel(false);
							selectedControllers.Remove(selectedControllers[i]);
						}
					}

					if (!selectedControllers.Contains(audioSourceController))
					{
						selectedControllers.Add(audioSourceController);
					}

					audioSourceController.EnableInfoPanel(true);
				}
			}
		}
	}
}
