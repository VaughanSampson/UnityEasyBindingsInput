using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EPBindings
{
    public class EasyPlayerBindings
    {
         
        private PlayerInput playerInput;
        private InputActionMap actionMap;
         
        private InputActionRebindingExtensions.RebindingOperation rebindOperation;
        private bool doingInteractiveRebind = false; 

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
        /// This <c>SelectActionMap</c> method selects an selectedAction map from the
        /// <c>PlayerInput</c> class. This is neccessary rebind an selectedAction on this map.
        /// </summary>
        /// <param name="actionMapName">The name of the selectedAction map.</param>
        public void SelectActionMap(string actionMapName)
        {
            actionMap = playerInput.actions.FindActionMap(actionMapName); 
        }
         
        /// <summary>
        /// This predicate is checked to avoid some errors when doing an interactive rebind.
        /// </summary>
        /// <param name="selectedAction">The selected selectedAction for rebinding.</param>
        /// <returns></returns>
        private bool CanDoInteractiveRebind(InputActionReference selectedAction)
        {
            if (actionMap == null)
            {
                Debug.LogWarning("There is no selected selectedAction map so an interactive rebind was terminated." 
                    + "Select an selectedAction map with SelectActionMap(string actionMapName).");
                return false;
            }
            else
            if (selectedAction == null || selectedAction.ToInputAction() == null)
            {
                Debug.LogWarning("The selectedAction name string given does not match any selectedAction from the selected actionMap."
                    +"The interactive rebind is terminated.");
                return false;
            }
            else
            if (doingInteractiveRebind)
            {
                Debug.LogWarning("An InteractiveRebind is already occurring.");
                return false;
            } 

            return true;

        }

        /// <summary>
        /// This method gets an selectedAction from an actionName and does an interactive rebinding with it.
        /// </summary>
        /// <param name="actionName">The name of the selectedAction.</param>
        /// <param name="actionBindingIndex">The bindingIndex of the specific binding within the selectedAction.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveRebind(string actionName, int actionBindingIndex = 0, string controlsExcluding = "")
        { 
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(actionMap.FindAction(actionName));
            InteractiveRebind(selectedAction, actionBindingIndex, controlsExcluding);

        }

        /// <summary>
        /// This method does an interactive rebinding to a given composite selectedAction.
        /// </summary>
        /// <param name="actionName">The name of the selectedAction.</param>
        /// <param name="compositePartName">The name of the composite part which is being rebinded.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveCompositeRebind(string actionName, string compositePartName, string controlsExcluding = "")
        {
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(playerInput.currentActionMap.FindAction(actionName));

            var bindingIndex = selectedAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == compositePartName.ToLower());

            InteractiveRebind(selectedAction, bindingIndex, controlsExcluding);

        }

        /// <summary>
        /// This method does an interactive rebinding to a given selectedAction.
        /// </summary>
        /// <param name="actionName">The selectedAction to rebind to.</param>
        /// <param name="actionBindingIndex">The bindingIndex of the specific binding within the selectedAction.</param>
        /// <param name="controlsExcluding">The string representation of the controls that should be ignored and not mapped to.</param>
        public void InteractiveRebind(InputActionReference selectedAction, int actionBindingIndex = 0, string controlsExcluding = "")
        { 
            if (!CanDoInteractiveRebind(selectedAction))
                return;

            doingInteractiveRebind = true;
            actionMap.Disable();

            rebindOperation = selectedAction.action.PerformInteractiveRebinding();

            if (controlsExcluding != "")
                rebindOperation.WithControlsExcluding(controlsExcluding);

            rebindOperation.WithTargetBinding(actionBindingIndex);

            rebindOperation.OnMatchWaitForAnother(0.1f);
            rebindOperation.OnComplete(operation => InteractiveRebindClosed());
            rebindOperation.OnCancel(operation => InteractiveRebindClosed());

            rebindOperation.Start();

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
        /// This predicate is used to check if an interactive rebinding is already occurring.
        /// </summary>
        /// <returns></returns>
        public bool IsDoingInteractiveRebind()
        {
            return doingInteractiveRebind;
        }
         


        public void RebindTo(string actionName, string newBindingPath) => RebindTo(actionName, 0, newBindingPath); 
        public void RebindTo(string actionName, int bindingIndex, string newBindingPath)
        {
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(actionMap.FindAction(actionName)); 

            RebindTo(selectedAction, bindingIndex, newBindingPath);
        } 

        public void RebindToComposite(string actionName, string compositePartName, string newBindingPath)
        {
            InputActionReference selectedAction = ScriptableObject.CreateInstance<InputActionReference>();
            selectedAction.Set(actionMap.FindAction(actionName)); 
            var bindingIndex = selectedAction.action.bindings.IndexOf(x => x.isPartOfComposite && x.name.ToLower() == compositePartName.ToLower());

            RebindTo(selectedAction, bindingIndex, newBindingPath);
        }
         
        public void RebindTo(InputActionReference selectedAction, int actionBindingIndex, string newBindingPath)
        {
            InputBinding binding = selectedAction.action.bindings[actionBindingIndex];
            binding.overridePath = newBindingPath;
            selectedAction.action.ApplyBindingOverride(actionBindingIndex, binding);

        } 


        /// <summary>
        /// This "SaveRebinds" saves rebind information to player prefs.
        /// </summary>
        /// <param name="saveTitle">The title used to save rebinds more uniquely.</param>
        public void SaveRebinds(string saveTitle = "playerActionBindings")
        {
            string rebinds = playerInput.actions.SaveBindingOverridesAsJson(); 
            PlayerPrefs.SetString(saveTitle + "_rebinds", rebinds);
        }

        /// <summary>
        /// This "SaveRebinds" loads rebind information from player prefs.
        /// </summary>
        /// <param name="saveTitle">The title used to save rebinds more uniquely.</param>
        public void LoadRebinds(string saveTitle = "playerActionBindings")
        {
            string rebinds = PlayerPrefs.GetString(saveTitle + "_rebinds", string.Empty);
            if (string.IsNullOrEmpty(rebinds)) { return; }
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }

    }



}