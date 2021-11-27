using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballControler : MonoBehaviour
{
    public float MoveSpeed = 10;
    public float T = 0;

    public Transform Ball;
    public Transform Arms;
    public Transform PosOverHead;
    public Transform PosDribble;
    public Transform Target;

    public bool BallInHands = true;
    public bool IsBallFlying = false;

    

    // Update is called once per frame
    void Update()
    {
        // Walking
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        // Ball in Hands
        if (BallInHands)
        {
            // Hold over head
            if (Input.GetKey(KeyCode.Space))
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * -170;

                // Look at Hoop
                transform.LookAt(Target.parent.position);
            }

            // Dribbling
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            // Throw ball
            if (Input.GetKeyUp(KeyCode.Space))
            {
                BallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // Ball in the air
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.8f;
            float t01 = T / duration;

            // Move to target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // Throw arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // Ball arrives at target
            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!BallInHands && !IsBallFlying)
        {
            BallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
