# UnityEasyPlayerBindings
`UnityEasyPlayerBindings` simplifies the managing the user bindings for Unity's [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) class. Forms of setting, saving and loading rebindings are condensed into single line functions for easy use across any game projects.

### This class supports:
+ Easy interactive rebinding of action controls.
+ Easy direct rebinging of action controls.
+ Saving and loading of customized controls.

## Setup
To begin using EasyPlayerBindings in Unity, have the EasyPlayerBindings.cs file into your Unity project's Asset folder. Now any C# script in your project can access this class through the `EPBindings` namespace. For this, put `using EPBindings;` at the top of the an accessing C# script.

## Usage
In an input managing script, an `EasyPlayerBindings` object can be referenced and instantiated.

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

## Methods within an initialized UnityEasyPlayerBindings object should be called to change, save and load user input bindings. 
 




### Later implementations will support:
+ UI supportive features for doing interactive rebindings.
