using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        GetComponent<Text>().text = Data.score.ToString("000");
    }
}
