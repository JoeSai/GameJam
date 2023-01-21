using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float disappearDelay = 5f; //道具消失延迟
    public float fadeTime = 1f; //渐隐时间
    private float timer;
    private bool isFading;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = disappearDelay;
        StartCoroutine(FadeIn());


    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f && !isFading)
        {
            StartCoroutine(FadeOut());
        }

        transform.Rotate(0, 0, 720 * Time.deltaTime);
    }


    IEnumerator FadeIn()
    {
        isFading = true;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            //spriteRenderer.color = new Color(1f, 1f, 1f, alpha);

            GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        isFading = false;
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            //spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        isFading = false;
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Destroy(gameObject);
    }
}