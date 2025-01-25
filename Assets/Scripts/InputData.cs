using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Singleton Class to get Controllers Input Data
/// It stores: 
///     Bools   => Primary Button, Secondary Button, Trigger Button, Grip buttons
///     Floats  => Trigger Value, Grip Value  (from 0 to 1)
///     Vector2 => Joystick Value             (from 0 to 1)
///     
///     Actions => The events will be fired only once when OnButtonPressed or OnButtonReleased
/// </summary>
public class InputData : MonoBehaviour
{
    // Singleton
    public static InputData Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            RightHandInputData.HandType = EHand.Right;
            LeftHandInputData.HandType = EHand.Left;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public UnityEngine.XR.InputDevice HMD;
    public UnityEngine.XR.InputDevice RightController;
    public UnityEngine.XR.InputDevice LeftController;

    public InputHandController RightHandInputData;
    public InputHandController LeftHandInputData;

    public enum EButtonAction
    {
        Pressed,
        Released
    }

    [System.Serializable]
    public enum EHand
    {
        Left,
        Right
    }

    [System.Serializable]
    public struct InputHandController
    {
        public EHand HandType;
        public bool PrimaryButton;
        public bool SecondaryButton;
        public float TriggerValue;
        public bool TriggerButton;
        public bool GripButton;
        public float GripValue;
        public Vector2 JoystickValue;

        public Action GripButtonPressed;
        public Action GripButtonReleased;
        public Action TriggerButtonPressed;
        public Action TriggerButtonReleased;
        public Action PrimaryButtonReleased;
        public Action PrimaryButtonPressed;
        public Action SecondaryButtonReleased;
        public Action SecondaryButtonPressed;
    }


    private void Update()
    {
        if (!HMD.isValid || !RightController.isValid || !LeftController.isValid)
            InitializeInputDevices();

        UpdateHandController(RightController, ref RightHandInputData);
        UpdateHandController(LeftController, ref LeftHandInputData);
    }


    void UpdateHandController(UnityEngine.XR.InputDevice device, ref InputHandController inputHandController)
    {
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool primaryButton))
            {
                if (!inputHandController.PrimaryButton && primaryButton) inputHandController.PrimaryButtonPressed?.Invoke();
                if (inputHandController.PrimaryButton && !primaryButton) inputHandController.PrimaryButtonReleased?.Invoke();
                inputHandController.PrimaryButton = primaryButton;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool secondaryButton))
            {
                if (!inputHandController.SecondaryButton && secondaryButton) inputHandController.SecondaryButtonPressed?.Invoke();
                if (inputHandController.SecondaryButton && !secondaryButton) inputHandController.SecondaryButtonReleased?.Invoke();
                inputHandController.SecondaryButton = secondaryButton;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out float trigger))
            {
                inputHandController.TriggerValue = trigger;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool triggerButton))
            {
                if (!inputHandController.TriggerButton && triggerButton) inputHandController.TriggerButtonPressed?.Invoke();
                if (inputHandController.TriggerButton && !triggerButton) inputHandController.TriggerButtonReleased?.Invoke();
                inputHandController.TriggerButton = triggerButton;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out float grip))
            {
                inputHandController.GripValue = grip;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out bool gripButton))
            {
                if (!inputHandController.GripButton && gripButton) inputHandController.GripButtonPressed?.Invoke();
                if (inputHandController.GripButton && !gripButton) inputHandController.GripButtonReleased?.Invoke();
                inputHandController.GripButton = gripButton;
            }
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 primary2DAxis))
            {
                inputHandController.JoystickValue = primary2DAxis;
            }
        }
        else
        {
            inputHandController.PrimaryButton = false;
            inputHandController.SecondaryButton = false;
            inputHandController.TriggerValue = 0f;
            inputHandController.TriggerButton = false;
            inputHandController.GripValue = 0f;
            inputHandController.GripButton = false;
            inputHandController.JoystickValue = Vector2.zero;
        }
    }

    private void InitializeInputDevices()
    {
        if (!RightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref RightController);
        if (!LeftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref LeftController);
    }

    void InitializeInputDevice(InputDeviceCharacteristics inputDeviceCharacteristics, ref UnityEngine.XR.InputDevice inputDevice)
    {
        List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }
}
