using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInputBindings
{
    public class PlayerInputBindings : MonoBehaviour
    {

        [SerializeField] private InputActionReference selectedAction;
        [SerializeField] private PlayerInput playerInput;

        public bool SelectActionMap(string str)
        {
            playerInput.SwitchCurrentActionMap(str);
            return true;
        }

        public bool SelectAction(string str)
        {
            this.selectedAction.Set(playerInput.currentActionMap.FindAction(str));
            return true;
        }


        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
        private bool doingInteractiveRebind = false;
        public bool DoingInteractiveRebind { get { return doingInteractiveRebind; } }
        public bool PerformInteractiveRebind(string controlsExcluding = "")
        {
            if (selectedAction == null || selectedAction.ToInputAction() == null)
            {
                Debug.LogWarning("Cannot PerformInteractiveRebind if there is no selected action. Select an action with SelectAction(InputActionReference).");
                return false;
            }

            if(doingInteractiveRebind)
            {
                Debug.LogWarning("Cannot PerformInteractiveRebind more than once simultaneously.");
                return false;
            }

            if (playerInput.currentActionMap == selectedAction.ToInputAction().actionMap)
            {
                Debug.LogWarning("Cannot PerformInteractiveRebind on currently active ActionMap");
                return false;
            }

            doingInteractiveRebind = true;

            rebindingOperation = selectedAction.action.PerformInteractiveRebinding()
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => PerformInteractiveRebindComplete())
                .Start();
            
            return true;
        }

        private void PerformInteractiveRebindComplete()
        {
            rebindingOperation.Dispose();
            doingInteractiveRebind = false;
        }


    }
}