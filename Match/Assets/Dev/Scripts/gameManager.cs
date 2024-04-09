using UnityEngine;

public class gameManager : MonoBehaviour {
    int stageNum = 1; //�ܰ�, �ܰ躰 ī���� ���� n*2 + 2
    public int selectCardCnt = 0;

    //ī��
    int[] allCard = new int[100]; //�ش� �ܰ��� ��� ī�� ��ȣ�� ����
    //int correctNum = 0; //���� ī����� ��, ��ü ī����� ���� �Ǹ� �ش� �ܰ� ���
    public Sprite[] changeImgArray; //ī�� ���� �̹�����

    public Material cardBack;
    public Material cardFront;
    public GameObject stage;
    public GameObject particle;
    public GameObject scoreCanvas;
    Animator animator;

    int openedCard = 0;

    public countControl cardcontrol; //countControll ��ũ��Ʈ���� openCNT ���� �ҷ����� ����
    public cardRotate cardrotate;
    private gameManager gamecontrol;


    private readonly string canvasString = "Canvas";


    void shuffleCard() {
        //n���� ī�� �� n/2���� ��ȣ�� �ο�
        //���Ŀ� ī�� ��ȣ�� ��� ���ؼ� n�� �ݺ�
        for (int i = 0; i < (stageNum * 2 + 2); i++) {
            allCard[i] = i;
        }

        //0���� n-1���� �� ���� ���� �� ��(r1, r2)�� ��� allCard�� r1��°�� r2��°�� ���� �ٲپ���, n/2�� �ݺ�.
        for (int j = 0; j < ((stageNum * 2 + 2) / 2); j++) {
            int random1 = Random.Range(0, stageNum * 2 + 2);
            int random2 = Random.Range(0, stageNum * 2 + 2);

            int tmp = allCard[random1];
            allCard[random1] = allCard[random2];
            allCard[random2] = tmp;
        }

        setCard();
    }

    //ī�� �� ���� �����ִ� ���� �ڵ�
    public void playCard() {
        shuffleCard(); //allCard�迭�� ������ �������� 1~n���� �������

        //���� ��ư �� panel ����
        //UI ��ũ��Ʈ ���� ����

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

    //ī�� ���� ����
    public void setCard() {
        //n��ŭ �ݺ�
        //ī��� �Ȱ��� �̹����� �ѽ־� �����ϹǷ� n/2���� �̹����� ����-> ���� �迭�� �� �� ���ڸ� ����� ��
        //ī�� �̹��� ���� �ڵ�
        //��) 1�ܰ�� 4��, ���� �ݺ����� 2�� �ݺ�

        for (int k = 0; k < (stageNum * 2 + 2); k++) 
        {
            GameObject changedCard = GameObject.Find("card" + allCard[k].ToString()); //card3,1,0,2������� changedCard�� �Ҵ�
            SpriteRenderer sr = changedCard.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>(); //changedCard�� sprite�� �����Ͽ� 

            sr.sprite = changeImgArray[allCard[k / 2]]; //changeImgArray�� 3,1��° �̹����� ���� ������� (3,3,1,1)
                                                        // "/" ������ ������ �����ϰ� �� ����
                                                        // ���� K�� 0�϶�, 1�϶��� 0, 2�϶�, 3�϶��� 1

            Debug.Log(allCard[k]);//1�ܰ� : 4���� ���� ���� ��� (�� 3102)
        }
    }

    //�� �� ���� ī�尡 ��ġ�ϴ��� Ȯ��
    public void checkSelectCard() {
        int openCount = cardcontrol.getOpenCount();

        if (selectCardCnt < ((stageNum * 2) + 2)){
            if (openCount.Equals(2)) {
                GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);

                //������ �� ī���� �̸� ���� �غ�
                string name01 = cardcontrol.getCardName(0);
                string name02 = cardcontrol.getCardName(1);

                //�� ī�� ���� ���� ���
                if (cardcontrol.getCardInfo(0).Equals(cardcontrol.getCardInfo(1))) {
                    Invoke("equalCard", 1f);
                    Invoke("removeArray", 1f);
                    Invoke("updateGame", 2f);
                    Invoke("returnClick", 2f);
                }

                //�� ī�� ���� �ٸ� ���
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

    //���� ���������� �Ѿ�� ���� �������ִ� �Լ�
    void updateGame() {
        selectCardCnt += 2; //���� �����ִ� ī���� ��

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
