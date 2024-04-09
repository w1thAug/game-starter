using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countControl : MonoBehaviour
{
    public int openCount = 0;

    //������ �� ī�� �̹����� ����
    public ArrayList cardInfo = new ArrayList();

    //������ �� ī�� ���Ӱ� ����
    public ArrayList cardName = new ArrayList();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //���� ī�� ������ ������ +1
    public void openCountUp()
    {
        if(openCount == 2)
        {
            openCount = 1; //0���� 2���� �� ���� ������ ī��� 1��°
        }
        else
        {
            openCount++;
        }
    }

    //�����ִ� ī�� �� ��ȯ
    public int getOpenCount()
    {
        return openCount;
    }

    //������ ī�� ���� ����
    public void insertCardInfo(string info) {
        cardInfo.Add(info);
    }

    public string getCardInfo(int index) {
        return (string)cardInfo[index];
    }   

    public void removeCardInfo() {
        cardInfo.Clear();
    }
    
    //������ ī�� �̸� ����
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

