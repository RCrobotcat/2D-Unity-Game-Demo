using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowCameraRotation))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true; // 是否始终面向摄像机
    [SerializeField] bool shouldShowHealthNumbers = true; // 是否显示健康值

    float finalValue;
    float animationSpeed = 0.1f;
    float leftoverAmount = 0f;

    // Caches
    HealthSystemForDummies healthSystem;
    Image image;
    Text text;
    FollowCameraRotation followCameraRotation;

    private void Start()
    {
        healthSystem = GetComponentInParent<HealthSystemForDummies>();
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
        followCameraRotation = GetComponent<FollowCameraRotation>();
        healthSystem.OnCurrentHealthChanged.AddListener(ChangeHealthFill); // 添加监听器, 当健康值改变时调用 ChangeHealthFill 方法
    }

    void Update()
    {
        animationSpeed = healthSystem.AnimationDuration;

        if (!healthSystem.HasAnimationWhenHealthChanges)
        {
            image.fillAmount = healthSystem.CurrentHealthPercentage / 100; // 更新健康值
        }

        text.text = $"{healthSystem.CurrentHealth}/{healthSystem.MaximumHealth}";

        text.enabled = shouldShowHealthNumbers;

        followCameraRotation.enabled = isBillboarded;
    }

    private void ChangeHealthFill(CurrentHealth currentHealth)
    {
        if (!healthSystem.HasAnimationWhenHealthChanges) return; // 如果没有动画，直接返回

        StopAllCoroutines(); // 停止所有协程
        StartCoroutine(ChangeFillAmount(currentHealth)); // 启动协程
    }

    private IEnumerator ChangeFillAmount(CurrentHealth currentHealth) // 健康填充动画协程
    {
        finalValue = currentHealth.percentage / 100; // 表示健康条的目标填充量(最终值)

        float cacheLeftoverAmount = this.leftoverAmount; // 缓存剩余量

        float timeElapsed = 0; // 已经过去的时间

        while (timeElapsed < animationSpeed)
        {
            float leftoverAmount = Mathf.Lerp((currentHealth.previous / healthSystem.MaximumHealth) + cacheLeftoverAmount, finalValue, timeElapsed / animationSpeed);
            // 起始值为currentHealth.previous / healthSystem.MaximumHealth, 结束值为finalValue, 插值为timeElapsed / animationSpeed。 leftoverAmount是当前的剩余量
            this.leftoverAmount = leftoverAmount - finalValue; // 计算剩余量
            image.fillAmount = leftoverAmount;
            timeElapsed += Time.deltaTime;
            yield return null; // 用于暂停协程的执行，直到下一帧(相当于等待一帧)
        }

        this.leftoverAmount = 0;
        image.fillAmount = finalValue;
    }
}
