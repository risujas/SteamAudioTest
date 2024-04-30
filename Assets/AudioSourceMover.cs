using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceMover : MonoBehaviour
{
	[SerializeField] private LayerMask groundLayerMask;
	[SerializeField] private LayerMask audioSourceLayerMask;
	[SerializeField] private float minSourceHeight = 0.35f;
	[SerializeField] private float maxSourceHeight = 5.0f;

	private GameObject selectedGameObject;

	public bool HasObjectSelected
	{
		get { return selectedGameObject != null; }
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown(0))
		{
			if (!selectedGameObject)
			{
				if (Physics.Raycast(ray, out hit, 100.0f, audioSourceLayerMask))
				{
					AudioSource audioSource = hit.collider.gameObject.GetComponentInParent<AudioSource>();
					if (audioSource)
					{
						selectedGameObject = audioSource.gameObject;
					}
				}
			}
			else
			{
				selectedGameObject = null;
			}
		}

		if (selectedGameObject)
		{
			if (Physics.Raycast(ray, out hit, 100.0f, groundLayerMask))
			{
				selectedGameObject.transform.position = new(hit.point.x, selectedGameObject.transform.position.y, hit.point.z);
			}

			var input = Input.GetAxis("Mouse ScrollWheel");
			selectedGameObject.transform.position += Vector3.up * input;
			float y = Mathf.Clamp(selectedGameObject.transform.position.y, minSourceHeight, maxSourceHeight);
			selectedGameObject.transform.position = new(selectedGameObject.transform.position.x, y, selectedGameObject.transform.position.z);
		}
	}
}
