using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private float soundCheckInterval = 0.1f;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        StartCoroutine(PlayMovingSound());
    }

    private IEnumerator PlayMovingSound()
    {
        while (true)
        {
            if (player.IsWalking)
            {
                if (player.IsSprinting)
                {
                    SFXManager.Instance.PlaySprintingSound(player.transform.position, 1f);
                }
                else
                {
                    SFXManager.Instance.PlayWalkingSound(player.transform.position, 1f);
                }
            }
            yield return new WaitForSeconds(soundCheckInterval);
        }
    }
}
