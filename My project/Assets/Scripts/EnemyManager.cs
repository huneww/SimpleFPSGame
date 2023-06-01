using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static EnemyManager Instance;
    // 스폰 딜레이
    [SerializeField]
    private float spawnDelay;

    // 코루틴 스폰 딜레이 시간
    private WaitForSeconds spawnTime;

    // 적 프리펩
    [SerializeField]
    private GameObject[] enemyPrefabs;
    // 적 소환될 오브젝트
    [SerializeField]
    private GameObject plane;
    // 슬라임 오브젝트 풀
    [SerializeField]
    private Queue<GameObject> slimePool = new Queue<GameObject>();
    // 거불이 오브젝트 풀
    [SerializeField]
    private Queue<GameObject> turtlePool = new Queue<GameObject>();
    // 슬라임 부모 오브젝트
    [SerializeField]
    private Transform slimeParent;
    // 거북이 부모 오브젝트
    [SerializeField]
    private Transform turtleParent;
    // 적 소환 사이즈 확인 콜라이더
    private MeshCollider meshCollider;

    private void Awake()
    {
        // 싱글토 생성
        Instance = this;
        // 코룬티 딜레이 타임 설정
        spawnTime = new WaitForSeconds(spawnDelay);
    }

    /// <summary>
    /// 몬스터 소환 코루틴 실행 메서드
    /// </summary>
    public void StartInvoke()
    {
        // 3초후 코루틴 함수 실행
        Invoke("StartRoutine", 3f);
    }

    private void StartRoutine()
    {
        // 코루틴 함수 실행
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void Start()
    {
        // 소환판 콜라이더 정보 획득
        meshCollider = plane.GetComponent<MeshCollider>();

        // 각 몬스터 15개 생성
        for (int i = 0; i < 15; i ++)
        {
            // 슬라임 오브젝트 생성
            GameObject slime = Instantiate(enemyPrefabs[0], Vector3.zero, enemyPrefabs[0].transform.rotation);
            // 부모 오브젝트 설정
            slime.transform.parent = slimeParent;
            // 오브젝트 비활성화
            slime.SetActive(false);
            // 풀에 추가
            slimePool.Enqueue(slime);
            // 거북이 오브젝트 생성
            GameObject turtle = Instantiate(enemyPrefabs[1], Vector3.zero, enemyPrefabs[1].transform.rotation);
            // 부모 오브젝트 설정
            turtle.transform.parent = turtleParent;
            // 오브젝트 비활성화
            turtle.SetActive(false);
            // 풀에 추가
            turtlePool.Enqueue(turtle);
        }
    }

    /// <summary>
    /// 적 생성
    /// </summary>
    private void SpawnEnemy()
    {
        // 소환할 오브젝트 저장 변수
        GameObject obj = null;
        // 0에서 적프리펩수에서 랜덤 숫자 획득
        int random = Random.Range(0, enemyPrefabs.Length);

        switch (random)
        {
            // 0이면
            case 0:
                if (slimePool.Count == 0)
                    CreateEnemy(random);
                // 슬라임 풀에서 오브젝트 획득
                obj = slimePool.Dequeue();
                break;
            // 1이면
            case 1:
                if (turtlePool.Count == 0)
                    CreateEnemy(random);
                // 거북이 풀에서 오브젝트 획득
                obj = turtlePool.Dequeue();
                break;
        }

        // 획득한 오브젝트 활성화
        obj.SetActive(true);
        // 오브젝트 위치 랜덤 지정
        obj.transform.position = ReturnRandomSpawnPos();
    }

    /// <summary>
    /// 몬스터 생성
    /// </summary>
    /// <param name="value">생성할 몬스터 번호</param>
    private void CreateEnemy(int value)
    {
        if (value == 0)
        {
            // 슬라임 오브젝트 생성
            GameObject slime = Instantiate(enemyPrefabs[0], Vector3.zero, enemyPrefabs[0].transform.rotation);
            // 부모 오브젝트 설정
            slime.transform.parent = slimeParent;
            // 오브젝트 비활성화
            slime.SetActive(false);
            // 풀에 추가
            slimePool.Enqueue(slime);
        }
        else if (value == 1)
        {
            // 거북이 오브젝트 생성
            GameObject turtle = Instantiate(enemyPrefabs[1], Vector3.zero, enemyPrefabs[1].transform.rotation);
            // 부모 오브젝트 설정
            turtle.transform.parent = turtleParent;
            // 오브젝트 비활성화
            turtle.SetActive(false);
            // 풀에 추가
            turtlePool.Enqueue(turtle);
        }
        else
        {
            // 오류 메시지 출력
            Debug.Log("CreateEnemy Value Error!");
            // 메서드 종료
            return;
        }
    }

    /// <summary>
    /// 죽은 적 반환
    /// </summary>
    /// <param name="returnobj">반환할 오브젝트</param>
    public void ReturnEnemy(GameObject returnobj)
    {
        // 반환할 오브젝트 비활성화
        returnobj.SetActive(false);

        // 반환 오브젝트의 태그가 슬라임이라면
        if (returnobj.name.Contains("Slime"))
        {
            // 슬라임 풀에 추가
            slimePool.Enqueue(returnobj);
        }
        // 반환 오브젝트의 태그가 거북이라면
        else if (returnobj.name.Contains("Turtle"))
        {
            // 거북이 풀에 추가
            turtlePool.Enqueue(returnobj);
        }
    }

    /// <summary>
    /// 적 랜덤 위치 반환
    /// </summary>
    /// <returns>랜덤한 Vector3 반환</returns>
    private Vector3 ReturnRandomSpawnPos()
    {
        // 판의 X사이즈 획득
        float rangeX = meshCollider.bounds.size.x;
        // 판의 Z사이즈 획득
        float rangeZ = meshCollider.bounds.size.z;

        int random = Random.Range(0, 4);

        // 판 끝쪽에 몬스터 소환
        if (random == 0)
        {
            rangeX = (rangeX / 2) * -1;
            rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);
        }
        else if (random == 1)
        {
            rangeX = (rangeX / 2);
            rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);
        }
        else if (random == 2)
        {
            rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
            rangeZ = (rangeZ / 2) * -1;
        }
        else
        {
            rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
            rangeZ = (rangeZ / 2);
        }
        
        //// 판의 사이즈에서 랜덤한 값 획득
        //rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
        //rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);

        // 랜덤으로 획득한 값에 저장
        Vector3 spawnPos = Vector3.zero + new Vector3(rangeX, 0, rangeZ);

        // 랜덤 위치 반환
        return spawnPos;
    }

    /// <summary>
    /// 적 스폰 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyRoutine()
    {
        // 각 풀에 아무것도 없지 안을때까지 반복
        while (true)
        {
            if (GameManager.Instance.isDead)
                yield break;

            // SpawnEnemy메서드 실행
            SpawnEnemy();
            // 딜레이 타임마다 실행
            yield return spawnTime;
        }
    }
}
