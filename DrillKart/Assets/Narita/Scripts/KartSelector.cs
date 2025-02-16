using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KartSelector : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    public void KartSelect(GameObject kart)
    {
        GameManager.Kart = kart;
        Debug.Log(kart.name + "‚ª‘I‚Î‚ê‚Ü‚µ‚½");
        SceneManager.LoadScene("TestScene");
    }
}
