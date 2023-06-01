using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �̱��� ����
    public static PlayerController Instance;

    // ī�޶� ������Ʈ
    public GameObject camer;

    // �÷��̾� �̵��ӵ�
    [SerializeField]
    private float speed = 5f;
    // �÷��̾� ���� �Ŀ�
    [SerializeField]
    private float jumpPower = 10f;
    // �÷��̾� ���� ��Ÿ��
    [SerializeField]
    private float jumpDelay = 1.5f;
    // ������ ��� �ð� ����
    private float jumpCurTime;

    // ������ٵ�
    private Rigidbody rigid;
    // �÷��̾� �̵� ��ġ
    private Vector3 movement;
    // ���콺 ȸ���� �÷��̾� ȸ��
    private RotateToMouse rotateToMouse;
    // �÷��̾� ���� ��ũ��Ʈ
    [HideInInspector]
    public PlayerAttack playerAttack;

    // ���� ������ ���� ����
    private int bulletCount;
    // �߻� ������
    private float shootDelay = 0.15f;
    // �߻� ������ üũ
    private float curShootTime;

    private void Awake()
    {

        // �ν��Ͻ� ���� ��ü�� ����
        Instance = this;

        // ���۽� ���� Ÿ�� �ʱ�ȭ
        jumpCurTime = jumpDelay;
        curShootTime = shootDelay;
        bulletCount = 0;
    }

    private void Start()
    {
        // �� ������Ʈ �޾� ����
        rigid = GetComponent<Rigidbody>();
        rotateToMouse = GetComponent<RotateToMouse>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        // ���� �÷������̾ƴ϶��
        if (!GameManager.Instance.isPlaying || GameManager.Instance.isDead)
            return;

        // ���콺 ȸ���� ī�޶� ������Ʈ �� �ڽ� ��ü ȸ��
        RotateUpdate();

        // �̵� ��ǲ �޾� ����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // �̵� ���Ϳ� ����
        movement.Set(h, 0, v);

        // �÷��̾��� ��ġ ����
        transform.Translate(movement * speed * Time.deltaTime);

        curShootTime += Time.deltaTime;

        // ���콺 ��Ŭ����
        if (Input.GetMouseButton(0) && curShootTime >= shootDelay)
        {
            // SpawnBullet �޼��� ȣ��, ���� �÷��̾� ȸ������ ����
            playerAttack.SpawnBullet(transform.rotation.eulerAngles);
            // ������ Ÿ�� �ʱ�ȭ
            curShootTime = 0;
            // ȿ���� �÷���
            SFXManager.Instance.ShootSound();
        }

    }

    private void RotateUpdate()
    {
        // �� �����Ӹ��� ���콺 ��ġ Ȯ��
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // ���콺 ��ġ�� �޾� ī�޶� ȸ��
        rotateToMouse.RotateUpdate(mouseX, mouseY);
    }

    private void FixedUpdate()
    {
        // ���� �÷������� �ƴ϶��
        if (!GameManager.Instance.isPlaying)
            return;

        // ����ð� �� ����
        jumpCurTime += Time.deltaTime;
        // �����̽��� ����� ��Ÿ���� �����ٸ�
        if (Input.GetButton("Jump") && jumpCurTime >= jumpDelay)
        {
            // �÷��̾ ���� ����, �÷��̾��� ����/10 ��ŭ �� �߰�
            rigid.AddForce(Vector3.up * jumpPower * (rigid.mass / 10), ForceMode.Impulse);
            // ���� ����ð� �ʱ�ȭ
            jumpCurTime = 0f;
        }
    }
}
