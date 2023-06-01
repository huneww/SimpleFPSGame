using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    // 효과은 오디오소스
    [SerializeField]
    private AudioSource[] SFX;
    // 발사 효과음
    [SerializeField]
    private AudioClip gunShootClip;
    // 플레이할 오디오소스 인덱스
    private int index;

    private void Awake()
    {
        Instance = this;
        index = 0;
    }

    public void ShootSound()
    {
        // 오디오 클립 설정
        SFX[index].clip = gunShootClip;
        // 오디오 실행
        SFX[index].Play();
        // 인덱스값 변경
        index = (index + 1) % SFX.Length;
    }

    // 효과음 볼륨 조절
    public void SFXVolumeChange()
    {
        foreach (var source in SFX)
        {
            // 각 SFX 플레이어 볼륨 설정
            source.volume = GameManager.Instance.SFXBar.value;
        }
    }
}
