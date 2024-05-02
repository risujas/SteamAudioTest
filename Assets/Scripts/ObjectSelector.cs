using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
	[SerializeField] private LayerMask selectableLayers;
	[SerializeField] private RectTransform panel;
	[SerializeField] private float minTimeForBoxSelection = 0.1f;
	[SerializeField] private float minMouseDistanceForBoxSelection = 1.0f;

	private Vector3 initialMousePosition;
	private Vector3 currentMousePosition;

	private float startTime;
	private bool selecting;

	public List<GameObject> SelectedObjects
	{
		get; private set;
	}

	private bool UsingBoxSelection
	{
		get
		{
			float elapsedTime = Time.time - startTime;
			float mouseDistance = Vector3.Distance(initialMousePosition, currentMousePosition);
			return elapsedTime >= minTimeForBoxSelection && mouseDistance >= minMouseDistanceForBoxSelection;
		}
	}

	private bool IsInsideSelectionBox(Vector3 objectScreenPos, Vector3 minScreenPos, Vector3 maxScreenPos)
	{
		return (objectScreenPos.x >= minScreenPos.x && objectScreenPos.x <= maxScreenPos.x) && (objectScreenPos.y >= minScreenPos.y && objectScreenPos.y <= maxScreenPos.y);
	}

	private void SelectWithBox()
	{
		Vector3 minScreenPos = Vector3.Min(initialMousePosition, currentMousePosition);
		Vector3 maxScreenPos = Vector3.Max(initialMousePosition, currentMousePosition);

		foreach (GameObject obj in FindObjectsByType(typeof(GameObject), FindObjectsSortMode.None))
		{
			if ((selectableLayers & (1 << obj.layer)) == 0)
			{
				continue;
			}

			Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

			if (IsInsideSelectionBox(objectScreenPos, minScreenPos, maxScreenPos))
			{
				if (!SelectedObjects.Contains(obj))
				{
					SelectedObjects.Add(obj);
				}
			}
			else
			{
				if (SelectedObjects.Contains(obj))
				{
					SelectedObjects.Remove(obj);
				}
			}
		}

		panel.gameObject.SetActive(false);
	}

	private void SelectWithRaycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, selectableLayers))
		{
			SelectedObjects.Add(hit.collider.gameObject);
		}
	}

	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0) && !selecting)
		{
			initialMousePosition = Input.mousePosition;
			startTime = Time.time;
			selecting = true;
		}

		if (selecting)
		{
			if (Input.GetMouseButtonUp(0))
			{
				if (UsingBoxSelection)
				{
					SelectWithBox();
				}
				else
				{
					SelectWithRaycast();
				}

				selecting = false;
			}

			else if (Input.GetMouseButton(0))
			{
				currentMousePosition = Input.mousePosition;

				if (UsingBoxSelection)
				{
					UpdateBoxVisuals();
				}
			}
		}
	}

	private void UpdateBoxVisuals()
	{
		panel.gameObject.SetActive(selecting);

		float width = currentMousePosition.x - initialMousePosition.x;
		float height = currentMousePosition.y - initialMousePosition.y;

		// :thinking:
	}

	private void Update()
	{
		SelectedObjects = new List<GameObject>();

		HandleInput();
	}
}
