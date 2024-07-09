using UnityEngine;

public class monsterController : MonoBehaviour
{
    public Transform player;
    public float speed = 1.0f;

    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    HealthBar healthBar; // 添加对 HealthBar 的引用
    HealthSystemForDummies healthSystem; // 添加对 HealthSystemForDummies 的引用


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>(); // 初始化 HealthBar
        healthSystem = GetComponent<HealthSystemForDummies>(); // 初始化 HealthSystemForDummies
        /*if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // 设置最大健康值
            healthBar.SetHealth(currentHealth); // 设置当前健康值
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // 让怪物不断向玩家移动
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        characterController player = collision.gameObject.GetComponent<characterController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        characterController player = collision.gameObject.GetComponent<characterController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (amount < 0)
        {
            healthSystem.AddToCurrentHealth(amount);
        }
        Debug.Log("Monster Health: " + currentHealth + "/" + maxHealth);
        /*if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // 更新健康条
        }*/
        if (currentHealth <= 0)
        {
            healthSystem.Kill();
            Destroy(gameObject);
        }
    }
}
