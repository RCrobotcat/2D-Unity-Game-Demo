using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileController : MonoBehaviour
{
    public static projectileController instance { get; private set; } // ����ģʽ: �����Ľű�����ͨ�� projectileController.instance ��������ű���ʵ��s

    Rigidbody2D rigidbody2d; // ��ҩ�ĸ���
    Animator animator; // ��ҩ�Ķ���������

    void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetTrigger("fireEnd");
        if(other.gameObject.tag == "monster")
        {
            monsterController monster = other.gameObject.GetComponent<monsterController>();
            if (monster != null)
            {
                monster.ChangeHealth(-1);
                Animator animator_monster = other.gameObject.GetComponent<Animator>();
                animator_monster.SetTrigger("isHit");
            }
        }
    }

    void Awake()
    {
        instance = this;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }


    // This function will be called at the end of the fireEnd animation
    void OnFireEndAnimationComplete()
    {
        Destroy(gameObject);
    }
}