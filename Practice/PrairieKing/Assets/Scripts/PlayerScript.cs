using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float attackRate = 0.1f;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private float teleportDistance = 1.5f;


    [SerializeField] private GameObject teleportClone;
    public float teleportFadeTime = 0.2f;

    [SerializeField] private LayerMask teleportCheckLayers;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private float nextAttack;
    private Quaternion targetRotation;
    private bool isAttack;

    private Vector2 moveVelocity;

    private float flashDurationOnHit = 0.2f;
    private float flashInterval = 0.1f;
    [SerializeField] private Color flashColor = Color.white;
    private Color originalColor;
    private float flashDuration;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

    }

    void Update()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
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

        Vector3 direction = new Vector3(moveX, moveY, 0);

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if (Vector3.Magnitude(direction) > 0.1f)
            {
                GameObject clone = Instantiate(teleportClone, transform.position, transform.rotation);
                // 设置克隆体材质的透明度
                clone.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.5f);
                Destroy(clone, teleportFadeTime);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, teleportDistance, teleportCheckLayers); //在瞬移距离内检测碰撞
                if (hit.collider != null && !hit.collider.gameObject.CompareTag("Enemy"))
                {
                    transform.position = hit.point - (Vector2)direction * 0.01f; //瞬移到墙壁边缘
                }
                else //如果没有碰撞
                {
                    transform.position += direction * teleportDistance; //瞬移到预期位置
                }
                SoundManager.Instance.PlayTeleportSound();
            }

        }

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

        SoundManager.Instance.PlayAttackSound();
    }

    public void TakeDamage(int damage)
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }
        SetHealth(health - damage);
        StartCoroutine(Flash());
    }

    public void SetHealth(int health)
    {
        this.health = health;
        Camera.main.GetComponent<CameraShake>().ScreenShake();
        SoundManager.Instance.PlayPlayerHurtSound();
        GUIManager.Instance.UpdateHealthUI(this.health);
        if (this.health <= 0)
        {
            GameManager.Instance.GameOver();
        }
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