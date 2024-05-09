//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.2
//     from Assets/Settings/Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public partial class @Controls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Global"",
            ""id"": ""74f02d9e-0e43-423c-860d-1277e74aae3d"",
            ""actions"": [
                {
                    ""name"": ""VolumeDown"",
                    ""type"": ""Button"",
                    ""id"": ""f5935f86-7449-4747-8e5a-131c59b942ae"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""VolumeUp"",
                    ""type"": ""Button"",
                    ""id"": ""7cda4ef9-834f-4667-83c9-72af7034eeef"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""NextClip"",
                    ""type"": ""Button"",
                    ""id"": ""31a0d0fe-8102-44ab-a4c7-7b413014bfd6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PreviousClip"",
                    ""type"": ""Button"",
                    ""id"": ""591a9927-d58f-42c3-aa37-b2a1cd0f1070"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""01e41582-5cb0-4a12-afad-7166f6a418a5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ba238885-e49d-4849-bbe0-2c93d638c1ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VolumeDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11bcf75e-fd1f-4bbf-a29a-5ba9c21d8a64"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""VolumeUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""32fd042e-3c6f-474d-87f9-54b4c39e65e5"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextClip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""338086b4-c247-41f0-9e7b-a2c93fb91b3f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousClip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afe18884-d26a-4625-8824-155a6de09542"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Global
        m_Global = asset.FindActionMap("Global", throwIfNotFound: true);
        m_Global_VolumeDown = m_Global.FindAction("VolumeDown", throwIfNotFound: true);
        m_Global_VolumeUp = m_Global.FindAction("VolumeUp", throwIfNotFound: true);
        m_Global_NextClip = m_Global.FindAction("NextClip", throwIfNotFound: true);
        m_Global_PreviousClip = m_Global.FindAction("PreviousClip", throwIfNotFound: true);
        m_Global_Delete = m_Global.FindAction("Delete", throwIfNotFound: true);
    }

    ~@Controls()
    {
        Debug.Assert(!m_Global.enabled, "This will cause a leak and performance issues, Controls.Global.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Global
    private readonly InputActionMap m_Global;
    private List<IGlobalActions> m_GlobalActionsCallbackInterfaces = new List<IGlobalActions>();
    private readonly InputAction m_Global_VolumeDown;
    private readonly InputAction m_Global_VolumeUp;
    private readonly InputAction m_Global_NextClip;
    private readonly InputAction m_Global_PreviousClip;
    private readonly InputAction m_Global_Delete;
    public struct GlobalActions
    {
        private @Controls m_Wrapper;
        public GlobalActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @VolumeDown => m_Wrapper.m_Global_VolumeDown;
        public InputAction @VolumeUp => m_Wrapper.m_Global_VolumeUp;
        public InputAction @NextClip => m_Wrapper.m_Global_NextClip;
        public InputAction @PreviousClip => m_Wrapper.m_Global_PreviousClip;
        public InputAction @Delete => m_Wrapper.m_Global_Delete;
        public InputActionMap Get() { return m_Wrapper.m_Global; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GlobalActions set) { return set.Get(); }
        public void AddCallbacks(IGlobalActions instance)
        {
            if (instance == null || m_Wrapper.m_GlobalActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GlobalActionsCallbackInterfaces.Add(instance);
            @VolumeDown.started += instance.OnVolumeDown;
            @VolumeDown.performed += instance.OnVolumeDown;
            @VolumeDown.canceled += instance.OnVolumeDown;
            @VolumeUp.started += instance.OnVolumeUp;
            @VolumeUp.performed += instance.OnVolumeUp;
            @VolumeUp.canceled += instance.OnVolumeUp;
            @NextClip.started += instance.OnNextClip;
            @NextClip.performed += instance.OnNextClip;
            @NextClip.canceled += instance.OnNextClip;
            @PreviousClip.started += instance.OnPreviousClip;
            @PreviousClip.performed += instance.OnPreviousClip;
            @PreviousClip.canceled += instance.OnPreviousClip;
            @Delete.started += instance.OnDelete;
            @Delete.performed += instance.OnDelete;
            @Delete.canceled += instance.OnDelete;
        }

        private void UnregisterCallbacks(IGlobalActions instance)
        {
            @VolumeDown.started -= instance.OnVolumeDown;
            @VolumeDown.performed -= instance.OnVolumeDown;
            @VolumeDown.canceled -= instance.OnVolumeDown;
            @VolumeUp.started -= instance.OnVolumeUp;
            @VolumeUp.performed -= instance.OnVolumeUp;
            @VolumeUp.canceled -= instance.OnVolumeUp;
            @NextClip.started -= instance.OnNextClip;
            @NextClip.performed -= instance.OnNextClip;
            @NextClip.canceled -= instance.OnNextClip;
            @PreviousClip.started -= instance.OnPreviousClip;
            @PreviousClip.performed -= instance.OnPreviousClip;
            @PreviousClip.canceled -= instance.OnPreviousClip;
            @Delete.started -= instance.OnDelete;
            @Delete.performed -= instance.OnDelete;
            @Delete.canceled -= instance.OnDelete;
        }

        public void RemoveCallbacks(IGlobalActions instance)
        {
            if (m_Wrapper.m_GlobalActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGlobalActions instance)
        {
            foreach (var item in m_Wrapper.m_GlobalActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GlobalActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GlobalActions @Global => new GlobalActions(this);
    public interface IGlobalActions
    {
        void OnVolumeDown(InputAction.CallbackContext context);
        void OnVolumeUp(InputAction.CallbackContext context);
        void OnNextClip(InputAction.CallbackContext context);
        void OnPreviousClip(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
    }
}
