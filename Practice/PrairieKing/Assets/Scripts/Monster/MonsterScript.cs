using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    [SerializeField] protected float chaseSpeed = 1f;
    [SerializeField] protected float chaseTriggerDistance = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] protected int health = 10;

    [SerializeField] private GameObject explosionEffect;


    protected Transform target;
    protected Rigidbody2D rb;
    protected Vector2 moveVelocity;

    protected SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDurationOnHit = 0.2f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private Color flashColor = Color.white;
    protected Color originalColor;
    private float flashDuration;

    private bool isKnockback = false;
    private Vector2 knockbackDirection;
    private float knockbackForce;

    float lastAttackTime = 0f;
    [SerializeField] private float attackInterval = 2f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        InitParam();
    }
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    protected virtual void InitParam()
    {
        int health = Random.Range(8, 12);
        this.health = health;
        PerformHealthChangerEffect();
    }

    public void SetHealth(int health)
    {
        this.health = health;
        PerformHealthChangerEffect();
    }

    public int GetHealth()
    {
        return health;
    }

    protected virtual void PerformHealthChangerEffect()
    {
        if (health >= 10)
        {
            spriteRenderer.color = Color.magenta;
        }
        else if (health > 5)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.yellow;
        }
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        KnockBackUpdate();
        MonsterUpdate();
    }

    protected virtual void KnockBackUpdate()
    {
        if (isKnockback)
        {
            rb.velocity = knockbackDirection.normalized * knockbackForce;
            //rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            knockbackDirection = Vector2.zero;
            knockbackForce = 0;
            isKnockback = false;
            return;
        }
    }

    protected virtual void MonsterUpdate()
    {
        Vector2 chaseDirection = (target.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, target.position) < chaseTriggerDistance)
        {
            moveVelocity = Vector2.SmoothDamp(moveVelocity, chaseDirection * chaseSpeed, ref moveVelocity, 0.05f);
        }
        else
        {
            moveVelocity = Vector2.SmoothDamp(moveVelocity, Vector2.zero, ref moveVelocity, 0.05f);
        }
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    // π•ª˜ÕÊº“
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime > attackInterval)
            {
                PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
                player.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }
    // ‹µΩ…À∫¶

    public void TakeDamage(int damage, Vector2 knockbackDirection, float knockbackForce)
    {

        SetHealth(health - damage);

        if(GameManager.isTEST)
        {
            MonsterSpawner.Instance.AddKilledMonsterCount();
        }

        if (health < 0)
        {
            MonsterSpawner.Instance.AddKilledMonsterCount();
            SoundManager.Instance.PlayMonsterBoomSound();
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Flash());
        SoundManager.Instance.PlayMonsterHurtSound();

        this.knockbackDirection += knockbackDirection;
        this.knockbackForce += knockbackForce;
        isKnockback = true;

        

    }

    IEnumerator Flash()
    {
        flashDuration = flashDurationOnHit;
        while (flashDuration > 0)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
            flashDuration -= flashInterval * 2;
        }
    }
}
