using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum itemType
{
    gold,
    powerUp,
    speedUp,
    Hp,
    energy,
}


public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    public itemType type;

    public virtual void Init()
    {
        Debug.Log("아이템의 상태를 초기화 하였습니다.");
    }

    public virtual void Interacted()
    {
        Debug.Log("아이템을 획득하였습니다.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interacted();
        }
    }
}
