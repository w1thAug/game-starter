using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setStageNum : MonoBehaviour
{

    private gameManager gamecontrol;
    public Text stageNum;

    // ���� ȭ�� ��ܿ� �ܰ� ǥ��
    void Start()
    {
        gamecontrol = FindObjectOfType<gameManager>();
        stageNum.text = gamecontrol.getStageNum().ToString();
    }

    public void setText(int text) {
        stageNum.text = text.ToString();
    }
}
