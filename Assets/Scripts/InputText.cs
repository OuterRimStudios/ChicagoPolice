using UnityEngine;
using UnityEngine.Events;

public class InputText : MonoBehaviour
{
    public OVRInput.Button inputButton;
    public UnityEvent OnPressed;

    private void Update()
    {
        if (OVRInput.GetDown(inputButton))
            OnPressed?.Invoke();
    }
}
