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
        // 마우스 포인터 위치에 늑대 소환
        Vector3 spawnPosition = CalculateSpawnPosition(targetPosition);
        GameObject wolf = Instantiate(wolfPrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveAndDestroyWolf(wolf, targetPosition));
    }

    private Vector3 CalculateSpawnPosition(Vector3 targetPosition)
    {
        // 스킬 사용 위치 계산 (마우스 포인터 주변에 소환)
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
        // 늑대 이동 및 일정 시간 후에 사라지도록 설정
        float startTime = Time.time;
        while (Vector3.Distance(wolf.transform.position, targetPosition) > 0.1f)
        {
            wolf.transform.position = Vector3.MoveTowards(wolf.transform.position, targetPosition, wolfSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(wolf);

        // 스킬 지속 시간이 지난 후에 늑대를 파괴합니다.
        yield return new WaitForSeconds(duration);
        Destroy(wolf);
    }
}
