using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    protected int mount;
    public override void Init()
    {
        base.Init();
    }

    public override void Interacted()
    {
        base.Interacted();
        //TODO 플레이어에게 재화 추가
    }
}
