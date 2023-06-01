using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public int score;

    // �� ������Ʈ ���� ����
    private NavMeshAgent agent;
    private GameObject target;
    private Animator animator;

    private void Start()
    {
        // �� ������Ʈ ȹ��
        target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // agent�� null�� �ƴҶ�
        if (agent != null)
            // agent ������Ʈ Ȱ��ȭ
            agent.enabled = true;

        // �ݶ��̴� Ȱ��ȭ
        GetComponent<SphereCollider>().enabled = true;
    }

    private void Update()
    {
        // agent ������Ʈ�� Ȱ��Ȱ �����϶�
        if (agent.enabled)
            // agent ��ǥ ���� ����
            agent.SetDestination(target.transform.position);

        // ���� �����
        if (agent.enabled && GameManager.Instance.isDead)
        {
            // ������Ʈ ������Ʈ ��Ȱ��ȭ
            agent.enabled = false;
            // �ִϸ��̼� ������Ʈ ��Ȱ��ȭ
            animator.enabled = false;
        }
    }

    /// <summary>
    /// ���� �¾�����
    /// </summary>
    public void Hit()
    {
        // �ݶ��̴� ��Ȱ��ȭ
        GetComponent<SphereCollider>().enabled = false;
        // agent ������Ʈ ��Ȱ��ȭ
        agent.enabled = false;
        // �״� �ִϸ��̼� ����
        animator.SetTrigger("Dead");
        // �����ð��� ����
        StartCoroutine(ReturnEnemy());
        GameManager.Instance.ScoreChange(score);
    }

    IEnumerator ReturnEnemy()
    {
        yield return new WaitForSeconds(3f);
        // �Ŵ������ִ� Ǯ�� ������Ʈ ��ȯ
        EnemyManager.Instance.ReturnEnemy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹��
        if (other.tag == "Player")
        {
            // �÷��̾� ��Ʈ �޼��� ȣ��
            GameManager.Instance.PlayerHit();
        }
    }
}
