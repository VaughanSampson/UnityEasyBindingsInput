# UnityEasyPlayerBindings
There is not much clear documentation on Unity's standard [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) in terms of rebinding keys. This script (`UnityEasyPlayerBindings`) streamlines functionality of the most common and desirable control rebinding features for unity projects.

With the script, you can use simple single-line methods to:
+ Rebind controls, including to specific bindings within an action or a composite action.
+ Initiate an interactive rebind where the user's next input control will be bound to a desired binding.
+ Get the users next input as a string binding path.
+ Set a binding by binding path.
+ Save and load all bindings. 


## Setup
To begin using EasyPlayerBindings in Unity, place the EasyPlayerBindings.cs file somewhere in your Unity project's Asset folder. Now pick/create your class which manages player input. This class should:
1. Import with `using EPBindings;`.
2. have a reference to the PlayerInput instance.
3. Instantiate and store an `EasyPlayerBindings` object.


For example:
```
using UnityEngine;
using EPBindings;

public class PlayerInputController : MonoBehaviour
{ 
    [SerializeField] private PlayerInput input; //Assigned in inspector
    private EasyPlayerBindings easyPlayerBindings; //To be instantiated

    private void OnEnable()
    {
        easyPlayerBindings = new EasyPlayerBindings(input); //Create instance, passing PlayerInput object
    }
}
``` 


## Using UnityEasyPlayerBindings
The following action map will be used for examples.

![Capture](https://github.com/VaughanSampson/UnityEasyPlayerBindings/assets/128713660/e96056e9-0b29-4e35-abf1-a7351c16332f)

### Set Action Map
To change any action, its parent action map must be selected.
```
SelectActionMap(string actionMapName)
```
Example:
```
easyPlayerBindings.SelectActionMap("Sample")
``` 
<br> 

### Rebinds with a binding path.

To rebind the _nth_ binding of a non-composite action:
```
RebindTo(string actionName, int bindingIndex, string newBindingPath)
```
Example: Binding the 'Jump' action's second binding to 'g'.
```
RebindTo("Dive", 1, "<Keyboard>/#(g)")
```
<br><br>

To rebind the labeled binding of a composite action:
```
RebindToComposite(string actionName, string compositeName, string compositeBindingName, string newBindingPath)
``` 
Example: Binding the 'Move' action's 'RightAndLeftSec' 'Negative' binding to 'g'.
```
RebindToComposite("Move", "RightAndLeftSec", "Negative", "<Keyboard>/#(g)")
```
<br><br>

To rebind the first/only binding of a non-composite action:
```
RebindTo(string actionName, string newBindingPath)
```
Example: Binding the 'Dive' action's only binding to 'g'.
```
easyPlayerBindings.RebindTo("Dive", "<Keyboard>/#(g)")
```
<br><br>


 




 
