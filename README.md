# UnityEasyPlayerBindings
There is a surprisingly little amount of clear documentation on Unity's standard [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) systems. For some, especially newer programmers, creating basic rebinding code for this unneccesarily complicated input standard would be a wasteful time sink. So `UnityEasyPlayerBindings` simplifies access to the most common and desirable control rebinding features for video game projects. 

With this class, you can use simple single-line methods to:
+ Rebind controls. This includes rebinding specific bindings within an action or composite action.
+ Initiate an interactive rebind. Here the user's next input will be bound to a desired input binding.
+ Save and load all bindings.  


## Setup
To begin using EasyPlayerBindings in Unity, put the EasyPlayerBindings.cs file in your Unity project's Asset folder. Now pick/create a class which manages player input. This class should:
1. Import the class with `using EPBindings;`.
2. Have access to the PlayerInput class which will be effected.
3. Instantiate and store a reference to an `EasyPlayerBindings` object.


For example:
```
using UnityEngine;
using EPBindings;

public class PlayerInputController : MonoBehaviour
{ 
    [SerializeField] private PlayerInput input;
    private EasyPlayerBindings inputBindings;

    private void Start()
    {
        inputBindings = new EasyPlayerBindings(input); 
    }
}
``` 
<br><br>
## Using UnityEasyPlayerBindings
The following action map will be used for examples.

![Capture](https://github.com/VaughanSampson/UnityEasyPlayerBindings/assets/128713660/e96056e9-0b29-4e35-abf1-a7351c16332f)

### Set Action Map
The ActionMap holding the action which should be changed **must be made current to do any rebindings**.
```
SelectActionMap(string actionMapName)
```
Example:
```
SelectActionMap("Sample")
```
Note: This will make the given ActionMap current and active in the `PlayerInput` component. If this is not wanted, simply call the function again with a different  action map name after the rebind is complete.
<br><br>


### Basic Rebind to Action
Use the following to rebind to a non-composite action **which has only one binding**. E.g. to rebind "Dive" in the example image.
```
RebindTo(string actionName, string newBindingPath)
```
Example: Setting the 'Dive' action's single binding to 'g'.
```
RebindTo("Dive", "<Keyboard>/#(g)")
```
<br><br>


Use the following to rebind to a non-composite action **which multiple bindings**. E.g. to rebind "Jump" in the example image.
```
RebindTo(string actionName, int bindingIndex, string newBindingPath)
```
Example: Setting the 'Jump' action's second binding to 'g'.
```
RebindTo("Dive", 1, "<Keyboard>/#(g)")
```
<br><br>


Use the following to rebind to a composite action. E.g. to rebind "Move" in the example image.
```
RebindToComposite(string actionName, string compositeName, string compositeBindingName, string newBindingPath)
```
Here you will specify the action name (e.g. 'Move'), the name of composite binding set (e.g. 'RightAndLeft'), and the name of the specific binding (e.g. 'Positive').

Example: Setting the 'Move' action's 'RightAndLeftSec' 'Negative' binding to 'g'.
```
RebindToComposite("Move", "RightAndLeftSec", "Negative", "<Keyboard>/#(g)")
```

Note: The composite commands are case-insensitive, given inconsistency in Unity backend naming conventions.
 





### Later implementations will support:
+ UI supportive features for doing interactive rebindings.
