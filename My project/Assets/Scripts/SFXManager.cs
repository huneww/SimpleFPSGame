using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // ȿ���� ������ҽ�
    [SerializeField]
    private AudioSource[] SFX;
    // �߻� ȿ����
    [SerializeField]
    private AudioClip gunShootClip;
    // �÷����� ������ҽ� �ε���
    private int index;

    private void Awake()
    {
        Instance = this;
        index = 0;
    }

    public void ShootSound()
    {
        // ����� Ŭ�� ����
        SFX[index].clip = gunShootClip;
        // ����� ����
        SFX[index].Play();
        // �ε����� ����
        index = (index + 1) % SFX.Length;
    }

    // ȿ���� ���� ����
    public void SFXVolumeChange()
    {
        foreach (var source in SFX)
        {
            // �� SFX �÷��̾� ���� ����
            source.volume = GameManager.Instance.SFXBar.value;
        }
    }
}
