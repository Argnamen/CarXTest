using UnityEngine;

public class CannonTower : MonoBehaviour
{
    public float m_shootInterval = 0.5f;
    public float m_range = 4f;
    public GameObject m_projectilePrefab;
    public Transform m_shootPoint;
    public Transform m_towerBase; // Основание башни для поворота
    public float m_rotationSpeed = 5f; // Скорость поворота башни

    public float m_basePrediction = 1.7f;
    public float m_minPrediction = 1.0f;
    public float m_maxPrediction = 3.0f;

    private float m_lastShotTime = -0.5f;
    private Monster m_currentTarget;
    private Vector3 m_currentAimPoint;

    void Update()
    {
        if (m_projectilePrefab == null || m_shootPoint == null)
            return;

        // Получаем цель
        Monster target = GetClosestMonster();

        // Если цель изменилась, сбрасываем прицеливание
        if (target != m_currentTarget)
        {
            m_currentTarget = target;
            m_currentAimPoint = target != null ? target.transform.position : Vector3.zero;
        }

        if (m_currentTarget == null) return;

        // Вычисляем новую точку прицеливания с упреждением
        Vector3 newAimPoint = CalculateAimPoint(m_currentTarget);

        // Плавно интерполируем точку прицеливания
        m_currentAimPoint = Vector3.Lerp(m_currentAimPoint, newAimPoint, Time.deltaTime * m_rotationSpeed);

        // Плавно поворачиваем башню
        if (m_towerBase != null)
        {
            Vector3 direction = (m_currentAimPoint - m_towerBase.position).normalized;
            if (direction != Vector3.zero)
            {
                // Сохраняем только горизонтальное вращение
                direction.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                m_towerBase.rotation = Quaternion.Slerp(m_towerBase.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
            }
        }

        // Проверяем, наведена ли башня на цель достаточно точно
        if (IsAimedAtTarget() && m_lastShotTime + m_shootInterval <= Time.time)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // ВАЖНО: Используем правильное направление для снаряда
        // Направление от точки выстрела к цели
        Vector3 shootDirection = (m_currentAimPoint - m_shootPoint.position).normalized;

        // Создаем снаряд с правильным вращением (направление выстрела)
        Instantiate(m_projectilePrefab, m_shootPoint.position, Quaternion.LookRotation(shootDirection));

        m_lastShotTime = Time.time;
    }

    private bool IsAimedAtTarget()
    {
        if (m_towerBase == null) return false;

        // Проверяем направление от точки выстрела к цели
        Vector3 currentDirection = m_towerBase.forward;
        Vector3 targetDirection = (m_currentAimPoint - m_shootPoint.position).normalized;

        // Сохраняем только горизонтальное направление для проверки
        currentDirection.y = 0;
        targetDirection.y = 0;

        // Проверяем угол между текущим и целевым направлением
        float angle = Vector3.Angle(currentDirection, targetDirection);
        return angle < 5f; // Допустимая погрешность в градусах
    }

    private Monster GetClosestMonster()
    {
        Monster closest = null;
        float minDistance = float.MaxValue;

        foreach (var monster in FindObjectsOfType<Monster>())
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance <= m_range && distance < minDistance)
            {
                minDistance = distance;
                closest = monster;
            }
        }
        return closest;
    }

    private Vector3 CalculateAimPoint(Monster monster)
    {
        // Направление движения монстра
        Vector3 monsterDirection = (monster.m_moveTarget.transform.position - monster.transform.position).normalized;

        // Расстояние от точки выстрела до монстра
        float distance = Vector3.Distance(m_shootPoint.position, monster.transform.position);

        // Время полета снаряда (расстояние / скорость)
        float flightTime = distance / 0.2f; // m_speed = 0.2f

        float predictionMultiplier = CalculatePredictionBasedOnRotationSpeed();
        float monsterMoveDistance = monster.m_speed * flightTime * predictionMultiplier;

        return monster.transform.position + monsterDirection * monsterMoveDistance + Vector3.up;
    }

    private float CalculatePredictionBasedOnRotationSpeed()
    {
        // Обратная зависимость: чем выше скорость поворота, тем меньше упреждение
        // Формула: base / (1 + rotationSpeed * factor)
        float rotationFactor = 0.1f; // Коэффициент влияния скорости поворота
        float prediction = m_basePrediction / (1f + m_rotationSpeed * rotationFactor);

        return Mathf.Clamp(prediction, m_minPrediction, m_maxPrediction);
    }
}