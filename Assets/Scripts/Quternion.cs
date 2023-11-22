using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quternion : MonoBehaviour
{
    private SceneManager sceneManager;
    public static int Oce = 1;
    public static int quat = 0;
    public GameObject objectWith;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        /*sceneManager = GetComponent<SceneManager>();*/

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0); //바둑알이 구르지 않도록 하는 코드
        if (quat != 0 && quat % 2 == 1)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Red");

            foreach (GameObject obj in taggedObjects)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
        if (quat != 0 && quat % 2 == 0)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Blue");

            foreach (GameObject obj in taggedObjects)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
        if (quat > 0)
        {
            StartCoroutine(quata());
        }
    }
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Red" && GameManager.turn % 2 == 0 ||
        other.gameObject.tag == "Blue" && GameManager.turn % 2 == 1)
        {
            if (Oce == 1)
            {
                //PauseGame();
                //BallDrag ballDrag = objectWith.GetComponent<BallDrag>();
                //SceneManager.LoadScene("FpsScene");
            }
            Oce++;
        }
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    IEnumerator quata()
    {
        yield return new WaitForSeconds(0.5f); // 0.1초 대기
        quat = 0;
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Blue");

        foreach (GameObject obj in taggedObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        taggedObjects = GameObject.FindGameObjectsWithTag("Red");
        foreach (GameObject obj in taggedObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }
}
