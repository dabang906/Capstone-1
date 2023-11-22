using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Change2 : MonoBehaviour
{
    public void SceneChange(){
        if(GameManager.turn % 2 == 1){
            Quternion.quat = 2;
        }
        if(GameManager.turn % 2 == 0){
            Quternion.quat = 1;
        }
        SceneManager.LoadScene("MainScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
