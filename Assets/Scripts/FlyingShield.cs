using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingShield : MonoBehaviour
{
	public float angularSpeed = 250f;
	public float radius = 1.25f;
	public Vector3 localAxis = Vector3.up;
	public Vector3 lookDirection = Vector3.forward;
	public Vector3 offset = new Vector3(0, 1f, 0);
	public ParticleSystem sparklesVfx;

	private Transform parent;
	private Vector3 axis;
	private Vector3 initialDirection;
	private float angle = 0f;

	private void Start()
	{
		parent = transform.parent;
		if (parent == null)
		{
			Debug.LogError("FlyingShield: Родительский объект не назначен!");
			enabled = false;
			return;
		}

		if (lookDirection.sqrMagnitude < 1e-6f)
		{
			Debug.LogError("FlyingShield: Направление поворота не может быть нулевым вектором!");
			enabled = false;
			return;
		}
		lookDirection.Normalize();

		sparklesVfx.Stop();

		axis = parent.TransformDirection(localAxis);

		Vector3 perpendicular = Vector3.Cross(axis, Vector3.up);
		if (perpendicular.sqrMagnitude < 1e-6f)
		{
			perpendicular = Vector3.Cross(axis, Vector3.forward);
		}
		perpendicular.Normalize();
		initialDirection = perpendicular;

		transform.position = parent.position + offset + initialDirection * radius;

		UpdateRotation();
	}

	private void Update()
	{
		angle += angularSpeed * Time.deltaTime;
		if (angle >= 360f)
		{
			angle -= 360f;
		}

		Quaternion rotation = Quaternion.AngleAxis(angle, axis);
		Vector3 currentDirection = rotation * initialDirection;
		transform.position = parent.position + offset + currentDirection * radius;

		UpdateRotation();
	}

	private void UpdateRotation()
	{
		Vector3 directionToParent = (parent.position - transform.position).normalized;
		Quaternion baseRotation = Quaternion.LookRotation(directionToParent, axis.normalized);
		Quaternion adjustment = Quaternion.FromToRotation(Vector3.forward, lookDirection);
		transform.rotation = baseRotation * adjustment;
	}

	private void OnTriggerEnter(Collider other)
	{
		var guidedProjectile = other.gameObject.GetComponent<GuidedProjectile>();
		if (guidedProjectile != null)
		{
			Destroy(guidedProjectile.gameObject);
			sparklesVfx.Play();
			return;
		}

		var cannonProjectile = other.gameObject.GetComponent<CannonProjectile>();
		if (cannonProjectile != null)
		{
			Destroy(cannonProjectile.gameObject);
			sparklesVfx.Play();
			return;
		}
	}
}
