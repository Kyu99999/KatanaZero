using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject Prefab;
    public float speed = 7f;
    public bool isReflex = false;

    private void FixedUpdate()
    {
        if (!isReflex)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        else if (isReflex)
        {
            transform.position += -transform.right * speed * Time.deltaTime;
        }
    }

    private void Start()
    {
        Destroy(Prefab, 5f);
    }

    void Update()
    {
        
    }

    public void Fire(Transform transform, Quaternion quaternion)
    {
        Instantiate(Prefab, transform.position, quaternion);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!isReflex)
        {
            if (collision.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().Dead();
                Destroy(gameObject);
            }
        }
        else if (isReflex)
        {
            if (collision.GetComponent<MonsterAI>())
            {
                collision.GetComponent<MonsterAI>().HitAndDead();
                Destroy(gameObject);
            }
        }

       
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
