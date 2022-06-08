using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPad : Interactable
{
    [SerializeField]
    private GameObject door;
    private bool doorOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // this function is where we will design our interaction using code.
    protected override void Interact()
    {
        doorOpen = !doorOpen; // 참과 거짓 사이를 변환함.
        door.GetComponent<Animator>().SetBool("IsOpen", doorOpen);
    }
}
