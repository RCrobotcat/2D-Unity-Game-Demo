using UnityEngine;

public class monsterController : MonoBehaviour
{
    public Transform player;
    public float speed = 1.0f;

    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    HealthBar healthBar; // ��Ӷ� HealthBar ������
    HealthSystemForDummies healthSystem; // ��Ӷ� HealthSystemForDummies ������


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>(); // ��ʼ�� HealthBar
        healthSystem = GetComponent<HealthSystemForDummies>(); // ��ʼ�� HealthSystemForDummies
        /*if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth); // ������󽡿�ֵ
            healthBar.SetHealth(currentHealth); // ���õ�ǰ����ֵ
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // �ù��ﲻ��������ƶ�
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
        /*Debug.Log("Monster Health: " + currentHealth + "/" + maxHealth);*/
        /*if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth); // ���½�����
        }*/
        if (currentHealth <= 0)
        {
            healthSystem.Kill();
            Destroy(gameObject);
        }
    }
}
