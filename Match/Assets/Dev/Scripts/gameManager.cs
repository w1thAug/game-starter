using UnityEngine;

public class gameManager : MonoBehaviour {
    int stageNum = 1; //단계, 단계별 카드의 수는 n*2 + 2
    public int selectCardCnt = 0;

    //카드
    int[] allCard = new int[100]; //해당 단계의 모든 카드 번호를 저장
    //int correctNum = 0; //맞춘 카드쌍의 수, 전체 카드수의 반이 되면 해당 단계 통과
    public Sprite[] changeImgArray; //카드 랜덤 이미지들

    public Material cardBack;
    public Material cardFront;
    public GameObject stage;
    public GameObject particle;
    public GameObject scoreCanvas;
    Animator animator;

    int openedCard = 0;

    public countControl cardcontrol; //countControll 스크립트에서 openCNT 변수 불러오기 위함
    public cardRotate cardrotate;
    private gameManager gamecontrol;


    private readonly string canvasString = "Canvas";


    void shuffleCard() {
        //n개의 카드 중 n/2개의 번호만 부여
        //이후에 카드 번호를 얻기 위해서 n번 반복
        for (int i = 0; i < (stageNum * 2 + 2); i++) {
            allCard[i] = i;
        }

        //0부터 n-1까지 중 랜덤 숫자 두 개(r1, r2)를 골라 allCard의 r1번째와 r2번째를 서로 바꾸어줌, n/2번 반복.
        for (int j = 0; j < ((stageNum * 2 + 2) / 2); j++) {
            int random1 = Random.Range(0, stageNum * 2 + 2);
            int random2 = Random.Range(0, stageNum * 2 + 2);

            int tmp = allCard[random1];
            allCard[random1] = allCard[random2];
            allCard[random2] = tmp;
        }

        setCard();
    }

    //카드 한 번씩 보여주는 시작 코드
    public void playCard() {
        shuffleCard(); //allCard배열에 순서가 랜덤으로 1~n까지 들어있음

        //시작 버튼 및 panel 제거
        //UI 스크립트 따로 관리

        GameObject tmp1 = GameObject.Find("Canvas").transform.GetChild(0).gameObject;
        GameObject tmp2 = GameObject.Find("Canvas").transform.GetChild(1).gameObject;

        if (tmp1.activeSelf == true && tmp2.activeSelf == true) {
            tmp1.SetActive(false);
            tmp2.SetActive(false);
        }

        Debug.Log("game stage : " + stageNum);

        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
        showCard();
        Invoke("hideCard", 2f);
        Invoke("returnClick", 3f);
    }


    void returnClick() {
        GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(false);
    }

    void showCard() {
        for (int i = 0; i < (stageNum * 2 + 2); i++) {
            GameObject cardtmp = stage.transform.GetChild(stageNum - 1).GetChild(i).gameObject;
            animator = cardtmp.GetComponent<Animator>();
            animator.SetBool("isOpen", true);
            openedCard++;
        }
    }

    void hideCard() {
        if (openedCard.Equals((stageNum * 2 + 2))) {
            for (int i = 0; i < (stageNum * 2 + 2); i++) {
                GameObject cardtmp2 = stage.transform.GetChild(stageNum - 1).GetChild(i).gameObject;
                animator = cardtmp2.GetComponent<Animator>();
                animator.SetBool("isOpen", false);
            }

            openedCard = 0;
        }
    }

    //카드 덱에 놓기
    public void setCard() {
        //n만큼 반복
        //카드는 똑같은 이미지가 한쌍씩 존재하므로 n/2장의 이미지만 존재-> 따라서 배열의 앞 두 숫자만 사용할 것
        //카드 이미지 변경 코드
        //예) 1단계는 4장, 따라서 반복문은 2번 반복

        for (int k = 0; k < (stageNum * 2 + 2); k++) 
        {
            GameObject changedCard = GameObject.Find("card" + allCard[k].ToString()); //card3,1,0,2순서대로 changedCard에 할당
            SpriteRenderer sr = changedCard.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>(); //changedCard의 sprite에 접근하여 

            sr.sprite = changeImgArray[allCard[k / 2]]; //changeImgArray의 3,1번째 이미지를 각각 집어넣음 (3,3,1,1)
                                                        // "/" 연산은 나머지 제거하고 몫만 나옴
                                                        // 따라서 K가 0일때, 1일때는 0, 2일때, 3일때는 1

            Debug.Log(allCard[k]);//1단계 : 4개의 숫자 랜덤 출력 (예 3102)
        }
    }

    //고른 두 장의 카드가 일치하는지 확인
    public void checkSelectCard() {
        int openCount = cardcontrol.getOpenCount();

        if (selectCardCnt < ((stageNum * 2) + 2)){
            if (openCount.Equals(2)) {
                GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);

                //뒤집힌 두 카드의 이름 정보 준비
                string name01 = cardcontrol.getCardName(0);
                string name02 = cardcontrol.getCardName(1);

                //두 카드 값이 같을 경우
                if (cardcontrol.getCardInfo(0).Equals(cardcontrol.getCardInfo(1))) {
                    Invoke("equalCard", 1f);
                    Invoke("removeArray", 1f);
                    Invoke("updateGame", 2f);
                    Invoke("returnClick", 2f);
                }

                //두 카드 값이 다를 경우
                else {
                    Invoke("wrongCard", 1f);
                    Invoke("cardClose", 2f);
                    Invoke("removeWrongSign", 2f);
                    Invoke("removeArray", 2f);
                    Invoke("returnClick", 2f);
                }
            }
        } 
    }

    //다음 스테이지로 넘어가는 것을 결정해주는 함수
    void updateGame() {
        selectCardCnt += 2; //현재 열려있는 카드의 수

        if (selectCardCnt == ((stageNum * 2) + 2)) {
            if(stageNum < 3) {
                selectCardCnt = 0;
                checkStageMoving();
            }
            else if (stageNum.Equals(3)) {
                scoreCanvas.transform.GetChild(3).gameObject.SetActive(true);
                particle.transform.GetChild(0).gameObject.SetActive(true);
                particle.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    void checkStageMoving() {
        scoreCanvas.transform.Find("stageUpTxt").gameObject.SetActive(true);
    }

    void wrongCard() {
        string name01 = cardcontrol.getCardName(0);
        string name02 = cardcontrol.getCardName(1);

        GameObject.Find(name01).transform.GetChild(2).gameObject.SetActive(true);
        GameObject.Find(name02).transform.GetChild(2).gameObject.SetActive(true);
    }

    void equalCard() {
        string name01 = cardcontrol.getCardName(0);
        string name02 = cardcontrol.getCardName(1);

        GameObject.Find(name01).transform.GetChild(1).gameObject.SetActive(true);
        GameObject.Find(name02).transform.GetChild(1).gameObject.SetActive(true);
    }

    void removeWrongSign() {
        string name01 = cardcontrol.getCardName(0);
        string name02 = cardcontrol.getCardName(1);


        GameObject.Find(name01).transform.GetChild(2).gameObject.SetActive(false);
        GameObject.Find(name02).transform.GetChild(2).gameObject.SetActive(false);
    }

    public void cardClose() {
        string name01 = cardcontrol.getCardName(0);
        string name02 = cardcontrol.getCardName(1);

        GameObject.Find(name01).GetComponent<Animator>().SetBool("isOpen", false);
        GameObject.Find(name02).GetComponent<Animator>().SetBool("isOpen", false);

    }

    void removeArray() {
        cardcontrol.removeCardInfo();
        cardcontrol.removeCardName();
    }

    public int getStageNum() {
        return stageNum;
    }

    public void setStageNum(int num) {
        stageNum = num;
    }
}
