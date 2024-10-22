using UnityEngine;

public class projectileController : MonoBehaviour
{
    public static projectileController instance { get; private set; } // 单例模式: 其它的脚本可以通过 projectileController.instance 访问这个脚本的实例s

    Rigidbody2D rigidbody2d; // 弹药的刚体
    Animator animator; // 弹药的动画控制器

    void OnCollisionEnter2D(Collision2D other)
    {
        animator.SetTrigger("fireEnd");

        /*cinemachineShake.instance.shakingCamera(5.0f, 0.1f); // 震动相机*/

        if(gameObject.tag == "fireBall")
            cinemachineShake.instance.shakingCamera(3.0f, 0.1f);
        else if(gameObject.tag == "greenFireBall")
            cinemachineShake.instance.shakingCamera(5.0f, 0.1f);

        if (other.gameObject.tag == "monster")
        {
            monsterController monster = other.gameObject.GetComponent<monsterController>();
            if (monster != null)
            {
               if(gameObject.tag == "fireBall")
                    monster.ChangeHealth(-1);
               else if(gameObject.tag == "greenFireBall")
                    monster.ChangeHealth(-2);

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
