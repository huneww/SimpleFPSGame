using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    // X축으로 회전 속도
    [SerializeField]
    private float rotCamXSpeed = 5f;
    // Y축으로 회전 속도
    [SerializeField]
    private float rotCamYSpeed = 3f;

    // 카메라 오브젝트
    [SerializeField]
    private GameObject camer;

    // 플레이어 회전각도 리미트
    private float clampAngleLimit = 80;
    // X축 각도 저장 변수
    private float eulerAngleX;
    // Y축 각도 저장 변수
    private float eulerAngleY;

    public void RotateUpdate(float mouseX, float mouseY)
    {

        // 현재 마우스 Y좌표 * 회전속도 저장
        eulerAngleX -= mouseY * rotCamXSpeed;
        // 현재 마우스 X좌표 * 회전속도 저장
        eulerAngleY += mouseX * rotCamYSpeed;
        // X축은 회전각 최대,최소치를 넘지 안도록 설정
        eulerAngleX = Mathf.Clamp(eulerAngleX, -clampAngleLimit, clampAngleLimit);
        // 카메라 회전값 변경
        camer.transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
        // 플레이어는 Y축만 변경
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
    }
}
