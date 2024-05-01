using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxSelector : MonoBehaviour
{
	[SerializeField] private LayerMask selectableLayers;
	[SerializeField] private RectTransform panel;

	private Vector3 initialMousePosition;
	private Vector3 currentMousePosition;

	public List<GameObject> SelectedObjects
	{
		get; private set;
	}

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

	private void HandleInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			initialMousePosition = Input.mousePosition;
		}
		else if (Input.GetMouseButton(0))
		{
			currentMousePosition = Input.mousePosition;
			UpdateTransform();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			FinalizeSelection();
		}
	}

	private void UpdateTransform()
	{
		if (!panel.gameObject.activeSelf)
		{
			panel.gameObject.SetActive(true);
		}

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
