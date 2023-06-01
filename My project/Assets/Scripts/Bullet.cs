using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // �Ѿ� �̴� ��
    [SerializeField]
    private float bulletForcePower;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private Vector3 bulletCurRotation;

    // ������ٵ�
    private Rigidbody rigid;

    private void Awake()
    {
        bulletSpawn = GameObject.FindGameObjectWithTag("BulletSpawn").transform;
    }

    // ������Ʈ�� Ȱ��ȭ �� ��
    private void OnEnable()
    {
        // rigid�� ���ٸ�
        if (rigid == null)
        {
            // ������ٵ� ������Ʈ ��������
            rigid = GetComponent<Rigidbody>();
        }

        // �Ѿ� ��ġ �ʱ�ȭ
        transform.position = bulletSpawn.position;
        // �Ѿ� ȸ���� �ʱ�ȭ
        transform.rotation = Quaternion.Euler(
            bulletCurRotation.x + PlayerController.Instance.camer.transform.rotation.eulerAngles.x,
            bulletCurRotation.y + PlayerController.Instance.camer.transform.rotation.eulerAngles.y,
            bulletCurRotation.z + PlayerController.Instance.camer.transform.rotation.eulerAngles.z);

        // �Ѿ� �ӵ� Z�� �������� �б�
        rigid.velocity = transform.up * bulletForcePower;

        // ���� �ð��� �Ѿ� ȸ��
        Invoke("ReturnBullet", 3f);
    }

    private void ReturnBullet()
    {
        // PlayerAttack ��ũ��Ʈ���ִ� ȸ�� �޼��� ȣ��
        PlayerController.Instance.playerAttack.ReturnBullet(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �κ�ũ�Լ� ����
        CancelInvoke("ReturnBullet");

        // ������Ʈ�� �浹��
        // PlayerAttack ��ũ��Ʈ���ִ� ȸ�� �޼��� ȣ��
        PlayerController.Instance.playerAttack.ReturnBullet(gameObject);

        // ���� �浹�� �� Hit�޼��� ����
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyMove>().Hit();
        }
    }
}