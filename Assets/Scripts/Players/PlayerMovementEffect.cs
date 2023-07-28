using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementEffect : MonoBehaviour
{
    [SerializeField] private GameObject particalObject;

    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        particalObject.SetActive(false);
    }

    private void Update()
    {
        particalObject.SetActive(player.IsSprinting);
    }
}
