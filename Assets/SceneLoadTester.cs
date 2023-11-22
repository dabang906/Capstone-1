using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadingSceneController.LoadScene("UiScene");
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
