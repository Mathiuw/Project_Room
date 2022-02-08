using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowAndShake : MonoBehaviour
{
	[SerializeField] private Transform camPlace;

    public float shakeDuration = 0f;

	[SerializeField]private float shakeAmount;

	private float decreaseFactor = 1.0f;

	void Update()
    {
		if (shakeDuration > 0)
		{
			transform.position = Vector3.Lerp(transform.position, camPlace.position + Random.insideUnitSphere * shakeAmount, 1f);

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			transform.position = camPlace.position;
		}
	}
}