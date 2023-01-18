using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class RapidMonster1 : MonsterScript
{
    float chaseInterval = 5f;
    float lookatInterval = 2f;
    float strikeInterval = 2f;

    float sleepInterval = 0.6f;

    float deltaChaseSpeed = 0.2f;

    enum RMState 
    {
        chase,
        lookat,
        strike,
        sleep,
    }
    
    RMState rmState = RMState.chase;


    private float duration;
    Vector2 chaseDirection;
    Vector3 strikePosition;

    private float maxDegreesDelta = 3f;


    bool isStriking = false;

    protected override void KnockBackUpdate()
    {

    }

    protected override void MonsterUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        switch (rmState)
        {
            case RMState.chase:

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);

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
                    rmState = RMState.lookat;
                    duration = 0;
                }

                break;
            case RMState.lookat:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta);
                strikePosition = target.position;
                duration += Time.deltaTime;

                if (duration > lookatInterval)
                {
                    rmState = RMState.strike;
                    duration = 0;
                }

                rb.velocity = Vector2.zero;

                break;
            case RMState.strike:
                if (!isStriking)
                {
                    chaseDirection = (strikePosition - transform.position).normalized;
                    rb.velocity = chaseDirection * chaseSpeed * 10f;
                    isStriking = true;
                }

            
                duration += Time.deltaTime;
                if (duration > strikeInterval)
                {
                    EndStrike();
                }

                break;

            case RMState.sleep:
                duration += Time.deltaTime;
                if (duration > sleepInterval)
                {
                    rmState = RMState.chase;
                }

                break;
            default:
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EndStrike();
        }
    }

    private void EndStrike()
    {
        duration = 0;
        rb.velocity = Vector2.zero;
        isStriking = false;
        rmState = RMState.sleep;

        chaseSpeed += deltaChaseSpeed;
    }
}
