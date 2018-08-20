using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {

    void Awake()
    {
        Application.LoadLevel("Startskärm");
    }

}
