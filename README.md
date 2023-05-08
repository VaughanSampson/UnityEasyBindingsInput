# UnityEasyPlayerBindings
`UnityEasyPlayerBindings` will streamline the process of managing the controller bindings of Unity's [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) class. Varieties of setting, saving and loading rebindings are condensed into single line functions for easy use across any game projects.

### This class supports:
+ Interactive rebinding of action controls.
+ Interactive rebinding of composite controls.
+ Saving and loading of customized controls.

## Setup
To begin using EasyPlayerBindings in Unity, have the EasyPlayerBindings.cs file into your Unity project's Asset folder. Now any C# script in your project can access this class through the `EPBindings` namespace. To do this, put `using EPBindings;` at the top of the accessing C# file.

## Usage
In an input managing script, an EasyPlayerBindings object can be referenced and instantiated.

Like:
```
using UnityEngine;
using EPBindings;

public class PlayerInputController : MonoBehaviour
{ 
    [SerializeField] private PlayerInput input;
    private EasyPlayerBindings inputBindings;

    private void Start()
    {
        _inputBindings = new EasyPlayerBindings(input); 
    }
}
``` 

Now methods within this class are called to control user input bindings. 

### Later implementations will support:
+ Setting bindings non-interactively.
+ UI supportive features for doing interactive rebindings.
