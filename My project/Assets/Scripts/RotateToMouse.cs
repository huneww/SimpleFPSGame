using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    // X������ ȸ�� �ӵ�
    [SerializeField]
    private float rotCamXSpeed = 5f;
    // Y������ ȸ�� �ӵ�
    [SerializeField]
    private float rotCamYSpeed = 3f;

    // ī�޶� ������Ʈ
    [SerializeField]
    private GameObject camer;

    // �÷��̾� ȸ������ ����Ʈ
    private float clampAngleLimit = 80;
    // X�� ���� ���� ����
    private float eulerAngleX;
    // Y�� ���� ���� ����
    private float eulerAngleY;

    public void RotateUpdate(float mouseX, float mouseY)
    {

        // ���� ���콺 Y��ǥ * ȸ���ӵ� ����
        eulerAngleX -= mouseY * rotCamXSpeed;
        // ���� ���콺 X��ǥ * ȸ���ӵ� ����
        eulerAngleY += mouseX * rotCamYSpeed;
        // X���� ȸ���� �ִ�,�ּ�ġ�� ���� �ȵ��� ����
        eulerAngleX = Mathf.Clamp(eulerAngleX, -clampAngleLimit, clampAngleLimit);
        // ī�޶� ȸ���� ����
        camer.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        // �÷��̾�� Y�ุ ����
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
    }
}
