using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{

    public OnPlayerTriggerEnter playerTrigger;

    public InputAction interact;
 
    [SerializeField] private InputActionAsset controls;
 
    private InputActionMap _inputActionMap;
 
    private void Start()
    {
        _inputActionMap = controls.FindActionMap("PlayerControls");
 
        interact = _inputActionMap.FindAction("Interact");
    }
 
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.tag == "Button" && playerTrigger.playerInTrigger)
            {
                
            }
            else if(selection.tag != "Button" || !playerTrigger.playerInTrigger)
            {
                
            }
        }
    }
}
