using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public static float movementHorizontal = 0f;
    public static float movementVertical = 0f;
    public static Vector2 mousePosition;
    public static bool pressedSpacebar;
    public static bool attack;
    public static bool interact;

    Camera mainCamera;

    private void Start ()
    {
        mainCamera = Camera.main;
    }

    void Update () {
        movementHorizontal = Input.GetAxis("Horizontal");
        movementVertical = Input.GetAxis("Vertical");
        mousePosition = mainCamera.ScreenToWorldPoint ( Input.mousePosition );
        pressedSpacebar = Input.GetButtonDown("Dodge Roll");
        attack = Input.GetButtonDown("Fire1");
        interact = Input.GetKeyDown(KeyCode.E);
    }
}
