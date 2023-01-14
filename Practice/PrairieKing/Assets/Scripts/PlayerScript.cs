using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float attackRate = 0.1f;
    [SerializeField] private ParticleSystem attackParticles;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float nextAttack;
    private Quaternion targetRotation;
    private bool isAttack;

    private Vector2 moveVelocity;

    private float flashDurationOnHit = 0.2f;
    private float flashInterval = 0.1f;
    [SerializeField] private Color flashColor = Color.white;
    private Color originalColor = Color.blue;
    private float flashDuration;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //接收玩家攻击输入
        if (Input.GetKeyDown(KeyCode.UpArrow) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            PreAttack(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            PreAttack(Vector2.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            PreAttack(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && Time.time > nextAttack)
        {
            nextAttack = Time.time + attackRate;
            PreAttack(Vector2.left);
        }
        //接收玩家移动输入
        float moveX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float moveY = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        moveVelocity = new Vector2(moveX, moveY) * moveSpeed;

        
    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

        if (isAttack)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                Attack();
                isAttack = false;
            }
        }
    }

    void PreAttack(Vector2 attackDirection)
    {
        isAttack = true;
        targetRotation = Quaternion.LookRotation(Vector3.forward, attackDirection);
    }

    private void Attack()
    {
        attackParticles.Play();
    }

    public void TakeDamage(int damage)
    {
        Debug.LogError("damage");

        StartCoroutine(Flash());

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