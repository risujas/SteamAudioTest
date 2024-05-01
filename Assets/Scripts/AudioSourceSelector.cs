using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSelector : MonoBehaviour
{
	[SerializeField] private LayerMask audioSourceLayerMask;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, audioSourceLayerMask))
			{
				var audioSourceController = hit.collider.gameObject.GetComponentInParent<AudioSourceController>();
				if (audioSourceController)
				{
					audioSourceController.EnableInfoPanel(true);
				}
			}
		}
	}
}
