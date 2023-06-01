using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 현재 플레이중인지 확인
    public bool isPlaying;
    // 배경음 오디오소스
    [SerializeField]
    private AudioSource BGM;
    // 총 오브젝트
    [SerializeField]
    private GameObject Gun;
    // 인게임 인터페이스
    [SerializeField]
    private GameObject InGameUI;
    // 메인메뉴 인터페이스
    [SerializeField]
    private GameObject TitleUI;
    // 설정메뉴 인터페이스
    [SerializeField]
    private GameObject SettingUI;
    // 현재 점수 텍스트
    [SerializeField]
    public Text scoreText;
    // 현재 점수 변수
    private int nowScore;
    // 하트 이미지
    [SerializeField]
    private Image[] hearts;
    // 하트 스프라이트
    [SerializeField]
    private Sprite[] heartSprite;
    // 게임오버 인터페이스
    [SerializeField]
    private GameObject gameOverUI;
    // 최고 점수 텍스트
    [SerializeField]
    private Text maxScoreText;
    // 현재 체력
    private int health;
    // 죽었는지 확인
    public bool isDead;
    // 배경음 볼륨 설정 바
    public Slider BGMBar;
    // 효과음 볼륨 설정 바
    public Slider SFXBar;

    private void Awake()
    {
        // 싱글톤 생성
        Instance = this;

        // 배경음 볼륨 확인
        if (!PlayerPrefs.HasKey("BGMVolume"))
            PlayerPrefs.SetFloat("BGMVolume", 1);
        // 효과음 볼륨 확인
        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", 1);
        // 최고 점수 확인
        if (!PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);

        // 배경음 바 설정
        BGMBar.value = PlayerPrefs.GetFloat("BGMVolume");
        // 배경음 오디오 볼륨 설정
        BGM.volume = PlayerPrefs.GetFloat("BGMVolume");
        // 효과음 바 설정
        SFXBar.value = PlayerPrefs.GetFloat("SFXVolume");
        // 효과음 오디오 볼륨 설정
        SFXManager.Instance.SFXVolumeChange();
        // 최고 점수 텍스트 설정
        if (PlayerPrefs.GetInt("MaxScore") < 1000)
            maxScoreText.text = "Max Score : " + PlayerPrefs.GetInt("MaxScore").ToString();
        else
            maxScoreText.text = "Max Score : " + GetThousandCommaText(PlayerPrefs.GetInt("MaxScore")).ToString();
    }

    private void Start()
    {
        // 총 오브젝트 획득
        Gun = GameObject.FindGameObjectWithTag("Gun");
        // 총 비활성화
        Gun.SetActive(false);
        // 커서 활성화
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // 플레이 버튼
    public void StartButton()
    {
        // 시작시 커서 비활성화
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 현재 점수 초기화
        nowScore = 0;
        // 현재 점수 텍스트 초기화
        scoreText.text = nowScore.ToString();

        // 메인메뉴 인터페이스 비활성화
        TitleUI.SetActive(false);
        // 인게임 인터페이스 활성화
        InGameUI.SetActive(true);
        // 총 오브젝트 활성화
        Gun.SetActive(true);
        // 몬스터 소환 코루틴 실행
        EnemyManager.Instance.StartInvoke();
        // 배경음 플레이
        BGM.Play();
        // 현재 플레이 참으로 변경
        isPlaying = true;
        // 체력 초기화
        health = 3;

        // 체력 이미지 스프라이트 초기화
        foreach (var obj in hearts)
        {
            obj.sprite = heartSprite[0];
        }
    }

    // 종료 버튼 클릭
    public void ExitButton()
    {
        Debug.Log("Quit Game");
        // 앱 종료
        Application.Quit();
    }

    // BGM 볼륨 조절
    public void BGMVolumeChange()
    {
        BGM.volume = BGMBar.value;
    }

    // 점수 획득 메서드
    public void ScoreChange(int score)
    {
        nowScore += score;
        StartCoroutine(UpScoreRotine(nowScore, nowScore - score));
    }

    // 점수 올라가는 코루틴
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

    // 천 단위로 콤마 출력
    private string GetThousandCommaText(int value)
    {
        return string.Format("{0:#,###}", value);
    }

    // 맞을시 체력 감소
    public void PlayerHit()
    {
        // 하트 이미지 스프라이트 변경
        hearts[health - 1].sprite = heartSprite[1];
        health--;

        // 체력이 0이하로 떨어지면
        if (health <= 0)
        {
            // Dead 메서드 실행
            Dead();
        }
    }

    // 게임 오버
    private void Dead()
    {
        // 커서 활성화
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // isDead 참으로 변경
        isDead = true;
        // 게임 오버 인터페이스 활성화
        gameOverUI.SetActive(true);
        // 게임 오버 인터페이스 페이드인 효과 코루틴
        StartCoroutine(GameOver.Instance.GameOverRoutine());
        // 배경음 정지
        BGM.Stop();
        // 현재 점수와 최고 점수 비교후 점수 저장
        PlayerPrefs.SetInt("MaxScore", Mathf.Max(nowScore, PlayerPrefs.GetInt("MaxScore")));
    }

    public void SceneChange()
    {
        SceneManager.LoadScene(0);
    }
}
