using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 싱글톤 패턴
    public static PlayerController Instance;

    // 카메라 오브젝트
    public GameObject camer;

    // 플레이어 이동속도
    [SerializeField]
    private float speed = 5f;
    // 플레이어 점프 파워
    [SerializeField]
    private float jumpPower = 10f;
    // 플레이어 점프 쿨타임
    [SerializeField]
    private float jumpDelay = 1.5f;
    // 점프시 경과 시간 저장
    private float jumpCurTime;

    // 리지드바디
    private Rigidbody rigid;
    // 플레이어 이동 위치
    private Vector3 movement;
    // 마우스 회전시 플레이어 회전
    private RotateToMouse rotateToMouse;
    // 플레이어 공격 스크립트
    [HideInInspector]
    public PlayerAttack playerAttack;

    // 남은 잔탕수 저장 변수
    private int bulletCount;
    // 발사 딜레이
    private float shootDelay = 0.15f;
    // 발사 딜레이 체크
    private float curShootTime;

    private void Awake()
    {

        // 인스턴스 현재 객체로 지정
        Instance = this;

        // 시작시 점프 타임 초기화
        jumpCurTime = jumpDelay;
        curShootTime = shootDelay;
        bulletCount = 0;
    }

    private void Start()
    {
        // 각 컴포넌트 받아 오기
        rigid = GetComponent<Rigidbody>();
        rotateToMouse = GetComponent<RotateToMouse>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        // 게임 플레이중이아니라면
        if (!GameManager.Instance.isPlaying || GameManager.Instance.isDead)
            return;

        // 마우스 회전시 카메라 오브젝트 및 자식 객체 회전
        RotateUpdate();

        // 이동 인풋 받아 저장
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 이동 벡터에 저장
        movement.Set(h, 0, v);

        // 플레이어의 위치 변경
        transform.Translate(movement * speed * Time.deltaTime);

        curShootTime += Time.deltaTime;

        // 마우스 왼클릭시
        if (Input.GetMouseButton(0) && curShootTime >= shootDelay)
        {
            // SpawnBullet 메서드 호출, 현재 플레이어 회전각도 전달
            playerAttack.SpawnBullet(transform.rotation.eulerAngles);
            // 딜레이 타임 초기화
            curShootTime = 0;
            // 효과음 플레이
            SFXManager.Instance.ShootSound();
        }

    }

    private void RotateUpdate()
    {
        // 매 프레임마다 마우스 위치 확인
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 마우스 위치를 받아 카메라 회전
        rotateToMouse.RotateUpdate(mouseX, mouseY);
    }

    private void FixedUpdate()
    {
        // 게임 플레이중이 아니라면
        if (!GameManager.Instance.isPlaying)
            return;

        // 경과시간 값 변경
        jumpCurTime += Time.deltaTime;
        // 스페이스바 누루고 쿨타임이 지났다면
        if (Input.GetButton("Jump") && jumpCurTime >= jumpDelay)
        {
            // 플레이어를 위로 점프, 플레이어의 질량/10 만큼 힘 추가
            rigid.AddForce(Vector3.up * jumpPower * (rigid.mass / 10), ForceMode.Impulse);
            // 점프 경과시간 초기화
            jumpCurTime = 0f;
        }
    }
}
