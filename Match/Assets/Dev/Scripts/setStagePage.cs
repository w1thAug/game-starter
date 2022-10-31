using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setStagePage : MonoBehaviour
{
    private gameManager gamecontrol;
    public GameObject scoreCanvas;
    public GameObject stage;

    public setStageNum stageNumFun;

    // 스테이지 종료시 다음 스테이지로 이동

    public void goGame() {
        gamecontrol = FindObjectOfType<gameManager>();
        int stageNum = gamecontrol.getStageNum();

        stage.transform.GetChild(stageNum - 1).gameObject.SetActive(false);
        stage.transform.GetChild(stageNum).gameObject.SetActive(true);
        stageNum = stageNum + 1;
        Debug.Log("set Stage : " + stageNum );

        stageNumFun.setText(stageNum);
        gamecontrol.setStageNum(stageNum);

        scoreCanvas.transform.GetChild(2).gameObject.SetActive(false);
        gamecontrol.playCard();
    }

    public void stopGame() {
        scoreCanvas.SetActive(false);
        ExitGame();
    }

    public void ExitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
