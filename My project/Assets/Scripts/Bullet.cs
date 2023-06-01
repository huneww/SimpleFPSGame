using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알 미는 힘
    [SerializeField]
    private float bulletForcePower;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private Vector3 bulletCurRotation;

    // 리지드바디
    private Rigidbody rigid;

    private void Awake()
    {
        bulletSpawn = GameObject.FindGameObjectWithTag("BulletSpawn").transform;
    }

    // 오브젝트가 활성화 될 때
    private void OnEnable()
    {
        // rigid가 없다면
        if (rigid == null)
        {
            // 리지드바디 컴포넌트 가져오기
            rigid = GetComponent<Rigidbody>();
        }

        // 총알 위치 초기화
        transform.position = bulletSpawn.position;
        // 총알 회전값 초기화
        transform.rotation = Quaternion.Euler(
            bulletCurRotation.x + PlayerController.Instance.camer.transform.rotation.eulerAngles.x,
            bulletCurRotation.y + PlayerController.Instance.camer.transform.rotation.eulerAngles.y,
            bulletCurRotation.z + PlayerController.Instance.camer.transform.rotation.eulerAngles.z);

        // 총알 속도 Z축 방향으로 밀기
        rigid.velocity = transform.up * bulletForcePower;

        // 일정 시간후 총알 회수
        Invoke("ReturnBullet", 3f);
    }

    private void ReturnBullet()
    {
        // PlayerAttack 스크립트에있는 회수 메서드 호출
        PlayerController.Instance.playerAttack.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 인보크함수 해제
        CancelInvoke("ReturnBullet");

        // 오브젝트와 충돌시
        // PlayerAttack 스크립트에있는 회수 메서드 호출
        PlayerController.Instance.playerAttack.ReturnBullet(gameObject);

        // 적과 충돌시 적 Hit메서드 실행
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyMove>().Hit();
        }
    }
}