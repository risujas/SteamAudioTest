using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private GameObject targetObject;
	[SerializeField] private float zoomSpeed = 1.0f;
	[SerializeField] private float lerpSpeed = 1.0f;
	[SerializeField] private float minHeight = 1.0f;
	[SerializeField] private float maxHeight = 30.0f;

	private float desiredHeight;
	private Controls controls;

	private void HandleInput()
	{
		var input = controls.Camera.Zoom.ReadValue<float>();
		desiredHeight += -input * Time.deltaTime * zoomSpeed;
		desiredHeight = Mathf.Clamp(desiredHeight, minHeight, maxHeight);
	}

	private void Move()
	{
		Vector3 desiredPosition = new(transform.position.x, desiredHeight, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);

		transform.LookAt(targetObject.transform);
	}

	private void Awake()
	{
		controls = new Controls();
		controls.Camera.Enable();
	}

	private void Start()
	{
		desiredHeight = transform.position.y;
	}

	private void Update()
	{
		HandleInput();
		Move();
	}
}
