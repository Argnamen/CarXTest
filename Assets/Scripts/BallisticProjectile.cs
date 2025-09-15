using System.Threading.Tasks;
using UnityEngine;

public class BallisticProjectile : MonoBehaviour
{
    public float m_speed = 0.2f;
    public int m_damage = 10;

    public Vector3 targetPos;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 controlPoint;
    private float startTime;
    private bool isLaunched = false;
    private float totalDistance;
    private float totalDuration;

    void Start()
    {
        StartLaunch();
    }

    public void StartLaunch()
    {
        startPosition = transform.position;
        endPosition = targetPos;
        startTime = Time.time;
        isLaunched = true;

        // Правильная контрольная точка для вертикальной параболы
        CalculateControlPoint();

        // Рассчитываем общую длину кривой Безье
        totalDistance = CalculateBezierLength(startPosition, controlPoint, endPosition, 20);

        // Рассчитываем время полета на основе скорости
        totalDuration = totalDistance / (m_speed * 400);
    }

    void CalculateControlPoint()
    {
        // Направление от старта к цели в горизонтальной плоскости
        Vector3 horizontalDirection = new Vector3(
            endPosition.x - startPosition.x,
            0,
            endPosition.z - startPosition.z
        ).normalized;

        // Средняя точка между стартом и целью
        Vector3 midPoint = (startPosition + endPosition) * 0.5f;

        // Контрольная точка находится над средней точкой
        controlPoint = midPoint + Vector3.up + horizontalDirection * 0.5f;
    }

    void Update()
    {
        if (!isLaunched) return;

        float elapsedTime = Time.time - startTime;
        float t = Mathf.Clamp01(elapsedTime / totalDuration);


        Vector3 position = CalculateQuadraticBezier(startPosition, controlPoint, endPosition, t);
        transform.position = position;

        // Плавный поворот от начального направления к направлению на цель
        UpdateRotation(t);

    }

    Vector3 CalculateQuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // Формула квадратичной кривой Безье: (1-t)² * P0 + 2 * (1-t) * t * P1 + t² * P2
        float u = 1 - t;
        float uu = u * u;
        float tt = t * t;

        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }

    void UpdateRotation(float t)
    {
        if (t < 0.3f)
        {
            // В начале полета - смотрим прямо по направлению выстрела
            Vector3 horizontalDirection = new Vector3(
                endPosition.x - startPosition.x,
                0,
                endPosition.z - startPosition.z
            ).normalized;
            transform.rotation = Quaternion.LookRotation(horizontalDirection);
        }
        else
        {
            // Плавно поворачиваемся по направлению движения
            Vector3 tangent = CalculateBezierTangent(startPosition, controlPoint, endPosition, t);
            if (tangent.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(tangent.normalized);
            }
        }
    }

    Vector3 CalculateBezierTangent(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // Производная кривой Безье для получения направления движения
        return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
    }

    // Метод для расчета длины кривой Безье (приблизительно)
    float CalculateBezierLength(Vector3 p0, Vector3 p1, Vector3 p2, int segments)
    {
        float length = 0f;
        Vector3 previousPoint = p0;

        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 currentPoint = CalculateQuadraticBezier(p0, p1, p2, t);
            length += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        return length;
    }

    // Метод для получения текущей скорости снаряда (может быть полезен)
    public float GetCurrentSpeed()
    {
        if (!isLaunched) return 0f;

        float elapsedTime = Time.time - startTime;
        float t = Mathf.Clamp01(elapsedTime / totalDuration);

        // Скорость изменения параметра t
        float speedFactor = 1f / totalDuration;

        // Длина касательной (производной) в текущей точке
        Vector3 tangent = CalculateBezierTangent(startPosition, controlPoint, endPosition, t);
        float instantaneousSpeed = tangent.magnitude * speedFactor;

        return instantaneousSpeed;
    }
    void OnTriggerEnter(Collider other)
    {
        var monster = other.gameObject.GetComponent<Monster>();
        if (monster == null)
            return;

        Debug.Log(monster.name);

        monster.m_hp -= m_damage;
        if (monster.m_hp <= 0)
        {
            Destroy(monster.gameObject);
        }
        Destroy(gameObject);
    }
}