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
        _inputBindings = new EasyPlayerBindings(input); 
    }
}
``` 

## Using UnityEasyPlayerBindings
The following Action Map will be used for examples.

![Capture](https://github.com/VaughanSampson/UnityEasyPlayerBindings/assets/128713660/69ad5751-7e3d-4a3b-9ff9-8fc3588eed84)

### Basic Rebind to Action
```
RebindTo(string actionName, string newBindingPath)
```
 





### Later implementations will support:
+ UI supportive features for doing interactive rebindings.
