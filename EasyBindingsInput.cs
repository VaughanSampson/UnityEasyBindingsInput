using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerInputBindings
{
    public class EasyBindingsInput
    {

        [SerializeField] private InputActionReference selectedAction;
        [SerializeField] private PlayerInput playerInput;


        // Variables for performing an interactive rebinding
        private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
        private bool doingInteractiveRebind = false;
        public bool DoingInteractiveRebind { get { return doingInteractiveRebind; } }

        /// <summary>
        /// Constructor method <c>EasyBindingsInput</c> initialises an EasyBindingsInput object.
        /// </summary>
        /// <param name="playerInput"></param>
        public EasyBindingsInput(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }

        /// <summary>
        /// Method <c>SelectActionMap</c> will switch action maps by string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool SelectActionMap(string str)
        {
            playerInput.SwitchCurrentActionMap(str);
            return true;
        }

        /// <summary>
        /// Method <c>SelectAction</c> will select an action for rebinding
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool SelectAction(string str)
        {
            this.selectedAction.Set(playerInput.currentActionMap.FindAction(str));
            return true;
        }

        /// <summary>
        /// Method <c>PerformInteractiveRebind</c> will take the players next input to bind that to
        /// selected action.
        /// </summary>
        /// <param name="controlsExcluding"></param>
        /// <returns></returns>
        public bool PerformInteractiveRebind(string controlsExcluding = "")
        {
            if (selectedAction == null || selectedAction.ToInputAction() == null)
            {
                Debug.LogWarning("Cannot PerformInteractiveRebind if there is no selected action. " +
                    "Select an action with SelectAction(InputActionReference).");
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

            rebindingOperation = selectedAction.action
                .PerformInteractiveRebinding()
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => {
                    rebindingOperation.Dispose();
                    doingInteractiveRebind = false; 
                }).Start();
            
            return true;
        }


    }



    // TO BE IMPLEMENTED NEXT
    /*
     * Functionality for multiple keys.
     * Functionality for special bindings, e.g. composite.
     * Saving and loading of bindngs.'
     * 
     */
}