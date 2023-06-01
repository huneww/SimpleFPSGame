using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ���� �÷��������� Ȯ��
    public bool isPlaying;
    // ����� ������ҽ�
    [SerializeField]
    private AudioSource BGM;
    // �� ������Ʈ
    [SerializeField]
    private GameObject Gun;
    // �ΰ��� �������̽�
    [SerializeField]
    private GameObject InGameUI;
    // ���θ޴� �������̽�
    [SerializeField]
    private GameObject TitleUI;
    // �����޴� �������̽�
    [SerializeField]
    private GameObject SettingUI;
    // ���� ���� �ؽ�Ʈ
    [SerializeField]
    public Text scoreText;
    // ���� ���� ����
    private int nowScore;
    // ��Ʈ �̹���
    [SerializeField]
    private Image[] hearts;
    // ��Ʈ ��������Ʈ
    [SerializeField]
    private Sprite[] heartSprite;
    // ���ӿ��� �������̽�
    [SerializeField]
    private GameObject gameOverUI;
    // �ְ� ���� �ؽ�Ʈ
    [SerializeField]
    private Text maxScoreText;
    // ���� ü��
    private int health;
    // �׾����� Ȯ��
    public bool isDead;
    // ����� ���� ���� ��
    public Slider BGMBar;
    // ȿ���� ���� ���� ��
    public Slider SFXBar;

    private void Awake()
    {
        // �̱��� ����
        Instance = this;

        // ����� ���� Ȯ��
        if (!PlayerPrefs.HasKey("BGMVolume"))
            PlayerPrefs.SetFloat("BGMVolume", 1);
        // ȿ���� ���� Ȯ��
        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", 1);
        // �ְ� ���� Ȯ��
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);

        // ����� �� ����
        BGMBar.value = PlayerPrefs.GetFloat("BGMVolume");
        // ����� ����� ���� ����
        BGM.volume = PlayerPrefs.GetFloat("BGMVolume");
        // ȿ���� �� ����
        SFXBar.value = PlayerPrefs.GetFloat("SFXVolume");
        // ȿ���� ����� ���� ����
        SFXManager.Instance.SFXVolumeChange();
        // �ְ� ���� �ؽ�Ʈ ����
        if (PlayerPrefs.GetInt("MaxScore") < 1000)
            maxScoreText.text = "Max Score : " + PlayerPrefs.GetInt("MaxScore").ToString();
        else
            maxScoreText.text = "Max Score : " + GetThousandCommaText(PlayerPrefs.GetInt("MaxScore")).ToString();
    }

    private void Start()
    {
        // �� ������Ʈ ȹ��
        Gun = GameObject.FindGameObjectWithTag("Gun");
        // �� ��Ȱ��ȭ
        Gun.SetActive(false);
        // Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // �÷��� ��ư
    public void StartButton()
    {
        // ���۽� Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // ���� ���� �ʱ�ȭ
        nowScore = 0;
        // ���� ���� �ؽ�Ʈ �ʱ�ȭ
        scoreText.text = nowScore.ToString();

        // ���θ޴� �������̽� ��Ȱ��ȭ
        TitleUI.SetActive(false);
        // �ΰ��� �������̽� Ȱ��ȭ
        InGameUI.SetActive(true);
        // �� ������Ʈ Ȱ��ȭ
        Gun.SetActive(true);
        // ���� ��ȯ �ڷ�ƾ ����
        EnemyManager.Instance.StartInvoke();
        // ����� �÷���
        BGM.Play();
        // ���� �÷��� ������ ����
        isPlaying = true;
        // ü�� �ʱ�ȭ
        health = 3;

        // ü�� �̹��� ��������Ʈ �ʱ�ȭ
        foreach (var obj in hearts)
        {
            obj.sprite = heartSprite[0];
        }
    }

    // ���� ��ư Ŭ��
    public void ExitButton()
    {
        Debug.Log("Quit Game");
        // �� ����
        Application.Quit();
    }

    // BGM ���� ����
    public void BGMVolumeChange()
    {
        BGM.volume = BGMBar.value;
    }

    // ���� ȹ�� �޼���
    public void ScoreChange(int score)
    {
        nowScore += score;
        StartCoroutine(UpScoreRotine(nowScore, nowScore - score));
    }

    // ���� �ö󰡴� �ڷ�ƾ
    IEnumerator UpScoreRotine(int target, int cur)
    {
        while (cur < target)
        {
            cur += 1;
            scoreText.text = GetThousandCommaText(cur);
            yield return null;
        }

        cur = target;
        scoreText.text = GetThousandCommaText(cur);
    }

    // õ ������ �޸� ���
    private string GetThousandCommaText(int value)
    {
        return string.Format("{0:#,###}", value);
    }

    // ������ ü�� ����
    public void PlayerHit()
    {
        // ��Ʈ �̹��� ��������Ʈ ����
        hearts[health - 1].sprite = heartSprite[1];
        health--;

        // ü���� 0���Ϸ� ��������
        if (health <= 0)
        {
            // Dead �޼��� ����
            Dead();
        }
    }

    // ���� ����
    private void Dead()
    {
        // Ŀ�� Ȱ��ȭ
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // isDead ������ ����
        isDead = true;
        // ���� ���� �������̽� Ȱ��ȭ
        gameOverUI.SetActive(true);
        // ���� ���� �������̽� ���̵��� ȿ�� �ڷ�ƾ
        StartCoroutine(GameOver.Instance.GameOverRoutine());
        // ����� ����
        BGM.Stop();
        // ���� ������ �ְ� ���� ���� ���� ����
        PlayerPrefs.SetInt("MaxScore", Mathf.Max(nowScore, PlayerPrefs.GetInt("MaxScore")));
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(0);
    }
}
