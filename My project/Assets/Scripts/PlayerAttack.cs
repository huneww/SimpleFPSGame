using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;

    // 총알 프리펩
    [SerializeField]
    private GameObject bulletPre;
    // 총알 스폰 위치
    [SerializeField]
    private Transform bulletSpawn;
    // 총알 풀 부모 객체
    [SerializeField]
    private Transform bulletParent;
    // 총알 풀
    [SerializeField]
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    // 총알 현재 각도 저장
    [SerializeField]
    private Vector3 bulletCurRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 총알 풀에 총알 생성
        for (int i = 0; i < 30; i++)
        {
            // 오브젝트 생성
            GameObject obj = Instantiate(bulletPre, bulletSpawn.position, bulletPre.transform.rotation);
            // 오브젝트 비활성화
            obj.SetActive(false);
            // 오브젝트 부모 지정
            obj.transform.parent = bulletParent;
            // 오브젝트 풀에 추가
            bulletPool.Enqueue(obj);
        }
    }


    public void SpawnBullet(Vector3 rotation)
    {
        // 풀에서 총알 제거
        GameObject bullet = bulletPool.Dequeue();
        // 총알 활성화
        bullet.SetActive(true);
    }

    /// <summary>
    /// 총알 회수 메서드
    /// </summary>
    /// <param name="returnBullet">회수할 총알</param>
    public void ReturnBullet(GameObject returnBullet)
    {
        // 회수할 총알 초기화
        returnBullet.SetActive(false);
        // 위치 초기화
        returnBullet.transform.position = Vector3.zero;
        // 회전각 기본 각도로 변경
        returnBullet.transform.rotation = Quaternion.Euler(bulletCurRotation);
        // 회수할 총알 속도 초기화
        returnBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // 총알 풀에 반환
        bulletPool.Enqueue(returnBullet);
    }
}
