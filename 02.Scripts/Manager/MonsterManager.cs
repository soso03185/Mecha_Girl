using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
public class MonsterManager
{
    private List<MonsterScript> monsters = new List<MonsterScript>();


    public void AddMonster(MonsterScript monster)
    {
        monsters.Add(monster);
    }

    public void DeleteMonster(MonsterScript monster)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == monster)
            {
                monsters.RemoveAt(i);
                return;
            }
        }
    }

    public MonsterScript GetNearestMonster(Transform trans)
    {
        if (monsters.Count == 0)
            return default;

        var distances =
            monsters.Select(monster => Vector3.Distance(trans.position, monster.transform.position));

        float minDistance = distances.Min();

        int index = distances.ToList().IndexOf(minDistance);

        return monsters[index];
    }

    public MonsterScript GetFarthestMonster(Transform trans)
    {
        if (monsters.Count == 0)
            return default;

        var distances =
            monsters.Select(monster => Vector3.Distance(trans.position, monster.transform.position));

        float maxDistance = distances.Max();

        int index = distances.ToList().IndexOf(maxDistance);

        return monsters[index];
    }

    public List<MonsterScript> GetMonsterInRange(Vector3 pos, float range)
    {
        if (monsters.Count == 0)
            return default;

        List<MonsterScript> result = new List<MonsterScript>();

        for (int i = 0; i < monsters.Count; i++)
        {
            float distance = Vector3.Distance(pos, monsters[i].transform.position);
            if (distance <= range)
            {
                result.Add(monsters[i]);
            }
        }

        return result;
    }

    public List<MonsterScript> GetMonsterInCircularSector(Transform trans, float angle, float radius)
    {
        if (monsters.Count == 0)
            return default;

        List<MonsterScript> result = new List<MonsterScript>();
        float halfAngleCosine = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
        Vector3 forward = trans.forward;

        foreach (var monster in monsters)
        {
            Vector3 directionToMonster = monster.transform.position - trans.position;
            float distanceToMonster = directionToMonster.magnitude;

            if (distanceToMonster <= radius)
            {
                Vector3 directionNormalized = directionToMonster.normalized;
                float dotProduct = Vector3.Dot(forward, directionNormalized);

                if (dotProduct >= halfAngleCosine)
                {
                    result.Add(monster);
                }
            }
        }

        return result;
    }

    public List<MonsterScript> GetMonsterInQuadrangle(Transform trans, float width, float height)
    {
        if (monsters.Count == 0)
            return default;

        List<MonsterScript> result = monsters.Where(monster => monster.transform.position.x <= trans.position.x + width / 2f &&
        monster.transform.position.x >= trans.position.x - width / 2f && monster.transform.position.z <= trans.position.z + height / 2f
        && monster.transform.position.z >= trans.position.z - height / 2f).ToList();

        return result;
    }

    public List<MonsterScript> GetMonsterInCross(Transform trans, float width, float length)
    {
        if (monsters.Count == 0)
            return default;

        List<MonsterScript> result = monsters.Where(monster => (monster.transform.position.x <= trans.position.x + width / 2f && monster.transform.position.x >= trans.position.x - width / 2f && monster.transform.position.z <= trans.position.z + length / 2f && monster.transform.position.z >= trans.position.z - length / 2f) || (monster.transform.position.x <= trans.position.x + length / 2f && monster.transform.position.x >= trans.position.x - length / 2f && monster.transform.position.z <= trans.position.z + width / 2f && monster.transform.position.z >= trans.position.z - width / 2f)).ToList();

        return result;
    }

    public List<MonsterScript> GetMonsterInPerpendicularDistance(Vector3 a, Vector3 b, float dist)
    {
        if (monsters.Count == 0)
            return default;

        List<MonsterScript> result = new List<MonsterScript>();

        foreach (MonsterScript monster in monsters)
        {
            Vector3 ab = b - a;
            Vector3 ap = monster.transform.position - a;

            float abLengthSquared = Vector3.Dot(ab, ab);
            float dotProduct = Vector3.Dot(ap, ab) / abLengthSquared;

            float t = Mathf.Clamp(dotProduct, 0, 1);

            Vector3 nearestPoint = a + t * ab;

            if (Vector3.Distance(monster.transform.position, nearestPoint) < dist)
            {
                result.Add(monster);
            }
        }

        return result;
    }
    public List<MonsterScript> GetAllMonsters()
    {
        return monsters;
    }
    public int GetMonsterPopulation()
    {
        return monsters.Count;
    }

    public void ResetMonsterDist()
    {
        monsters.Clear();
    }

}
