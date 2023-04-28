using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmDrop : MonoBehaviour
{
    
    public Button confirmButton;
    public Button cancelButton;

    private UnityAction confirmAction;
    private UnityAction cancelAction;

    public void SetConfirmAction(UnityAction action)
    {
        confirmAction = action;
        confirmButton.onClick.AddListener(Confirm);
    }

    public void SetCancelAction(UnityAction action)
    {
        cancelAction = action;
        cancelButton.onClick.AddListener(Cancel);
    }

    private void Confirm()
    {
        confirmAction?.Invoke();
        Close();
    }

    private void Cancel()
    {
        cancelAction?.Invoke();
        Close();
    }

    private void Close()
    {
        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
