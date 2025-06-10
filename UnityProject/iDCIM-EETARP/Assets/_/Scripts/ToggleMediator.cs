using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class ToggleMediator : MonoBehaviour
{
    public UnityEvent<bool> invokeEvent;
    [Foldout("[Event] On/Off")]
    public UnityEvent invokeIsOn;
    [Foldout("[Event] On/Off")]
    public UnityEvent invokeIsOff;
    
    public bool IsOn
    {
        set
        {
           if(value) invokeIsOn?.Invoke();
           else invokeIsOff?.Invoke();
           invokeEvent?.Invoke(value);
        }
    }
}