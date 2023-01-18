using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class BulletWeaponScript : MonoBehaviour
{
    [SerializeField] private int particleDamage = 1;
    [SerializeField] private float particleKnockbackForce = 1f;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    [SerializeField] int currentBullets = 1;
    [SerializeField] int maxBullets = 3;
    private float upgradeRadiusValue = 0.2f;


    // Start is called before the first frame update

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    public void UpdateWeapon()
    {
        if(currentBullets >= maxBullets)
        {
            return;
        }

        currentBullets++;


        ParticleSystem weaponParticleSystem = GetComponent<ParticleSystem>();
        var emission = weaponParticleSystem.emission;
        var bursts = new ParticleSystem.Burst[1];
        bursts[0] = new ParticleSystem.Burst(0f, currentBullets);
        emission.SetBursts(bursts);

        var shapeModule = weaponParticleSystem.shape;
        shapeModule.radius += upgradeRadiusValue;
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            ps.GetCollisionEvents(other, collisionEvents);
            Vector3 hitNormal = collisionEvents[0].normal;
            MonsterScript monster = other.GetComponent<MonsterScript>();


            int count = ps.GetParticles(particles);
            for (int i = 0; i < count; i++)
            {
  
                if (particles[i].remainingLifetime > 0f &&  (Vector3.Distance(particles[i].position, other.transform.position) <= 1f))
                {
                    monster.TakeDamage(particleDamage, -hitNormal, particleKnockbackForce);
                    particles[i].remainingLifetime = 0f;
                }
            }
            ps.SetParticles(particles, count);
        }
    }

    private void OnParticleTrigger()
    {
        
    }

}
