using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance;

    // �Ѿ� ������
    [SerializeField]
    private GameObject bulletPre;
    // �Ѿ� ���� ��ġ
    [SerializeField]
    private Transform bulletSpawn;
    // �Ѿ� Ǯ �θ� ��ü
    [SerializeField]
    private Transform bulletParent;
    // �Ѿ� Ǯ
    [SerializeField]
    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    // �Ѿ� ���� ���� ����
    [SerializeField]
    private Vector3 bulletCurRotation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // �Ѿ� Ǯ�� �Ѿ� ����
        for (int i = 0; i < 30; i++)
        {
            // ������Ʈ ����
            GameObject obj = Instantiate(bulletPre, bulletSpawn.position, bulletPre.transform.rotation);
            // ������Ʈ ��Ȱ��ȭ
            obj.SetActive(false);
            // ������Ʈ �θ� ����
            obj.transform.parent = bulletParent;
            // ������Ʈ Ǯ�� �߰�
            bulletPool.Enqueue(obj);
        }
    }


    public void SpawnBullet(Vector3 rotation)
    {
        // Ǯ���� �Ѿ� ����
        GameObject bullet = bulletPool.Dequeue();
        // �Ѿ� Ȱ��ȭ
        bullet.SetActive(true);
    }

    /// <summary>
    /// �Ѿ� ȸ�� �޼���
    /// </summary>
    /// <param name="returnBullet">ȸ���� �Ѿ�</param>
    public void ReturnBullet(GameObject returnBullet)
    {
        // ȸ���� �Ѿ� �ʱ�ȭ
        returnBullet.SetActive(false);
        // ��ġ �ʱ�ȭ
        returnBullet.transform.position = Vector3.zero;
        // ȸ���� �⺻ ������ ����
        returnBullet.transform.rotation = Quaternion.Euler(bulletCurRotation);
        // ȸ���� �Ѿ� �ӵ� �ʱ�ȭ
        returnBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        // �Ѿ� Ǯ�� ��ȯ
        bulletPool.Enqueue(returnBullet);
    }
}
