using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextChange : MonoBehaviour
{
    // TextMesh 변수 선언
    public TextMesh tm;
    public GameObject buttonObject;

    // 시작 함수
    void Start()
    {
        // TextMesh 컴포넌트 가져오기
        tm = GetComponent<TextMesh>();
    }

    // 매 프레임마다 실행되는 함수
    void Update()
    {
        /*
        if (GameManager.red == 0 && GameManager.blue == 0) tm.text = "Draw!!!";
        else if (GameManager.red == 0) tm.text = "Blue team won!";
        else if (GameManager.blue == 0) tm.text = "Red team won!";
        */
        // 빨간팀 또는 파란팀이 모두 파괴된 경우
        if (GameManager.red == 0 || GameManager.blue == 0) StartCoroutine("Winner");
        else
        {
            // 빨간팀 차례일 경우
            if (GameManager.turn % 2 == 1)
            {
                // TextMesh 내용 변경
                tm.text = "Turn: " + GameManager.turn + "\n" + GameManager.red + " : " + GameManager.blue + "\nBlack player's turn";
                // TextMesh 위치, 회전 변경
                transform.position = new Vector3(0, -10, 60);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            // 파란팀 차례일 경우
            else
            {
                // TextMesh 내용 변경
                tm.text = "Turn: " + GameManager.turn + "\n" + GameManager.red + " : " + GameManager.blue + "\nWhite player's turn";
                // TextMesh 위치, 회전 변경
                transform.position = new Vector3(0, -10, -60);
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }
        }
    }

    // 우승자 결정 및 표시하는 함수
    IEnumerator Winner()
    {
        
        // Shot 방지
        GameManager.allowShoot = false;
        yield return new WaitForSeconds(3.0f);
        if (GameManager.red == 0 && GameManager.blue == 0) tm.text = "Draw!";
        else if (GameManager.blue == 0)
        {
            tm.text = "Black team won!";
            buttonObject.SetActive(true);
            if (GameManager.turn % 2 == 0) GameObject.Find("Main Camera").GetComponent<GameManager>().player2Turn();
        }
        else if (GameManager.red == 0)
        {
            tm.text = "White team won!";
            buttonObject.SetActive(true);
            if (GameManager.turn % 2 == 1) GameObject.Find("Main Camera").GetComponent<GameManager>().player2Turn();
        }
        else Debug.Log("Error! Can't find winner.");
    }

}