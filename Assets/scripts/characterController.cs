using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class characterController : MonoBehaviour
{
    float horizontal;
    float vertical;
    Vector2 lookDirection = new Vector2(1, 0);
    [SerializeField] float grabDistance = 0.3f;
    bool isGrabbing = false;

    public Rigidbody2D rigidbody2d; // 玩家的刚体
    public Rigidbody2D connectBodyCha;
    public Animator animator; // 玩家的动画控制器
    public HingeJoint2D hingeJoint2D;

    public float speed = 3.0f;

    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;
    public float timeInvincible = 1f; // time of invincible is 0.5 seconds

    Animator playerAnimator;

    // 引用子弹预制体
    public GameObject projectilePrefab_fireBall;
    public GameObject projectilePrefab_greenFireBall;

    GameObject projectilePrefab_currentUse; // 当前使用的子弹的预制体

    float nextFireTime = 0f; // 下次发射时间
    public float fireRate = 0.5f; // 发射间隔

    public GameObject text_canvas; // 显示当前使用的子弹的画布
    public TextMeshProUGUI projectile_text; // 显示当前使用的子弹
    float displayTime = 1f;
    float timerDisplay;

    HealthBar healthBar; // 添加对 HealthBar 的引用
    HealthSystemForDummies healthSystem; // 添加对 HealthSystemForDummies 的引用

    // awake 方法在 Start 方法之前调用
    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>(); // 初始化 HealthBar
        healthSystem = GetComponent<HealthSystemForDummies>(); // 初始化 HealthSystemForDummies
    }

    // Start is called before the first frame update
    void Start()
    {
        connectBodyCha = hingeJoint2D.connectedBody;

        loadingPosition();

        if (PlayerPrefs.HasKey("health"))
        {
            currentHealth = (int) PlayerPrefs.GetFloat("health");
            /*UIHealthBar.instance.setValue(currentHealth / (float)maxHealth);*/
            healthSystem.ReviveWithCustomHealth(currentHealth);
        }
        else
        {
            currentHealth = maxHealth;
        }

        // 判断当前使用的子弹
        if (PlayerPrefs.HasKey("projectile"))
        {
            projectilePrefab_currentUse = PlayerPrefs.GetString("projectile") == "fireBall" ? projectilePrefab_fireBall : projectilePrefab_greenFireBall;
        }
        else
        {
            projectilePrefab_currentUse = projectilePrefab_fireBall;
        }

        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) // 当玩家输入的某个轴向的值不为 0 时
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

        if (Input.GetKeyDown(KeyCode.E)) // 按下 E 键时, 抓取物体
        {
            if (!isGrabbing)
            {
                // 检测指定区域内的所有碰撞器
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grabDistance);
                // 遍历所有碰撞器，查找标记为 "Grabbable" 的物体
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

        // 切换子弹
        if (Input.GetKey(KeyCode.Alpha1) && projectilePrefab_currentUse != projectilePrefab_fireBall)
        {
            projectilePrefab_currentUse = projectilePrefab_fireBall;
            Debug.Log("fireBall");
            timerDisplay = displayTime;
            projectile_text.SetText("using Fire Ball");
            projectile_text.color = new Color(255, 72, 0, 255); // 文本颜色为红色
            text_canvas.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.Alpha2) && projectilePrefab_currentUse != projectilePrefab_greenFireBall)
        {
            projectilePrefab_currentUse = projectilePrefab_greenFireBall;
            Debug.Log("greenFireBall");
            timerDisplay = displayTime;
            projectile_text.SetText("using Green Fire Ball");
            projectile_text.color = new Color32(14, 255, 13, 255); // 文本颜色为绿色
            text_canvas.SetActive(true);
        }

        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                text_canvas.SetActive(false);
            }
        }

        // 切换场景(时空转换设定)
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            savingPosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            savingPosition();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        // 按下鼠标左键时, 发射飞弹(飞弹跟随鼠标)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            LaunchByMouse();
            nextFireTime = Time.time + fireRate; // 更新下次发射时间
        }

        // 按下 F 键时, 发射飞弹
        if (Input.GetKey(KeyCode.F) && Time.time >= nextFireTime)
        {
            Launch();
            nextFireTime = Time.time + fireRate; // 更新下次发射时间
            /*animator.SetTrigger("Launch");*/
        }

        if (Input.GetKeyDown(KeyCode.Escape))
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
        // Save position to disk
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("health", health); // Save health to disk
        PlayerPrefs.SetString("projectile", projectilePrefab_currentUse.tag); // Save current projectile to disk
        PlayerPrefs.Save(); // Ensure data is saved to disk immediately
    }

    public void ResetGame()
    {
        PlayerPrefs.SetFloat("PlayerX", 7.48f);
        PlayerPrefs.SetFloat("PlayerY", -0.21f);
        PlayerPrefs.SetFloat("health", maxHealth);
        PlayerPrefs.SetString("projectile", "fireBall");
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

        healthSystem.AddToCurrentHealth(amount);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        /*Debug.Log(currentHealth);*/

        if (currentHealth <= 0)
        {
            healthSystem.Kill();
            ResetGame();
        }

        /*UIHealthBar.instance.setValue(currentHealth / (float)maxHealth);*/
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab_currentUse, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        projectileController projectile = projectileObject.GetComponent<projectileController>();
        projectile.Launch(lookDirection, 300);

        // Ignore collision between projectile and character
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D characterCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(projectileCollider, characterCollider);
    }

    void LaunchByMouse()
    {
        // 记录鼠标位置
        Vector3 direction = Input.mousePosition;

        // 生成子弹
        GameObject projectileObject = Instantiate(projectilePrefab_currentUse, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        projectileController projectile = projectileObject.GetComponent<projectileController>();

        // 子弹速度由鼠标点击的位置减去屏幕的宽高的1/2得到
        // 主要就是坐标的转换
        projectile.Launch(new Vector2(direction.x - Camera.main.pixelWidth / 2, direction.y - Camera.main.pixelHeight / 2).normalized, 300);

        // Ignore collision between projectile and character
        Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
        Collider2D characterCollider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(projectileCollider, characterCollider);
    }
}
