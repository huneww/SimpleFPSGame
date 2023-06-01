using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    // 현재 점수 출력 텍스트
    [SerializeField]
    private Text score;
    // 인터페이스 그룹
    [SerializeField]
    private CanvasGroup canvasGroup;
    // 페이드인 걸리는 시간
    [SerializeField]
    private float required = 1f;

    private void Awake()
    {
        // 싱글톤 생성
        Instance = this;
    }

    public IEnumerator GameOverRoutine()
    {
        // 현재 점수 텍스트 저장
        score.text = "Score : " + GameManager.Instance.scoreText.text;

        while (canvasGroup.alpha < 1)
        {
            // 그룹 알파값 변경
            canvasGroup.alpha += Time.deltaTime / required;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}
