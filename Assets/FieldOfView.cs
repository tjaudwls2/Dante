using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    // �þ� ������ �������� �þ� ����
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    // ����ũ 2��
    public LayerMask targetMask, obstacleMask;

    // Target mask�� ray hit�� transform�� �����ϴ� ����Ʈ
    public List<Transform> visibleTargets = new List<Transform>();

    void Start()
    {
        // 0.2�� �������� �ڷ�ƾ ȣ��
       // StartCoroutine(FindTargetsWithDelay(0.2f));
    }

  
    public void FindVisibleTargets()
    {
        visibleTargets.Clear();
        // viewRadius�� ���������� �� �� ���� �� targetMask ���̾��� �ݶ��̴��� ��� ������
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            // �÷��̾�� forward�� target�� �̷�� ���� ������ ���� �����
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                
                    if(target.GetComponent<monster_one>() != null)
                    visibleTargets.Add(target);
                
            }
        }
    }

  
}