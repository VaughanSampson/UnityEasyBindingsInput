using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EasyPlayerBindings
{
    [System.Serializable]
    public class EasyPlayerBindings
    {
         
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
        /// Predicate <c>CanDoInteractiveRebind</c> returns true if this object has enough 
        /// information to perform and interactive rebind; otherwise, false.
        /// </summary>
        /// <returns></returns>
        private bool CanDoInteractiveRebind(InputActionReference selectedAction)
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

        public void InteractiveRebind(string actionName, int index = 0, string controlsExcluding = "")
        { 
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(playerInput.currentActionMap.FindAction(actionName));
            InteractiveRebind(selectedAction, index, controlsExcluding);

        }

        public void InteractiveRebind(InputActionReference selectedAction, int index = 0, string controlsExcluding = "")
        { 
            if (!CanDoInteractiveRebind(selectedAction))
                return;

            doingInteractiveRebind = true;
            playerInput.currentActionMap.Disable();

            rebindOperation = selectedAction.action.PerformInteractiveRebinding();

            if (controlsExcluding != "")
                rebindOperation.WithControlsExcluding(controlsExcluding);

            rebindOperation.WithTargetBinding(index);

            rebindOperation.OnMatchWaitForAnother(0.1f);
            rebindOperation.OnComplete(operation => InteractiveRebindClosed());
            rebindOperation.OnCancel(operation => InteractiveRebindClosed());

            rebindOperation.Start();

        }

        public void InteractiveRebind(string actionName, string compositePartName, string controlsExcluding = "")
        {
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(playerInput.currentActionMap.FindAction(actionName));

            var bindingIndex = selectedAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == compositePartName.ToLower()); 

            InteractiveRebind(selectedAction, bindingIndex, controlsExcluding);

        }

        public void CancelInteractiveRebind() => rebindOperation.Cancel(); 

        private void InteractiveRebindClosed()
        { 
            rebindOperation.Dispose();
            doingInteractiveRebind = false;
            playerInput.currentActionMap.Enable();
        }


    }



    // TO BE IMPLEMENTED NEXT
    /*
     * Saving and loading of bindngs.'
     */
}