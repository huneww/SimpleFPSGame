using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static EnemyManager Instance;
    // ���� ������
    [SerializeField]
    private float spawnDelay;

    // �ڷ�ƾ ���� ������ �ð�
    private WaitForSeconds spawnTime;

    // �� ������
    [SerializeField]
    private GameObject[] enemyPrefabs;
    // �� ��ȯ�� ������Ʈ
    [SerializeField]
    private GameObject plane;
    // ������ ������Ʈ Ǯ
    [SerializeField]
    private Queue<GameObject> slimePool = new Queue<GameObject>();
    // �ź��� ������Ʈ Ǯ
    [SerializeField]
    private Queue<GameObject> turtlePool = new Queue<GameObject>();
    // ������ �θ� ������Ʈ
    [SerializeField]
    private Transform slimeParent;
    // �ź��� �θ� ������Ʈ
    [SerializeField]
    private Transform turtleParent;
    // �� ��ȯ ������ Ȯ�� �ݶ��̴�
    private MeshCollider meshCollider;

    private void Awake()
    {
        // �̱��� ����
        Instance = this;
        // �ڷ�Ƽ ������ Ÿ�� ����
        spawnTime = new WaitForSeconds(spawnDelay);
    }

    /// <summary>
    /// ���� ��ȯ �ڷ�ƾ ���� �޼���
    /// </summary>
    public void StartInvoke()
    {
        // 3���� �ڷ�ƾ �Լ� ����
        Invoke("StartRoutine", 3f);
    }

    private void StartRoutine()
    {
        // �ڷ�ƾ �Լ� ����
        StartCoroutine(SpawnEnemyRoutine());
    }

    private void Start()
    {
        // ��ȯ�� �ݶ��̴� ���� ȹ��
        meshCollider = plane.GetComponent<MeshCollider>();

        // �� ���� 15�� ����
        for (int i = 0; i < 15; i ++)
        {
            // ������ ������Ʈ ����
            GameObject slime = Instantiate(enemyPrefabs[0], Vector3.zero, enemyPrefabs[0].transform.rotation);
            // �θ� ������Ʈ ����
            slime.transform.parent = slimeParent;
            // ������Ʈ ��Ȱ��ȭ
            slime.SetActive(false);
            // Ǯ�� �߰�
            slimePool.Enqueue(slime);
            // �ź��� ������Ʈ ����
            GameObject turtle = Instantiate(enemyPrefabs[1], Vector3.zero, enemyPrefabs[1].transform.rotation);
            // �θ� ������Ʈ ����
            turtle.transform.parent = turtleParent;
            // ������Ʈ ��Ȱ��ȭ
            turtle.SetActive(false);
            // Ǯ�� �߰�
            turtlePool.Enqueue(turtle);
        }
    }

    /// <summary>
    /// �� ����
    /// </summary>
    private void SpawnEnemy()
    {
        // ��ȯ�� ������Ʈ ���� ����
        GameObject obj = null;
        // 0���� ������������� ���� ���� ȹ��
        int random = Random.Range(0, enemyPrefabs.Length);

        switch (random)
        {
            // 0�̸�
            case 0:
                if (slimePool.Count == 0)
                    CreateEnemy(random);
                // ������ Ǯ���� ������Ʈ ȹ��
                obj = slimePool.Dequeue();
                break;
            // 1�̸�
            case 1:
                if (turtlePool.Count == 0)
                    CreateEnemy(random);
                // �ź��� Ǯ���� ������Ʈ ȹ��
                obj = turtlePool.Dequeue();
                break;
        }

        // ȹ���� ������Ʈ Ȱ��ȭ
        obj.SetActive(true);
        // ������Ʈ ��ġ ���� ����
        obj.transform.position = ReturnRandomSpawnPos();
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="value">������ ���� ��ȣ</param>
    private void CreateEnemy(int value)
    {
        if (value == 0)
        {
            // ������ ������Ʈ ����
            GameObject slime = Instantiate(enemyPrefabs[0], Vector3.zero, enemyPrefabs[0].transform.rotation);
            // �θ� ������Ʈ ����
            slime.transform.parent = slimeParent;
            // ������Ʈ ��Ȱ��ȭ
            slime.SetActive(false);
            // Ǯ�� �߰�
            slimePool.Enqueue(slime);
        }
        else if (value == 1)
        {
            // �ź��� ������Ʈ ����
            GameObject turtle = Instantiate(enemyPrefabs[1], Vector3.zero, enemyPrefabs[1].transform.rotation);
            // �θ� ������Ʈ ����
            turtle.transform.parent = turtleParent;
            // ������Ʈ ��Ȱ��ȭ
            turtle.SetActive(false);
            // Ǯ�� �߰�
            turtlePool.Enqueue(turtle);
        }
        else
        {
            // ���� �޽��� ���
            Debug.Log("CreateEnemy Value Error!");
            // �޼��� ����
            return;
        }
    }

    /// <summary>
    /// ���� �� ��ȯ
    /// </summary>
    /// <param name="returnobj">��ȯ�� ������Ʈ</param>
    public void ReturnEnemy(GameObject returnobj)
    {
        // ��ȯ�� ������Ʈ ��Ȱ��ȭ
        returnobj.SetActive(false);

        // ��ȯ ������Ʈ�� �±װ� �������̶��
        if (returnobj.name.Contains("Slime"))
        {
            // ������ Ǯ�� �߰�
            slimePool.Enqueue(returnobj);
        }
        // ��ȯ ������Ʈ�� �±װ� �ź��̶��
        else if (returnobj.name.Contains("Turtle"))
        {
            // �ź��� Ǯ�� �߰�
            turtlePool.Enqueue(returnobj);
        }
    }

    /// <summary>
    /// �� ���� ��ġ ��ȯ
    /// </summary>
    /// <returns>������ Vector3 ��ȯ</returns>
    private Vector3 ReturnRandomSpawnPos()
    {
        // ���� X������ ȹ��
        float rangeX = meshCollider.bounds.size.x;
        // ���� Z������ ȹ��
        float rangeZ = meshCollider.bounds.size.z;

        int random = Random.Range(0, 4);

        // �� ���ʿ� ���� ��ȯ
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
        
        //// ���� ������� ������ �� ȹ��
        //rangeX = Random.Range((rangeX / 2) * -1, rangeX / 2);
        //rangeZ = Random.Range((rangeZ / 2) * -1, rangeZ / 2);

        // �������� ȹ���� ���� ����
        Vector3 spawnPos = Vector3.zero + new Vector3(rangeX, 0, rangeZ);

        // ���� ��ġ ��ȯ
        return spawnPos;
    }

    /// <summary>
    /// �� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemyRoutine()
    {
        // �� Ǯ�� �ƹ��͵� ���� ���������� �ݺ�
        while (true)
        {
            if (GameManager.Instance.isDead)
                yield break;

            // SpawnEnemy�޼��� ����
            SpawnEnemy();
            // ������ Ÿ�Ӹ��� ����
            yield return spawnTime;
        }
    }
}
