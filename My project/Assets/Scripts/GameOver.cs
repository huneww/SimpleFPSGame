using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;

    // ���� ���� ��� �ؽ�Ʈ
    [SerializeField]
    private Text score;
    // �������̽� �׷�
    [SerializeField]
    private CanvasGroup canvasGroup;
    // ���̵��� �ɸ��� �ð�
    [SerializeField]
    private float required = 1f;

    private void Awake()
    {
        // �̱��� ����
        Instance = this;
    }

    public IEnumerator GameOverRoutine()
    {
        // ���� ���� �ؽ�Ʈ ����
        score.text = "Score : " + GameManager.Instance.scoreText.text;

        while (canvasGroup.alpha < 1)
        {
            // �׷� ���İ� ����
            canvasGroup.alpha += Time.deltaTime / required;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}
