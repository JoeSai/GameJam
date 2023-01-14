using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float chaseTriggerDistance = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int health = 10;

    private Transform target;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDurationOnHit = 0.2f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private Color flashColor = Color.white;
    private Color originalColor;
    private float flashDuration;

    private bool isKnockback = false;
    private Vector3 knockbackDirection;
    private float knockbackForce;

    float lastAttackTime = 0f;
    [SerializeField] private float attackInterval = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;

    }

    public void SetHealth(int health)
    {
        this.health = health;

        if(health > 10)
        {
            spriteRenderer.color = Color.magenta;
        }else if (health > 5){
            spriteRenderer.color = Color.red;
        } else
        {
            spriteRenderer.color = Color.cyan;
        }
        originalColor = spriteRenderer.color;
    }

    void FixedUpdate()
    {
        if (isKnockback)
        {
            rb.velocity = knockbackDirection * knockbackForce;
            //rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            knockbackDirection = Vector3.zero;
            knockbackForce = 0;
            isKnockback = false;
            return;
        }
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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            player.TakeDamage(damage);
        }
    }
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
        if (health < 0)
        {
            MonsterSpawner.Instance.AddKilledMonsterCount();
            Destroy(gameObject);
            return;
        }
        StartCoroutine(Flash());

        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
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
