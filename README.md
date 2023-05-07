# UnityEasyPlayerBindings
The `EasyPlayerBindings` C# script allows easy management of player input bindings in Unity. This tool is specifically designed to simplify the binding customization features of Unity's [PlayerInput](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html) class.

### This class supports:
+ Interactive rebinding of individual binds within actions.
+ Interactive rebinding of composite binds.
+ Saving and loading bindings.

## Setup
Download the EasyPlayerBindings.cs file. This has all the code inside of it. Move this into your Unity project's Asset folder wherever is will be kept. Now, any cs file can acess this class through the `EPBindings` namespace. To do this, put `using EPBindings;` at the top of the cs file.

## Usage
An `EasyPlayerBindings` object should be instantiate and referenced by some other object which will not be destroyed randomly. For example, a `PlayerInputController` class or a `GameController` class. 

Instantiation example:
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


### Later implementations will support:
+ Setting bindings non-interactively.
+ UI supportive features for doing interactive rebindings.
