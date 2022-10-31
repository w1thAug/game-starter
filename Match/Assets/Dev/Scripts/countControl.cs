using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countControl : MonoBehaviour
{
    public int openCount = 0;

    //뒤집힌 두 카드 이미지값 저장
    public ArrayList cardInfo = new ArrayList();

    //뒤집힌 두 카드 네임값 저장
    public ArrayList cardName = new ArrayList();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //열린 카드 증가할 떄마다 +1
    public void openCountUp()
    {
        if(openCount == 2)
        {
            openCount = 1; //0에서 2까지 센 다음 누르는 카드는 1번째
        }
        else
        {
            openCount++;
        }
    }

    //열려있는 카드 수 반환
    public int getOpenCount()
    {
        return openCount;
    }

    //선택한 카드 정보 저장
    public void insertCardInfo(string info) {
        cardInfo.Add(info);
    }

    public string getCardInfo(int index) {
        return (string)cardInfo[index];
    }   

    public void removeCardInfo() {
        cardInfo.Clear();
    }
    
    //선택한 카드 이름 저장
    public void insertCardName(string info) {
        cardName.Add(info);
    }

    public string getCardName(int index) {
        return (string)cardName[index];
    }

    public void removeCardName() {
        cardName.Clear();
    }
}

