using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void GameStart()
    {
        Debug.Log("게임 시작");
    }

    ///<Summary>
    /// 저장된 데이터가 있는지 확인하고 해당 데이터가 존재한다면 가져옵니다.
    ///</Summary>
    public void LoadData()
    {
        Debug.Log("저장된 데이터를 불러옵니다.");
    }

    public void EndGame()
    {

    }
}
