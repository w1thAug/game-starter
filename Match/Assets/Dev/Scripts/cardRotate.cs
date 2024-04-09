using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class cardRotate : MonoBehaviour
{

    public Material cardBack;
    public Material cardFront;
    Animator animator;
    public GameObject card;

    public int openCount = 0;
    public int checkFirst = 0; //�ʱ� open���� �Ǻ���

    public countControl cardcontrol; //countControll ��ũ��Ʈ���� openCNT ���� �ҷ����� ����
    public gameManager gamecontrol; 


    //������ �� ī�� ����
    public ArrayList cardInfo = new ArrayList();

    private void Awake()
    {
        animator = card.GetComponent<Animator>();
    }

    void Start()
    {
        gamecontrol = FindObjectOfType<gameManager>();
    }

    void Update()
    {
        
    }

    public void clickCard()
    {
        animator.SetBool("isOpen", true);
    }


    //ī�尡 ������ �ո��� ���̸鼭 count ++
    //���� ī���� �±׸� �迭�� ����
    public void cardOpen()
    {

        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.material = cardFront;
        this.transform.Find("CardImage").gameObject.SetActive(true);

        cardcontrol.openCountUp();
        checkFirst = checkFirst + 1;

        if (checkFirst > 1) {
            SpriteRenderer cardSR = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            string cardParam = (cardSR.sprite).ToString();
            cardcontrol.insertCardInfo(cardParam);
            cardcontrol.insertCardName(this.gameObject.name.ToString());

            /*Debug.Log("name check : " + this.gameObject.name.ToString());*/

            if ((cardcontrol.getOpenCount()).Equals(2)) {
                gamecontrol.checkSelectCard();
            }
        } 

    }
    
    //ī�尡 ������ �޸��� ����
    public void cardClose()
    {
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.material = cardBack;
        this.transform.Find("CardImage").gameObject.SetActive(false);
    }

    //���� ���� �� ���� �����ִ��� Ȯ�� (�ִ� 2��)
    public int getOpenCNT()
    {
        return openCount;
    }

    public int getCheckFirst() {
        return checkFirst;
    }

    public void setCheckFirst(int i) {
        checkFirst = i;
    }
}
