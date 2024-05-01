using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSelector : MonoBehaviour
{
	[SerializeField] private LayerMask audioSourceLayerMask;

	private List<AudioSourceController> selectedControllers = new List<AudioSourceController>();

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
