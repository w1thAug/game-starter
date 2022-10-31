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
    public int checkFirst = 0; //초기 open인지 판별용

    public countControl cardcontrol; //countControll 스크립트에서 openCNT 변수 불러오기 위함
    public gameManager gamecontrol; 


    //뒤집힌 두 카드 저장
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


    //카드가 열리면 앞면이 보이면서 count ++
    //열린 카드의 태그를 배열에 저장
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
    
    //카드가 닫히면 뒷면이 보임
    public void cardClose()
    {
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.material = cardBack;
        this.transform.Find("CardImage").gameObject.SetActive(false);
    }

    //현재 덱에 몇 장이 열려있는지 확인 (최대 2장)
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
