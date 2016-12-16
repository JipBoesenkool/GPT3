using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoidFlocking : MonoBehaviour
{
	internal BoidController controller;
	private Rigidbody rb;

	IEnumerator Start()
	{
		controller = GameObject.Find ("Boid Controller").GetComponent<BoidController>();
		rb = GetComponent<Rigidbody> ();

		while (true)
		{
			if (controller)
			{
				Vector3 vel = steer() * Time.deltaTime;

				if (!float.IsNaN(vel.x) && !float.IsNaN(vel.y) && !float.IsNaN(vel.z))
				{
					rb.velocity += vel;

					// enforce minimum and maximum speeds for the boids
					float speed = rb.velocity.magnitude;
					if (speed > controller.maxVelocity)
					{
						rb.velocity = rb.velocity.normalized * controller.maxVelocity;
					}
					else if (speed < controller.minVelocity)
					{
						rb.velocity = rb.velocity.normalized * controller.minVelocity;
					}

					transform.rotation = Quaternion.LookRotation(rb.velocity);
				}
					
			}
			float waitTime = Random.Range(0.3f, 0.5f);
			yield return new WaitForSeconds(waitTime);
		}
	}

	Vector3 steer()
	{
		Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
		randomize.Normalize();
		randomize *= controller.randomness;

		Vector3 center = controller.flockCenter - transform.localPosition;
		Vector3 velocity = controller.flockVelocity - rb.velocity;
		Vector3 follow = controller.target.localPosition - transform.localPosition;

		return (center + velocity + follow * 2 + randomize);
	}
}