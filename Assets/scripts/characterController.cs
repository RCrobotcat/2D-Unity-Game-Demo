using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class characterController : MonoBehaviour
{
    float horizontal; float vertical; Vector2 lookDirection = new Vector2(1, 0); [SerializeField] float grabDistance = 0.3f;
    bool isGrabbing = false;

    public Rigidbody2D rigidbody2d; // ��ҵĸ���
    public Rigidbody2D connectBodyCha;
    public Animator animator; // ��ҵĶ���������
    public HingeJoint2D hingeJoint2D;

    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;
    public float timeInvincible = 0.5f; // time of invincible is 0.5 seconds

    Animator playerAnimator;

    public GameObject projectilePrefab; // �����ӵ�Ԥ����

    // Start is called before the first frame update
    void Start()
    {
        connectBodyCha = hingeJoint2D.connectedBody;
        loadingPosition();
        currentHealth = maxHealth;

        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) // ����������ĳ�������ֵ��Ϊ 0 ʱ
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) // ���� E ��ʱ, ץȡ����
        {
            if (!isGrabbing)
            {
                // ���ָ�������ڵ�������ײ��
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabDistance);
                // ����������ײ�������ұ��Ϊ "Grabbable" ������
                foreach (var collider in hitColliders)
                {
                    if (collider.tag == "Grabbable")
                    {
                        Rigidbody2D targetRigidbody = collider.GetComponent<Rigidbody2D>();
                        if (targetRigidbody != null)
                        {
                            hingeJoint2D.connectedBody = targetRigidbody;
                            isGrabbing = true;
                        }
                    }
                }
            }
            else
            {
                hingeJoint2D.connectedBody = connectBodyCha;
                isGrabbing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) // ���� F ��ʱ, ����ɵ�
        {
            Launch();
            /*animator.SetTrigger("Launch");*/
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            savingPosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            savingPosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x += speed * horizontal * Time.deltaTime;
        position.y += speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    void loadingPosition()
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    void savingPosition()
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.Save(); // Ensure data is saved to disk immediately
    }

    public void ResetGame()
    {
        PlayerPrefs.SetFloat("PlayerX", 7.48f);
        PlayerPrefs.SetFloat("PlayerY", -0.21f);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            playerAnimator.SetTrigger("isHit");
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        /*Debug.Log(currentHealth);*/

        if (currentHealth <= 0)
        {
            ResetGame();
        }

        UIHealthBar.instance.setValue(currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        projectileController projectile = projectileObject.GetComponent<projectileController>();
        projectile.Launch(lookDirection, 300);

        // Ignore collision between projectile and character
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D characterCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(projectileCollider, characterCollider);
    }
}