using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RapidMonster : MonsterScript
{
    float chaseInterval = 3f;
    float rotateInterval = 3f;
    float strikeInterval = 1f;

    enum RMState 
    {
        chase,
        rotate,
        strike,
    }
    
    RMState rmState = RMState.chase;


    private float duration;
    Vector2 chaseDirection;
    protected override void MonsterUpdate()
    {

        switch (rmState)
        {
            case RMState.chase:
                transform.rotation = Quaternion.identity;
                chaseDirection = (target.position - transform.position).normalized;
                if (Vector2.Distance(transform.position, target.position) < chaseTriggerDistance)
                {
                    moveVelocity = Vector2.SmoothDamp(moveVelocity, chaseDirection * chaseSpeed, ref moveVelocity, 0.05f);
                }
                else
                {
                    moveVelocity = Vector2.SmoothDamp(moveVelocity, Vector2.zero, ref moveVelocity, 0.05f);
                }
                rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

                duration += Time.deltaTime;

                if (duration > chaseInterval)
                {
                    rmState = RMState.rotate;
                    duration = 0;
                }

                break;
            case RMState.rotate:
                transform.Rotate(0, 0, 90 * Time.deltaTime);
                duration += Time.deltaTime;

                if (duration > rotateInterval)
                {
                    rmState = RMState.strike;
                    duration = 0;
                }

                rb.velocity = Vector2.zero ;

                break;
            case RMState.strike:
                transform.rotation= Quaternion.identity ;
                chaseDirection = (target.position - transform.position).normalized;
                moveVelocity = Vector2.SmoothDamp(moveVelocity, chaseDirection * chaseSpeed * 5f, ref moveVelocity, 0.05f);

                transform.Rotate(0, 0, 90 * Vector2.SqrMagnitude(moveVelocity));

                rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);

                duration += Time.deltaTime;

                if (duration > strikeInterval)
                {
                    rmState = RMState.chase;
                    duration = 0;
                }
                break;
            default:
                break;
        }

    }
}
