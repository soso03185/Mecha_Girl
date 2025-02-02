using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill
{
    public Projectile m_projectilePrefab;
    protected List<Projectile> m_projectileInstances = new();

    public void InstantiateProjectile(int projectileNumber = 1)
    {
        List<Projectile> newProjectiles = new List<Projectile>();
        for(int i = 0; i < projectileNumber; i++)
        {
            Projectile newProjectile = Instantiate(m_projectilePrefab, transform.position, transform.rotation, this.transform);
            newProjectile.m_skill = this;
            m_projectileInstances.Add(newProjectile);
        }
    }

    public void InstantiateProjectile(Vector3 pos, int projectileNumber = 1)
    {
        List<Projectile> newProjectiles = new List<Projectile>();
        for (int i = 0; i < projectileNumber; i++)
        {
            Projectile newProjectile = Instantiate(m_projectilePrefab, pos, Quaternion.identity);
            newProjectile.m_skill = this;
            m_projectileInstances.Add(newProjectile);
        }
    }
}
