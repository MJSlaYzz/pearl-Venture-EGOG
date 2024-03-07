using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateTest : MonoBehaviour
{
    // this code was a test for the rotation system.

    public Transform player;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = player.position - transform.position;
        float angel = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        rb.rotation = angel;
        Debug.Log(angel);
        if(angel > 0f && angel <= 90f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }
        if (angel > 90f && angel <= 180)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }
        if (angel > -180f && angel <= -90f)
        {
            transform.localScale = new Vector3(transform.localScale.x, -1, transform.localScale.z);
        }
        if (angel > -90f && angel <= 0f)
        {
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        }

    }
}
