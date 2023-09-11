using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSummonSkill : MonoBehaviour, ISkillModule
{
    public float summonRange = 10f;
    public float wolfSpeed = 5f;
    public float duration = 5f;

    public void ActivateSkill(Vector3 targetPosition, GameObject wolfPrefab)
    {
        // ���콺 ������ ��ġ�� ���� ��ȯ
        Vector3 spawnPosition = CalculateSpawnPosition(targetPosition);
        GameObject wolf = Instantiate(wolfPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveAndDestroyWolf(wolf, targetPosition));
    }

    private Vector3 CalculateSpawnPosition(Vector3 targetPosition)
    {
        // ��ų ��� ��ġ ��� (���콺 ������ �ֺ��� ��ȯ)
        Ray ray = Camera.main.ScreenPointToRay(targetPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, summonRange))
        {
            return hit.point;
        }

        return ray.GetPoint(summonRange);
    }

    private IEnumerator MoveAndDestroyWolf(GameObject wolf, Vector3 targetPosition)
    {
        // ���� �̵� �� ���� �ð� �Ŀ� ��������� ����
        float startTime = Time.time;
        while (Vector3.Distance(wolf.transform.position, targetPosition) > 0.1f)
        {
            wolf.transform.position = Vector3.MoveTowards(wolf.transform.position, targetPosition, wolfSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(wolf);

        // ��ų ���� �ð��� ���� �Ŀ� ���븦 �ı��մϴ�.
        yield return new WaitForSeconds(duration);
        Destroy(wolf);
    }
}
