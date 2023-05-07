using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyPlayerBindings
{
    [System.Serializable]
    public class EasyPlayerBindings
    {

        [SerializeField] private InputActionReference selectedAction;
        [SerializeField] private PlayerInput playerInput;


        // Variables for performing an interactive rebinding
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
        private bool doingInteractiveRebind = false;
        public bool DoingInteractiveRebind { get { return doingInteractiveRebind; } }

        /// <summary>
        /// Constructor method <c>EasyPlayerBindings</c> initialises an EasyPlayerBindings object.
        /// </summary>
        /// <param name="playerInput"></param>
        public EasyPlayerBindings(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
            this.selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
        }

        /// <summary>
        /// Method <c>SelectActionMap</c> will select the <c>PlayerInput</c> class's current action map
        /// by its string name as long as it is defined in the <c>PlayerInput</c> class's current <c>Input
        /// Action Asset</c>.
        /// </summary>
        /// <param name="actionMapName"></param>
        /// <returns></returns>
        public void SelectActionMap(string actionMapName)
        {
            playerInput.SwitchCurrentActionMap(actionMapName);
        }

        /// <summary>
        /// Method <c>SelectAction</c> will select an action for rebinding
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public void SelectAction(string actionName)
        { 
            this.selectedAction.Set(playerInput.currentActionMap.FindAction(actionName));
        }


        /// <summary>
        /// Predicate <c>CanDoInteractiveRebind</c> returns true if this object has enough 
        /// information to perform and interactive rebind; otherwise, false.
        /// </summary>
        /// <returns></returns>
        private bool CanDoInteractiveRebind()
        {

            if (selectedAction == null || selectedAction.ToInputAction() == null)
            {
                Debug.LogWarning("Cannot InteractiveRebind if there is no selected action. " +
                    "Select an action with SelectAction(InputActionReference).");
                return false;
            }
            else
            if (doingInteractiveRebind)
            {
                Debug.LogWarning("Cannot InteractiveRebind more than once simultaneously.");
                return false;
            } 

            return true;

        }

        /// <summary>
        /// Method <c>InteractiveRebind</c> will take the player's next keyboard/mouse/controller/etc... input and bind
        /// it to the selected action. Select an action with <c>InteractiveRebind.SelectAction(string actionName)</c>
        /// </summary>
        /// <returns></returns>
        public void InteractiveRebind(string controlsExcluding = "")
        {
            if (!CanDoInteractiveRebind())
                return;
             
            doingInteractiveRebind = true;

            playerInput.currentActionMap.Disable();

            rebindOperation = selectedAction.action.PerformInteractiveRebinding();
            if (controlsExcluding != "")
                rebindOperation.WithControlsExcluding(controlsExcluding);
            rebindOperation.OnMatchWaitForAnother(0.1f);
            rebindOperation.OnComplete(operation => {
                rebindOperation.Dispose();
                doingInteractiveRebind = false;
                playerInput.currentActionMap.Enable();
            }).Start(); 
        }
        
        public void InteractiveRebindCompositeElement(string compositeName, string partName)
        {

            if (!CanDoInteractiveRebind())
                return;

            doingInteractiveRebind = true;

            playerInput.currentActionMap.Disable();

            rebindOperation = selectedAction.action.PerformInteractiveRebinding(); 

            var bindingIndex = selectedAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == partName.ToLower());

            InteractiveRebindCompositeAtIndex(bindingIndex);
        }



        private void InteractiveRebindCompositeAtIndex(int index)
        {
            if (index < 1)
            {
                Debug.LogError("Attempted to rebind to index below zero.");
                return;
            }

            rebindOperation.WithTargetBinding(index);


            rebindOperation.OnMatchWaitForAnother(0.1f);
            rebindOperation.OnComplete(operation => {
                rebindOperation.Dispose();
                doingInteractiveRebind = false;
                playerInput.currentActionMap.Enable();
            }).Start();
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