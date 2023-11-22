using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private void Awake() {
        ResumeGame();
        var objs = FindObjectsOfType<DontDestroyObject>();
        if(objs.Length == 6){
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ResumeGame ()
    {
        Time.timeScale = 1;
    }
    
}
