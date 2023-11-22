using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController1 : MonoBehaviour
{
    float fixtime;
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; //�޸��� Ű
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;     //���� Ű 

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;
    [SerializeField]
    private AudioClip audioClipRun;



    private RotateToMouse                 rotateToMouse;   //���콺 �̵����� ī�޶� ȸ��
    private MovementCharacterController   movement;        //Ű���� �Է����� �÷��̾� �̵�,����
    private Status                        status;          //�̵��ӵ� ���� �÷��̾� ���� 
    private PlayerAnimatorController      animator;         //�ִϸ��̼� �������
    private WeaponAssaultRifle            weapon;           // ���⸦ �̿��� ��������


    private void Awake()
    {
        //���콺 Ŀ���� ������ �ʰ� �����ϰ�, ���� ��ġ�� ������Ų��
        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();

    }

    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
        fixtime = Time.unscaledDeltaTime;
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //�̵��� �϶� (�ȱ� or �ٱ�)
        if (x != 0 || z != 0)
        {
            bool isRun = false;

            //���̳� �ڷ� �̵��� ���� �޸� �� ����
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
        }
        //���ڸ��� ����������
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }

    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Finish") {
            SceneManager.LoadScene("MainScene");
        }
    }
}
