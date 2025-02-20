using System.Collections;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public enum BuffType { FireRate, Health, NotDecided }
    public BuffType buffType = BuffType.NotDecided;

    private Transform player;
    private PlayerController playerScript;
    private float moveSpeed = 5f;
    private float delayBeforeChase = 0.5f;
    private DropItemPool dropItemPool;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        playerScript = FindObjectOfType<PlayerController>();
        if (buffType != BuffType.NotDecided) Invoke(nameof(StartChasing), delayBeforeChase);
        dropItemPool = FindObjectOfType<DropItemPool>();
    }

    private void StartChasing()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                ApplyBuff();
                ResetItem();
                break;
            }
            yield return null;
        }
    }

    private void ApplyBuff()
    {
        if (playerScript != null)
        {
            switch (buffType)
            {
                case BuffType.FireRate:
                    playerScript.ApplyFireRateBuff(2, 10);
                    break;
                case BuffType.Health:
                    playerScript.RecoverHealth(10);
                    break;
            }
        }
    }

    private void ResetItem()
    {
        dropItemPool.ReturnToPool(gameObject);
    }
}
