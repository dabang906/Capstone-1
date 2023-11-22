using UnityEngine;
using System.Collections;
using System.Threading;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine.TextCore.Text;
using UnityEngine.SceneManagement;

public class Al
{
    public string pos;
    public string rot;
}

// BallDrag 클래스 정의
[RequireComponent(typeof(LineRenderer))] // 이 스크립트는 LineRenderer 컴포넌트가 필요함을 나타냄
public class BallDrag : MonoBehaviourPunCallbacks
{
    // 필드 선언
    private Vector3 dragStartPos; // 드래그 시작 위치
    private Rigidbody rb; // Rigidbody 컴포넌트
    private LineRenderer lineRenderer; // LineRenderer 컴포넌트
    bool clicked = false;
    public static BallDrag control;
    public PhotonView PV;
    int player;
    private bool _isStopped = false; // 움직임 상태 플래그

    public Al al = new Al();
    public int id;
    public static int idCounter = 0;

    public string path;
    //public PhotonView PV;
    // Start 함수 정의
    void OnEnable()
    {
        id = idCounter++;
        Debug.Log(id);
    }
    void Start()
    {
        // 필드 초기화
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 할당
        lineRenderer = GetComponent<LineRenderer>(); // LineRenderer 컴포넌트 할당
        lineRenderer.positionCount = 2; // 라인 렌더러 위치 개수 설정
        lineRenderer.startWidth = 0.1f; // 선 굵기 설정
        lineRenderer.endWidth = 0.1f; // 선 굵기 설정
        lineRenderer.enabled = false; // 라인 렌더러 비활성화
        PV = GetComponent<PhotonView>();

        idCounter = 0;
    }

    // Update 함수 정의
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); // 공의 회전을 고정시키기 위해 z축 회전값을 0으로 설정

    }

    // OnMouseDown 함수 정의
    void OnMouseDown()
    {
        PV.RPC("MouseDownRPC", RpcTarget.All, gameObject.tag); // 마우스 다운 RPC 호출
    }
    [Photon.Pun.PunRPC] // RPC 어트리뷰트 추가
    void MouseDownRPC(string tagName)
    { // 마우스 다운 RPC 함수 정의
        clicked = true;
        float angle = -1;
        // GameManager.allowShoot와 같은 비슷한 변수를 동기화하여 다른 플레이어들과 공유해야합니다.
        if (GameManager.allowShoot)
        {
            if (tagName == "Red" && GameManager.turn % 2 == 1) angle = 0;
            if (tagName == "Blue" && GameManager.turn % 2 == 0) angle = 180;
        }
        if (angle == -1) return;
        GameManager.allowShoot = false;
        dragStartPos = Input.mousePosition;
        lineRenderer.enabled = true;
    }
    // OnMouseDrag 함수 정의

    void OnMouseDrag()
    {
        PV.RPC("OnMouseDragRPC", RpcTarget.All);
    }
    [Photon.Pun.PunRPC] // RPC 어트리뷰트 추가
    void OnMouseDragRPC()
    {
        Vector3 dragEndPos = Input.mousePosition; // 드래그 종료 위치 설정
        Vector3 shootDirection = (dragStartPos - dragEndPos).normalized; // 발사 방향 벡터 계산
        float power = Mathf.Clamp((dragStartPos - dragEndPos).magnitude, 0, 3000f); // 발사력 계산
        lineRenderer.SetPosition(0, transform.position);
        // 라인 렌더러 위치 설정
        if (clicked && GameManager.turn % 2 == 1 && tag == "Red") lineRenderer.SetPosition(1, transform.position + new Vector3(shootDirection.x, 0, shootDirection.y) * power / 10);
        if (clicked && GameManager.turn % 2 == 0 && tag == "Blue") lineRenderer.SetPosition(1, transform.position - new Vector3(shootDirection.x, 0, shootDirection.y) * power / 10);
    }

    void OnMouseUp()
    {
        PV.RPC("OnMouseUpRPC", RpcTarget.All);
    }
    [Photon.Pun.PunRPC] // RPC 어트리뷰트 추가
    // OnMouseUp 함수 정의
    void OnMouseUpRPC()
    {
        Vector3 dragEndPos = Input.mousePosition; // 드래그 종료 위치 설정
        Vector3 shootDirection = (dragStartPos - dragEndPos).normalized; // 발사 방향 벡터 계산
        float power = Mathf.Clamp((dragStartPos - dragEndPos).magnitude, 0, 700f); // 발사력 계산
        if (GameManager.turn % 2 == 1 && tag == "Red" && !_isStopped) { rb.AddForce(new Vector3(shootDirection.x, 0, shootDirection.y) * power); StartCoroutine("Shoot"); }// Shoot 코루틴 함수를 시작함. // 공에 발사력을 가함
        if (GameManager.turn % 2 == 0 && tag == "Blue" && !_isStopped) { rb.AddForce(new Vector3(-shootDirection.x, 0, -shootDirection.y) * power); StartCoroutine("Shoot"); }// Shoot 코루틴 함수를 시작함. // 공에 발사력을 가함
        lineRenderer.enabled = false; // 라인 렌더러 비활성화

    }
    IEnumerator Shoot() // Shoot 코루틴 함수
    {
        yield return new WaitForSeconds(0.5f); // 0.1초 대기
        while (rb.velocity.magnitude > 0.1 && transform.position.y >= 0 && Time.timeScale != 0) yield return new WaitForSeconds(0.1f); // 공의 속도가 0.1 이상이고, 공의 위치가 높이 0 이상인 동안 0.1초씩 대기
        if (GameManager.turn % 2 == 1)
        {// 현재 턴이 홀수면
            Quternion.Oce = 1;
            GameObject.Find("Main Camera").GetComponent<GameManager>().player2Turn(); // player2Turn 함수 실행
        }
        else
        {// 현재 턴이 짝수면
            Quternion.Oce = 1;
            GameObject.Find("Main Camera").GetComponent<GameManager>().player1Turn(); // player1Turn 함수 실행
        }
        if(GameManager.turn % 5 == 0) {
            GameManager.allowShoot = true;
            SceneManager.LoadScene("FpsScene");
        }
        GameManager.allowShoot = true; // allowShoot 변수를 true로 변경
    }

    void OnDisable()
    {
        // 경로 지정
        
        string pos = transform.position.x + "/" + transform.position.y + "/" + transform.position.z;
        string rot = transform.rotation.eulerAngles.x + "/" + transform.rotation.eulerAngles.y + "/" + transform.rotation.eulerAngles.z;

        al.pos = pos;
        al.rot = rot;

        if (gameObject.tag == "Red")
        {
            path = Application.persistentDataPath + "/black" + id;
            string data = JsonUtility.ToJson(al);
            File.WriteAllText(path, data);
        }
        if (gameObject.tag == "Blue")
        {
            path = Application.persistentDataPath + "/white" + id;
            string data = JsonUtility.ToJson(al);
            File.WriteAllText(path, data);
        }
    }
}
