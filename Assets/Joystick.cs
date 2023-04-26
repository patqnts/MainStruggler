using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    public float maxDistance = 50f; // maximum distance the joystick handle can move from the center
    public KeyCode upKey = KeyCode.W; // keyboard key to map to "up" axis
    public KeyCode downKey = KeyCode.S; // keyboard key to map to "down" axis
    public KeyCode leftKey = KeyCode.A; // keyboard key to map to "left" axis
    public KeyCode rightKey = KeyCode.D; // keyboard key to map to "right" axis

    private RectTransform handleTransform; // reference to joystick handle RectTransform
    private Vector2 joystickPosition = Vector2.zero; // current position of joystick handle
    private float joystickRadius; // radius of joystick handle movement area

    void Start()
    {
        handleTransform = transform.GetChild(0).GetComponent<RectTransform>();
        joystickRadius = GetComponent<RectTransform>().sizeDelta.x / 2 - handleTransform.sizeDelta.x / 2;
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(leftKey)) horizontal -= 1f;
        if (Input.GetKey(rightKey)) horizontal += 1f;
        if (Input.GetKey(downKey)) vertical -= 1f;
        if (Input.GetKey(upKey)) vertical += 1f;

        Vector2 direction = new Vector2(horizontal, vertical).normalized;
        joystickPosition = direction * Mathf.Min(direction.magnitude, 1f) * maxDistance;

        handleTransform.anchoredPosition = joystickPosition;
    }

    public float GetHorizontalAxis()
    {
        return joystickPosition.x / joystickRadius;
    }

    public float GetVerticalAxis()
    {
        return joystickPosition.y / joystickRadius;
    }
}


