using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public float m_interval = 3;
	public GameObject m_moveTarget;
	public FlyingShield m_flyingShieldPrefab;

	private float m_lastSpawn = -1;

	void Update () {
		if (Time.time > m_lastSpawn + m_interval) {
			var newMonster = GameObject.CreatePrimitive (PrimitiveType.Capsule);
			var r = newMonster.AddComponent<Rigidbody> ();
			r.useGravity = false;
			newMonster.transform.position = transform.position;
			var monsterBeh = newMonster.AddComponent<Monster> ();
			monsterBeh.m_moveTarget = m_moveTarget;

			if (m_flyingShieldPrefab != null)
				Instantiate(m_flyingShieldPrefab, newMonster.transform);

			m_lastSpawn = Time.time;
		}
	}
}
