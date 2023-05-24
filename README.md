# UnityEasyPlayerBindings
There is a surprisingly little amount of clear documentation on Unity's standard [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) functionality. For some programmers, creating basic rebinding code for this input standard is time sink. This package (`UnityEasyPlayerBindings`) streamlines functionality of the most common and desirable control rebinding features for video game projects. 

With this class, you can use simple single-line methods to:
+ Rebind controls. That is: rebind specific bindings within an action or composite action.
+ Initiate an interactive rebind where the user's next input will be bound to a desired input binding.
+ Get the users next input as a string binding path.
+ Set a binding by binding path.
+ Save and load all bindings. 


## Setup
To begin using EasyPlayerBindings in Unity, place the EasyPlayerBindings.cs file somewhere in your Unity project's Asset folder. Now pick/create a class which manages player input. This class should:
1. Import with `using EPBindings;`.
2. Access the main PlayerInput class.
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
To rebind the first/only binding of a non-composite action:
```
RebindTo(string actionName, string newBindingPath)
```
Example: Binding the 'Dive' action's only binding to 'g'.
```
easyPlayerBindings.RebindTo("Dive", "<Keyboard>/#(g)")
```
<br><br>


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

Note: strings passed are case-insensitive, given inconsistency in backend Unity naming conventions.
 





### Later implementations will support:
+ UI supportive features for doing interactive rebindings.
