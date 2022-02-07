using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rebinding : MonoBehaviour
{

    [Header("Jump")]
    [SerializeField] private InputActionReference jumpAction = null;
    [SerializeField] private TMP_Text jumpBindingText = null;
    [SerializeField] private GameObject jumpRebindObject = null;
    
    [Header("Dash")]
    [SerializeField] private InputActionReference dashAction = null;
    [SerializeField] private TMP_Text dashBindingText = null;
    [SerializeField] private GameObject dashRebindObject = null;
    
    [Header("Interact")]
    [SerializeField] private InputActionReference interactAction = null;
    [SerializeField] private TMP_Text interactBindingText = null;
    [SerializeField] private GameObject interactRebindObject = null;
    
    [Header("Sprint")]
    [SerializeField] private InputActionReference sprintAction = null;
    [SerializeField] private TMP_Text sprintBindingText = null;
    [SerializeField] private GameObject sprintRebindObject = null;
    
    [Header("Crouch")]
    [SerializeField] private InputActionReference crouchAction = null;
    [SerializeField] private TMP_Text crouchBindingText = null;
    [SerializeField] private GameObject crouchRebindObject = null;
    
    [Header("Fire/Use")]
    [SerializeField] private InputActionReference useAction = null;
    [SerializeField] private TMP_Text useBindingText = null;
    [SerializeField] private GameObject useRebindObject = null;
    
    [Header("Move Forward")]
    [SerializeField] private InputActionReference moveForwardAction = null;
    [SerializeField] private TMP_Text moveForwardBindingText = null;
    [SerializeField] private GameObject moveForwardRebindObject = null;
    
    [Header("Move Back")]
    [SerializeField] private InputActionReference moveBackAction = null;
    [SerializeField] private TMP_Text moveBackBindingText = null;
    [SerializeField] private GameObject moveBackRebindObject = null;
    
    [Header("Move Left")]
    [SerializeField] private InputActionReference moveLeftAction = null;
    [SerializeField] private TMP_Text moveLeftBindingText = null;
    [SerializeField] private GameObject moveLeftRebindObject = null;
    
    [Header("Move Right")]
    [SerializeField] private InputActionReference moveRightAction = null;
    [SerializeField] private TMP_Text moveRightBindingText = null;
    [SerializeField] private GameObject moveRightRebindObject = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartJumpRebinding()
    {
        jumpRebindObject.SetActive(false);

        rebindingOperation = jumpAction.action.PerformInteractiveRebinding()
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => JumpRebindComplete())
            .Start();
    }

    private void JumpRebindComplete()
    {
        jumpBindingText.text = InputControlPath.ToHumanReadableString(
            jumpAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        jumpRebindObject.SetActive(true);
    }
    
    public void StartDashRebinding()
    {
        dashRebindObject.SetActive(false);

        rebindingOperation = dashAction.action.PerformInteractiveRebinding()
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => DashRebindComplete())
            .Start();
    }
    
    private void DashRebindComplete()
    {
        int bindingIndex = dashAction.action.GetBindingIndexForControl(
            dashAction.action.controls[0]);
        
        dashBindingText.text = InputControlPath.ToHumanReadableString(
            dashAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        dashRebindObject.SetActive(true);
    }
    
    public void StartInteractRebinding()
    {
        interactRebindObject.SetActive(false);

        rebindingOperation = interactAction.action.PerformInteractiveRebinding()
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => InteractRebindComplete())
            .Start();
    }
    
    private void InteractRebindComplete()
    {
        int bindingIndex = interactAction.action.GetBindingIndexForControl(
            interactAction.action.controls[0]);
        
        interactBindingText.text = InputControlPath.ToHumanReadableString(
            interactAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        interactRebindObject.SetActive(true);
    }
    
    public void StartSprintRebinding()
    {
        sprintRebindObject.SetActive(false);

        rebindingOperation = sprintAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => SprintRebindComplete())
            .Start();
    }
    
    private void SprintRebindComplete()
    {
        int bindingIndex = sprintAction.action.GetBindingIndexForControl(
            sprintAction.action.controls[0]);
        
        sprintBindingText.text = InputControlPath.ToHumanReadableString(
            sprintAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        sprintRebindObject.SetActive(true);
    }
    
    public void StartCrouchRebinding()
    { crouchRebindObject.SetActive(false);

        rebindingOperation = crouchAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => CrouchRebindComplete())
            .Start();
    }
    
    private void CrouchRebindComplete()
    {
        int bindingIndex = crouchAction.action.GetBindingIndexForControl(
            crouchAction.action.controls[0]);
        
        crouchBindingText.text = InputControlPath.ToHumanReadableString(
            crouchAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        crouchRebindObject.SetActive(true);
    }
    
    public void StartUseRebinding()
    { useRebindObject.SetActive(false);

        rebindingOperation = useAction.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => UseRebindComplete())
            .Start();
    }
    
    private void UseRebindComplete()
    {
        int bindingIndex = useAction.action.GetBindingIndexForControl(
            useAction.action.controls[0]);
        
        useBindingText.text = InputControlPath.ToHumanReadableString(
            useAction.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        useRebindObject.SetActive(true);
    }
    
    public void StartMoveForwardRebinding()
    {
        moveForwardRebindObject.SetActive(false);

        rebindingOperation = moveForwardAction.action.PerformInteractiveRebinding(1)
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => MoveForwardRebindComplete())
            .Start();
    }
    
    private void MoveForwardRebindComplete()
    {
        moveForwardBindingText.text = InputControlPath.ToHumanReadableString(
            moveForwardAction.action.bindings[1].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        moveForwardRebindObject.SetActive(true);
    }
    
    public void StartMoveBackRebinding()
    {
        moveBackRebindObject.SetActive(false);

        rebindingOperation = moveBackAction.action.PerformInteractiveRebinding(2)
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => MoveBackRebindComplete())
            .Start();
    }
    
    private void MoveBackRebindComplete()
    {
        moveBackBindingText.text = InputControlPath.ToHumanReadableString(
            moveBackAction.action.bindings[2].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        moveBackRebindObject.SetActive(true);
    }
    
    public void StartMoveLeftRebinding()
    {
        moveLeftRebindObject.SetActive(false);

        rebindingOperation = moveLeftAction.action.PerformInteractiveRebinding(3)
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => MoveLeftRebindComplete())
            .Start();
    }
    
    private void MoveLeftRebindComplete()
    {
        moveLeftBindingText.text = InputControlPath.ToHumanReadableString(
            moveLeftAction.action.bindings[3].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        moveLeftRebindObject.SetActive(true);
    }
    
    public void StartMoveRightRebinding()
    {
        moveRightRebindObject.SetActive(false);

        rebindingOperation = moveRightAction.action.PerformInteractiveRebinding(4)
            
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => MoveRightRebindComplete())
            .Start();
    }
    
    private void MoveRightRebindComplete()
    {
        moveRightBindingText.text = InputControlPath.ToHumanReadableString(
            moveRightAction.action.bindings[4].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        
        rebindingOperation.Dispose();

        moveRightRebindObject.SetActive(true);
    }
}
