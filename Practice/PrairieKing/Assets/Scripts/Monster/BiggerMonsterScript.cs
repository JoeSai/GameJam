using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BiggerMonsterScript : MonsterScript
{

    protected override void InitParam()
    {
        int health = Random.Range(12, 16);
        this.health = health;
    }
    protected override void PerformHealthChangerEffect()
    {
        if (health < 5)
        {
            transform.localScale = Vector3.one;
            spriteRenderer.color = Color.black;
            originalColor = spriteRenderer.color;
        }
 
    }



}
