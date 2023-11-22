using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class MovementCharacterController : MonoBehaviour
{
    float fixtime;
    [SerializeField]
    private float     moveSpeed;         //�̵��ӵ�
    private Vector3   moveForce;         //�̵� �� (x, z�� y���� ������ ����� ���� �̵��� ����)

    [SerializeField]
    private float jumpForce;             //������   
    [SerializeField]
    private float gravity;               //�߷� ��� 

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;    
    }

    private CharacterController characterController;           //�÷��̾� �̵� ��� ���� ������Ʈ 

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        fixtime = Time.unscaledDeltaTime;
    }

    private void Update()
    {
        //����� �������� �߷¸�ŭ y�� �̵��ӵ� ����
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * fixtime;
        }

        //1�ʴ� moveForce �ӷ������̵�
        characterController.Move(moveForce * fixtime);
    }

    public void Jump()
    {
        if( characterController.isGrounded )
        {
            moveForce.y = jumpForce;
        }
    }

    public void MoveTo(Vector3 direction)
    {
        //�̵� ���� = ĳ������ ȸ���� * ���� �� 
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        //�̵� �� = �̵����� * �ӵ�
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }
    
}

