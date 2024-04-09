using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stageControl : MonoBehaviour {

    /*
     * *
        Ready : 준비 단계, 조각 셔플
        Start : 게임이 시작될 때
        Drag : 사용자가 조각을 잡을 때부터 이동시킬 때까지
        Drop : 사용자가 조각을 놓을 때
     * *
     */

    public enum State { Ready, Start, Drag, Drop, None }

    private State state = State.None;

    private Coroutine runCoroutine = null;
    public int stagePiece = 0;
    public int stageNum = 1; //현재 단계

    private bool isClick = false;

    public void Transition(State nextState) {
        transition(nextState);
    }

    private void transition(State nextState) {
        if (runCoroutine != null)
            StopCoroutine(runCoroutine);

        if (state != nextState) {
            state = nextState;
            runCoroutine = state switch {
                State.Ready => StartCoroutine(STReady()),
                State.Start => StartCoroutine(STStart()),
                State.Drag => StartCoroutine(STDrag()),
                State.Drop => StartCoroutine(STDrop()),
                _ => null,
            };
        }
    }
    private void Start() {
        updateStage();
        Transition(State.Ready);
    }


    IEnumerator STReady() {
        Debug.Log("Ready!");
        updateStage();
        if (stageNum < 4) {
            stageGameList[stageNum - 1].SetActive(true);
            randomPiece();
            yield return null;

            Transition(State.Start);
            yield return null;
        } else {
            ending.SetActive(true);
            stageText.text = "";
            puzzleSide.SetActive(false);
            yield return null;
        }
    }

    IEnumerator STStart() {
        Debug.Log("Start!");

        while (!isClick) {
            yield return null;
        }

        if (isClick) {
            Transition(State.Drag);
            yield return null;
        }

        yield return null;
    }

    IEnumerator STDrag() {
        Debug.Log("Drag!");

        while (isClick) {
            yield return null;
        }

        if (!isClick) {
            Transition(State.Drop);
            yield return null;
        }

        yield return null;
    }

    IEnumerator STDrop() {
        Debug.Log("Drop!");

        if (stagePiece.Equals(7)) {
            stagePieceList.Clear();
            stagePiece = 0;
            stageNum++;
            yield return new WaitForSeconds(1f);

            stageGameList[stageNum - 2].SetActive(false);
            Transition(State.Ready);
            yield return null;
        } else {
            Transition(State.Start);
            yield return null;
        }

        yield return null;
    }

    [SerializeField] private Text stageText = null;
    [SerializeField] private GameObject ending;
    [SerializeField] private GameObject particle;
    [SerializeField] private GameObject puzzleSide;

    [Space()]
    [Space()]

    [SerializeField] private List<GameObject> stageGameList = new List<GameObject>(); //Stage 내용
    [SerializeField] private List<GameObject> puzzleList = new List<GameObject>(); //퍼즐 조각 있는 그룹
    [SerializeField] private List<GameObject> stagePieceList = new List<GameObject>(); //Stage의 퍼즐조각들
    [SerializeField] private List<GameObject> gameList = new List<GameObject>(); //퍼즐 위치


    private void updateStage() {
        stageText.text = string.Format("{0} / 3", stageNum);
    }

    public void setStagePiece() {
        stagePiece += 1;
    }

    public int getStagePiece() {
        return stagePiece;
    }

    public GameObject getGame() {
        return gameList[stageNum - 1];
    }

    public void changeIsClick(bool click) {
        isClick = click;
    }

    void randomPiece() {
        int rnd = Random.Range(0, 3);

        int[] rndNum = new int[10];
        puzzleList[stageNum - 1].transform.GetChild(rnd).gameObject.SetActive(true);

        for (int i = 0; i < 10; i++) {
            rndNum[i] = i;
        }

        for (int j = 0; j < 5; j++) {
            int rnd2 = Random.Range(0, 9);
            int tmp = rndNum[j];
            rndNum[j] = rndNum[rnd2];
            rndNum[rnd2] = tmp;
        }

        Color[] colors = new Color[] { new Color32(255, 207, 204, 255), new Color32(255, 224, 204, 255), new Color32(255, 247, 204, 255), new Color32(215, 255, 204, 255), new Color32(204, 252, 255, 255), new Color32(204, 220, 255, 255), new Color32(222, 204, 255, 255), new Color32(255, 207, 204, 255), new Color32(255, 230, 253, 255), new Color32(230, 255, 245, 255) };

        for (int k = 0; k < 7; k++) {
            Image pieceColor = puzzleList[stageNum - 1].transform.GetChild(rnd).GetChild(k).GetComponent<Image>();
            pieceColor.color = colors[rndNum[k]];
        }
    }
}
