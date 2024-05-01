using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxSelector : MonoBehaviour
{
	[SerializeField] private LayerMask selectableLayers;

	private Vector3 initialMousePosition;
	private Vector3 currentMousePosition;

	private List<GameObject> selectedObjects = new List<GameObject>();
	public IReadOnlyList<GameObject> SelectedObjects => selectedObjects.AsReadOnly();

	private bool IsInsideSelectionBox(Vector3 objectScreenPos, Vector3 minScreenPos, Vector3 maxScreenPos)
	{
		return (objectScreenPos.x >= minScreenPos.x && objectScreenPos.x <= maxScreenPos.x) && (objectScreenPos.y >= minScreenPos.y && objectScreenPos.y <= maxScreenPos.y);
	}

	private void FinalizeSelection()
	{
		Vector3 minScreenPos = Vector3.Min(initialMousePosition, currentMousePosition);
		Vector3 maxScreenPos = Vector3.Max(initialMousePosition, currentMousePosition);

		foreach (GameObject obj in FindObjectsByType(typeof(GameObject), FindObjectsSortMode.None))
		{
			Vector3 objectScreenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

			if (IsInsideSelectionBox(objectScreenPos, minScreenPos, maxScreenPos))
			{
				if (!selectedObjects.Contains(obj))
				{
					selectedObjects.Add(obj);
				}
			}
			else
			{
				if (selectedObjects.Contains(obj))
				{
					selectedObjects.Remove(obj);
				}
			}
		}
	}

	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			initialMousePosition = Input.mousePosition;
		}
		else if (Input.GetMouseButton(0))
		{
			currentMousePosition = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			FinalizeSelection();
		}
	}

	private void Update()
	{
		HandleInput();
	}
}
