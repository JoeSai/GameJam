
using UnityEngine;


public class test : MonoBehaviour
{
    Transform target;
    private float timeCount = 0.0f;

    Quaternion fromRotation;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        fromRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(target.position);
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Slerp(fromRotation, targetRotation, 0.1f);
        timeCount = timeCount + Time.deltaTime;
    }


}



