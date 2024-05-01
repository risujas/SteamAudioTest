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

	private void Start()
	{
		desiredHeight = transform.position.y;
	}

	private void Update()
	{
		var input = Input.GetAxis("Mouse ScrollWheel");
		desiredHeight += -input * Time.deltaTime * zoomSpeed;
		desiredHeight = Mathf.Clamp(desiredHeight, minHeight, maxHeight);

		Vector3 desiredPosition = new(transform.position.x, desiredHeight, transform.position.z);
		transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);

		transform.LookAt(targetObject.transform);
	}
}
