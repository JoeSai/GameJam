using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWeaponScript : MonoBehaviour
{
    [SerializeField] private int particleDamage = 1;
    [SerializeField] private float particleKnockbackForce = 1f;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    // Start is called before the first frame update

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            ps.GetCollisionEvents(other, collisionEvents);
            Vector3 hitNormal = collisionEvents[0].normal;

            MonsterScript monster = other.GetComponent<MonsterScript>();
            monster.TakeDamage(particleDamage, -hitNormal, particleKnockbackForce);

            int count = ps.GetParticles(particles);
            for (int i = 0; i < count; i++)
            {
  
                if (particles[i].remainingLifetime > 0f &&  (Vector3.Distance(particles[i].position, other.transform.position) <= 0.5f))
                {
                    particles[i].remainingLifetime = 0f;
                }
            }
            ps.SetParticles(particles, count);
        }
    }

}
