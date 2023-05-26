using UnityEngine;
using UnityEngine.UI;

public class NumberInputField : MonoBehaviour
{
    private InputField inputField;

    private void Start()
    {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        // Remove non-digit characters from the input field
        string newValue = string.Empty;
        foreach (char c in value)
        {
            if (char.IsDigit(c))
                newValue += c;
        }
        inputField.text = newValue;
    }
}

