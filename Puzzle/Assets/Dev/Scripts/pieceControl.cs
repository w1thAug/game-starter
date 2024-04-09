using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class pieceControl : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler {
    public static Vector2 defaultposition;

    private stageControl stagecontrol;
    string pieceName;
    int pieceNum;

    bool click = false;

    Vector2 piecePosition;
    Vector2 pieceEndPosition; //����ڰ� ������ ���� ���� ��ġ��

    private void Start() {
        stagecontrol = FindObjectOfType<stageControl>();
    }

    void Update() {
        if (click) {
            var screenPoint = (Vector3)Input.mousePosition;
            screenPoint.z = 20.0f;
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        stagecontrol.changeIsClick(true);
        click = true;

        defaultposition = this.transform.position;
        pieceName = this.name;
        pieceNum = int.Parse(pieceName.Substring(pieceName.Length - 1, 1)); //������ ������ ��ȣ
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 currentPos = Input.mousePosition;
        this.transform.position = currentPos;
    }

    public void OnEndDrag(PointerEventData eventData) {
        stagecontrol.changeIsClick(false);
        click = false;

        pieceEndPosition = this.transform.position; //���� ��ġ
        piecePosition = stagecontrol.getGame().transform.GetChild(pieceNum - 1).position; //���� ���� ��ġ

        double distance = Math.Sqrt((Math.Pow(pieceEndPosition.x - piecePosition.x, 2) + Math.Pow(pieceEndPosition.y - piecePosition.y, 2)));

        if (distance < 0.5) {
            this.transform.position = piecePosition;
            stagecontrol.setStagePiece();
        } else {
            this.transform.position = defaultposition;
        }

    }


}
