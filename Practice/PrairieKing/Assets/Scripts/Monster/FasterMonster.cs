using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FasterMonster : MonsterScript
{

    protected override void InitParam()
    {
        int health = Random.Range(6, 10);
        this.health = health;

        float speed = Random.Range(1.1f, 1.6f);
        chaseSpeed = speed;

        
    }
    protected override void PerformHealthChangerEffect()
    {
        if(chaseSpeed > 0.5f)
        {
            chaseSpeed = chaseSpeed - 0.1f;
        }
    }

}
