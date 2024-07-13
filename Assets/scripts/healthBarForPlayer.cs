using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowCameraRotation))]
public class HealthBarForPlayer : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true; // �Ƿ�ʼ�����������
    [SerializeField] bool shouldShowHealthNumbers = true; // �Ƿ���ʾ����ֵ

    float finalValue;
    float animationSpeed = 0.2f;
    float leftoverAmount = 0f;

    // Caches
    HealthSystemForDummies healthSystem;
    public Image image;
    Text text;
    FollowCameraRotation followCameraRotation;

    private void Start()
    {
        healthSystem = GetComponentInParent<HealthSystemForDummies>();
        /*image = GetComponentInChildren<Image>();*/
        text = GetComponentInChildren<Text>();
        followCameraRotation = GetComponent<FollowCameraRotation>();
        healthSystem.OnCurrentHealthChanged.AddListener(ChangeHealthFill); // ��Ӽ�����, ������ֵ�ı�ʱ���� ChangeHealthFill ����
    }

    void Update()
    {
        animationSpeed = healthSystem.AnimationDuration;

        if (!healthSystem.HasAnimationWhenHealthChanges)
        {
            image.fillAmount = healthSystem.CurrentHealthPercentage / 100; // ���½���ֵ
        }

        text.text = $"{healthSystem.CurrentHealth}/{healthSystem.MaximumHealth}";

        text.enabled = shouldShowHealthNumbers;

        followCameraRotation.enabled = isBillboarded;
    }

    private void ChangeHealthFill(CurrentHealth currentHealth)
    {
        if (!healthSystem.HasAnimationWhenHealthChanges)
        {
            return; // ���û�ж�����ֱ�ӷ���
        }

        StopAllCoroutines(); // ֹͣ����Э��
        StartCoroutine(ChangeFillAmount(currentHealth)); // ����Э��
    }

    private IEnumerator ChangeFillAmount(CurrentHealth currentHealth) // ������䶯��Э��
    {
        finalValue = currentHealth.percentage / 100; // ��ʾ��������Ŀ�������(����ֵ)

        float cacheLeftoverAmount = this.leftoverAmount; // ����ʣ����

        float timeElapsed = 0; // �Ѿ���ȥ��ʱ��

        while (timeElapsed < animationSpeed)
        {
            float leftoverAmount = Mathf.Lerp((currentHealth.previous / healthSystem.MaximumHealth) + cacheLeftoverAmount, finalValue, timeElapsed / animationSpeed);
            // ��ʼֵΪcurrentHealth.previous / healthSystem.MaximumHealth, ����ֵΪfinalValue, ��ֵΪtimeElapsed / animationSpeed�� leftoverAmount�ǵ�ǰ��ʣ����
            this.leftoverAmount = leftoverAmount - finalValue; // ����ʣ����
            image.fillAmount = leftoverAmount;
            timeElapsed += Time.deltaTime;
            yield return null; // ������ͣЭ�̵�ִ�У�ֱ����һ֡(�൱�ڵȴ�һ֡)
        }

        this.leftoverAmount = 0;
        image.fillAmount = finalValue;
    }
}
