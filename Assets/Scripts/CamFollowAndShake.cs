using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowAndShake : MonoBehaviour
{
	[SerializeField] private Transform camPlace;
	[SerializeField] private float shakeDistance, smooth;

	private float decreaseFactor = 1.0f;

	public float shakeDuration = 0f;

	void Update()
    {
		if (shakeDuration > 0)
		{
			Vector3 targetRotation = camPlace.position + Random.insideUnitSphere * shakeDistance;

            transform.position = Vector3.Lerp(transform.position, targetRotation, smooth);

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;

			transform.position = camPlace.position;
		}
	}
}