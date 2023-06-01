using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public int score;

    // 각 컴포넌트 저장 변수
    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;

    private void Start()
    {
        // 각 컴포넌트 획득
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // agent가 null이 아닐때
        if (agent != null)
            // agent 컴포넌트 활성화
            agent.enabled = true;

        // 콜라이더 활성화
        GetComponent<SphereCollider>().enabled = true;
    }

    private void Update()
    {
        // agent 컴포넌트가 활성활 상태일때
        if (agent.enabled)
            // agent 목표 지점 설정
            agent.SetDestination(target.transform.position);

        // 게임 종료시
        if (agent.enabled && GameManager.Instance.isDead)
        {
            // 에이전트 컴포넌트 비활성화
            agent.enabled = false;
            // 애니메이션 컴포넌트 비활성화
            animator.enabled = false;
        }
    }

    /// <summary>
    /// 적이 맞았을때
    /// </summary>
    public void Hit()
    {
        // 콜라이더 비활성화
        GetComponent<SphereCollider>().enabled = false;
        // agent 컴포넌트 비활성화
        agent.enabled = false;
        // 죽는 애니메이션 실행
        animator.SetTrigger("Dead");
        // 일정시간후 삭제
        StartCoroutine(ReturnEnemy());
        GameManager.Instance.ScoreChange(score);
    }

    IEnumerator ReturnEnemy()
    {
        yield return new WaitForSeconds(3f);
        // 매니저에있는 풀에 오브젝트 반환
        EnemyManager.Instance.ReturnEnemy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌시
        if (other.tag == "Player")
        {
            // 플레이어 히트 메서드 호출
            GameManager.Instance.PlayerHit();
        }
    }
}
