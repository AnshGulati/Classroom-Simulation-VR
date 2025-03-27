using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public InputActionProperty showButton;
    public GameObject objectA;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        menu.transform.position = objectA.transform.position;


        if (showButton.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);
        }
    }
}