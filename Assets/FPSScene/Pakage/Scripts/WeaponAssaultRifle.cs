using System.Collections;
using UnityEngine;

public class WeaponAssaultRifle : MonoBehaviour
{
    float fixtime;
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipTakeOutWeapon;


    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip   audioClipTakeoutWeapon; //�������� ����
    [SerializeField]
    private AudioClip audioClipFire;

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting           weaponSetting;          //���� ����

    private float                   lastAttackTime = 0;     //������ �߻�ð� üũ��

    private AudioSource                     audioSource;    //���� ��� ������Ʈ
    private PlayerAnimatorController        animator;       //�ִϸ��̼� ��� ����
                                                            
    private void Awake()
    {
        audioSource     = GetComponent<AudioSource>();
        animator        = GetComponentInParent<PlayerAnimatorController>();
        fixtime = Time.unscaledDeltaTime;
    }

   private void OnEnable()       
    {
        PlaySound(audioClipTakeoutWeapon);
        muzzleFlashEffect.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    { 
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StartWeaponAction(int type = 0)
    {
        //���콺 ����Ŭ��(���ݽ���)
        if (type == 0)
        {
            //����
            if (weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            //�ܹ�
            else
            {
                OnAttack();
            }
        }
    }
    public void StopWeaponAction(int type = 0)
    {
        // ���콺 ����Ŭ�� (��������)
        if (type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    private IEnumerator OnAttackLoop()
    {
        while (true)
        { 
            OnAttack();

            yield return null;
        }
    }
    public void OnAttack()
    {
        if (Time.realtimeSinceStartup - lastAttackTime > weaponSetting.attackRate)
        {
            //�ٰ� �������� ���� X
            if (animator.MoveSpeed < 0.5f)
            {
                return;
            }
            //�����ֱⰡ �Ǿ�� ���� �Ҽ� �ֵ��� ����ð� ���� 
            lastAttackTime = Time.realtimeSinceStartup;

            //���� �ִϸ��̼� ���
            animator.Play("Fire", -1, 0);

            //�ѱ� ����Ʈ ���
            StartCoroutine("OnMuzzleFlashEffect");

            ////���� ����
            PlaySound(audioClipFire);
        }
    }
    
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }
}
