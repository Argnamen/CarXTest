using System.Collections;
using System.Threading;
using UnityEngine;

public class CannonTower : MonoBehaviour
{
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public GameObject m_projectilePrefab;
    public Transform m_shootPoint;

    private Monster m_target;
    private Vector3 A_start;
    private Vector3 V_a;

    private Vector3 B_start;
    private float V_b_magnitude = 0.2f;

    private float m_lastShotTime = -0.5f;
    private Vector3 m_previousTargetPosition;
    private float m_previousTime;

    [Header("Результаты")]
    public bool hasSolution;
    public Vector3 interceptPoint;
    public float tau;
    public Vector3 firingDirection;
    public float T_launch;

    private void Start()
    {
        B_start = m_shootPoint.position;
        m_previousTime = Time.time;
    }

    void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        // Ищем ближайшую цель
        FindClosestTarget();

        if (m_target != null)
        {
            // Обновляем параметры цели
            A_start = m_target.transform.position;

            // Вычисляем скорость цели на основе перемещения
            float currentTime = Time.time;
            float deltaTime = currentTime - m_previousTime;

            if (deltaTime > 0.01f) // Чтобы избежать деления на ноль
            {
                V_a = (A_start - m_previousTargetPosition) / deltaTime;
                m_previousTargetPosition = A_start;
                m_previousTime = currentTime;
            }

            B_start = m_shootPoint.position;

            // Вычисляем решение перехвата
            hasSolution = CalculateIntercept(A_start, V_a, B_start, V_b_magnitude, m_range,
                                           out interceptPoint, out tau, out firingDirection, out T_launch);

            if (hasSolution)
            {
                // Поворачиваем пушку в направлении выстрела
                if (firingDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(firingDirection);
                }

                // Стреляем с интервалом
                if (Time.time - m_lastShotTime >= m_shootInterval)
                {
                    Shoot();
                    m_lastShotTime = Time.time;
                }
            }
        }
    }

    void FindClosestTarget()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        float closestDistance = Mathf.Infinity;
        Monster closestMonster = null;

        foreach (var monster in monsters)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance <= m_range && distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monster;
            }
        }

        m_target = closestMonster;
        if (m_target != null)
        {
            m_previousTargetPosition = m_target.transform.position;
        }
    }

    void Shoot()
    {
        Instantiate(m_projectilePrefab, m_shootPoint.position, m_shootPoint.rotation);
        Debug.Log("Выстрел! Время полета: " + tau);
    }

    /// <summary>
    /// Статический метод для вычисления решения перехвата
    /// </summary>
    public static bool CalculateIntercept(Vector3 A_start, Vector3 V_a, Vector3 B_start,
                                        float V_b_magnitude, float R,
                                        out Vector3 interceptPoint, out float tau,
                                        out Vector3 firingDirection, out float T_launch)
    {
        interceptPoint = Vector3.zero;
        tau = 0f;
        firingDirection = Vector3.zero;
        T_launch = 0f;

        // Вектор от пушки к цели
        Vector3 D_start = A_start - B_start;
        float currentDistance = D_start.magnitude;

        // Если цель уже внутри радиуса R, стреляем сразу
        T_launch = 0f;

        // Позиция цели в момент выстрела
        Vector3 A_current = A_start + V_a * T_launch;

        // Расчет времени полета снаряда
        Vector3 D = A_current - B_start;

        float a = V_a.sqrMagnitude - V_b_magnitude * V_b_magnitude;
        float b = 2f * Vector3.Dot(D, V_a);
        float c = D.sqrMagnitude;

        float discriminant = b * b - 4f * a * c;
        if (discriminant < 0f) return false;

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float tau1 = (-b + sqrtDiscriminant) / (2f * a);
        float tau2 = (-b - sqrtDiscriminant) / (2f * a);

        // Выбираем минимальное положительное время полета
        if (tau1 >= 0 && tau2 >= 0)
            tau = Mathf.Min(tau1, tau2);
        else if (tau1 >= 0)
            tau = tau1;
        else if (tau2 >= 0)
            tau = tau2;
        else
            return false;

        // Точка перехвата
        interceptPoint = A_current + V_a * tau;

        // Направление выстрела (от пушки к точке перехвата)
        firingDirection = (interceptPoint - B_start).normalized;

        return true;
    }

    void OnDrawGizmos()
    {
        if (!hasSolution || m_target == null) return;

        // Позиция цели в момент выстрела
        Vector3 A_current = A_start + V_a * T_launch;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(A_current, 0.1f);
        Gizmos.DrawLine(A_start, A_current);

        // Точка перехвата
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(interceptPoint, 0.2f);
        Gizmos.DrawLine(B_start, interceptPoint);
        Gizmos.DrawLine(A_current, interceptPoint);

        // Направление выстрела
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(B_start, firingDirection * 3f);
    }
}