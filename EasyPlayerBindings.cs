using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EPBindings
{
    public class EasyPlayerBindings
    {
         
        private PlayerInput playerInput;
        private InputActionMap actionMap;

        // Variables for performing an interactive rebinding
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
        private bool doingInteractiveRebind = false;
        public bool DoingInteractiveRebind { get { return doingInteractiveRebind; } }

        /// <summary>
        /// This constructor necessarily gives a new <c>EasyPlayerBindings</c> object
        /// a reference to a <c>PlayerInput</c> class.
        /// </summary>
        /// <param name="playerInput">A <c>PlayerInput</c> class.</param>
        public EasyPlayerBindings(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }

        /// <summary>
        /// This <c>SelectActionMap</c> method selects an action map from the
        /// <c>PlayerInput</c> class. This is neccessary rebind an action on this map.
        /// </summary>
        /// <param name="actionMapName">The name of the action map.</param>
        public void SelectActionMap(string actionMapName)
        {
            actionMap = playerInput.actions.FindActionMap(actionMapName); 
        }
         
        /// <summary>
        /// This predicate is used to check if an interactive rebinding is already occurring.
        /// </summary>
        /// <returns></returns>
        public bool IsDoingInteractiveRebind()
        {
            return doingInteractiveRebind;
        }

        /// <summary>
        /// This predicate is checked to avoid some errors when doing an interactive rebind.
        /// </summary>
        /// <param name="selectedAction">The selected action for rebinding.</param>
        /// <returns></returns>
        private bool CanDoInteractiveRebind(InputActionReference selectedAction)
        {
            if (actionMap == null)
            {
                Debug.LogWarning("There is no selected action map so an interactive rebind was terminated." 
                    + "Select an action map with SelectActionMap(string actionMapName).");
                return false;
            }
            else
            if (selectedAction == null || selectedAction.ToInputAction() == null)
            {
                Debug.LogWarning("The action name string given does not match any action from the selected actionMap."
                    +"The interactive rebind is terminated.");
                return false;
            }
            else
            if (doingInteractiveRebind)
            {
                Debug.LogWarning("An InteractiveRebind is already occurring simultaneously.");
                return false;
            } 

            return true;

        }

        /// <summary>
        /// This method gets an action from an actionName and does an interactive rebinding with it.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="index">The index of the specific binding within the action.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveRebind(string actionName, int index = 0, string controlsExcluding = "")
        { 
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(actionMap.FindAction(actionName));
            InteractiveRebind(selectedAction, index, controlsExcluding);

        }

        /// <summary>
        /// This method does an interactive rebinding to a given action.
        /// </summary>
        /// <param name="actionName">The action to rebind to.</param>
        /// <param name="index">The index of the specific binding within the action.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveRebind(InputActionReference selectedAction, int index = 0, string controlsExcluding = "")
        { 
            if (!CanDoInteractiveRebind(selectedAction))
                return;

            doingInteractiveRebind = true;
            actionMap.Disable();

            rebindOperation = selectedAction.action.PerformInteractiveRebinding();

            if (controlsExcluding != "")
                rebindOperation.WithControlsExcluding(controlsExcluding);

            rebindOperation.WithTargetBinding(index);

            rebindOperation.OnMatchWaitForAnother(0.1f);
            rebindOperation.OnComplete(operation => InteractiveRebindClosed());
            rebindOperation.OnCancel(operation => InteractiveRebindClosed());

            rebindOperation.Start();

        }

        /// <summary>
        /// This method does an interactive rebinding to a given composite action.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="compositePartName">The name of the composite part which is being rebinded.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveRebind(string actionName, string compositePartName, string controlsExcluding = "")
        {
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(playerInput.currentActionMap.FindAction(actionName));

            var bindingIndex = selectedAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == compositePartName.ToLower()); 

            InteractiveRebind(selectedAction, bindingIndex, controlsExcluding);

        }

        /// <summary>
        /// This method will stop an active interactive rebindng.
        /// </summary>
        public void CancelInteractiveRebind() => rebindOperation.Cancel();

        /// <summary>
        /// This is called to cleanup leftover reminents of an interactive rebinding.
        /// </summary>
        private void InteractiveRebindClosed()
        { 
            rebindOperation.Dispose();
            doingInteractiveRebind = false;
            playerInput.currentActionMap.Enable();
        }

        /// <summary>
        /// This "SaveRebinds" saves rebind information to player prefs.
        /// </summary>
        /// <param name="saveTitle">The title used to save rebinds more uniquely.</param>
        public void SaveRebinds(string saveTitle = "actions")
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson(); 
            PlayerPrefs.SetString(saveTitle + "_rebinds", rebinds);
        }

        /// <summary>
        /// This "SaveRebinds" loads rebind information from player prefs.
        /// </summary>
        /// <param name="saveTitle">The title used to save rebinds more uniquely.</param>
        public void LoadRebinds(string saveTitle = "actions")
        {
            string rebinds = PlayerPrefs.GetString(saveTitle + "_rebinds", string.Empty);
            if (string.IsNullOrEmpty(rebinds)) { return; }
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }

    }



}