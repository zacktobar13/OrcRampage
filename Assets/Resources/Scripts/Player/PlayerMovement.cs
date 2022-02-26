using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float dodgeRollSpeedScalar;
    public bool isRunning = false;

    // TODO: Maybe make this a separate variable for each potential case that wants to stop shooting,
    //       that way multiple places won't be setting the same variable
    public bool movementEnabled = true;
    public bool gettingKnockedBack = false;
    public bool isDodgeRolling = false;
    public Vector2 knockbackDir;
    public Vector2 dodgeRollDir;
    public float knockbackDuration;
    public float knockbackDistanceMultiplier;
    public GameObject[] weapons;
    PlayerAttack playerAttack;
    public Rigidbody2D rigidBody;

    Vector2 fallRecoveryPoint;
    Vector2 knockbackEndPos;
    float currentSpeed;
    bool firstKnockback = true;
    float knockbackTime;

    TimeManager timeManager;

    private void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        SceneManager.sceneLoaded += ResetPositionOnNewLevel;
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();
    }

    private void FixedUpdate()
    {
        if ( gettingKnockedBack )
        {
            if ( firstKnockback )
            {
                currentSpeed = movementSpeed;
                knockbackTime = 0;
                firstKnockback = false;
            }

            knockbackTime += 1;
            currentSpeed = Utility.EaseOutQuad(currentSpeed, 0, knockbackTime / (60 * knockbackDuration));
            currentSpeed *= knockbackDistanceMultiplier;
            rigidBody.MovePosition(rigidBody.position + knockbackDir * currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            firstKnockback = true;
        }
    }

    void Update()
    {
        if (timeManager.IsGamePaused())
            return;

        if (!movementEnabled)
        {
            isRunning = false;
            return;
        }
        else if (gettingKnockedBack)
        {
            return;
        }

        Vector3 verticalMovement = Vector3.up * PlayerInput.movementVertical;
        Vector3 horizontalMovement = Vector3.right * PlayerInput.movementHorizontal;
        Vector2 movement = verticalMovement + horizontalMovement;
        movement = movement.normalized;

        // Dodge roll detection.
        if (PlayerInput.pressedSpacebar)
        {
            if (!isDodgeRolling)
            {
                if (movement == Vector2.zero)
                {
                    movement = (PlayerInput.mousePosition - (Vector2)transform.position).normalized / 2;
                }
                StartDodgeRoll(movement);
            }
        }

        // Dodge roll movement.
        if (isDodgeRolling)
        {
            rigidBody.position += (dodgeRollDir * Time.deltaTime * (movementSpeed * dodgeRollSpeedScalar * .8f));
            return;
        }
        else if (PlayerInput.movementVertical != 0 || PlayerInput.movementHorizontal != 0)
        {

            if (!isRunning)
            {
                isRunning = true;
            }
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
            }

        }

        rigidBody.position += movement * Time.deltaTime * movementSpeed;
    }

    public void MovePlayer(Vector2 position)
    {
        transform.position = position;
    }

    public void StartDodgeRoll(Vector2 direction)
    {
        playerAttack.enabled = false;
        dodgeRollDir = direction;
        isDodgeRolling = true;

        playerAttack.DisableWeapon();
    }

    public void StopDodgeRoll()
    {
        playerAttack.EnableWeapon();

        dodgeRollDir = Vector2.zero;
        playerAttack.enabled = true;
        isDodgeRolling = false;
        isRunning = false;
        dodgeRollSpeedScalar = 2;
    }

    public void ReduceDodgeRollSpeed()
    {
        dodgeRollSpeedScalar = 1;
    }

    public void ResetPositionOnNewLevel(Scene scene, LoadSceneMode sceneLoadMode)
    {
        transform.position = GameObject.Find("Level Entry Point").transform.position;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetPositionOnNewLevel;
    }

    private void OnDisable()
    {
        if (isDodgeRolling)
            StopDodgeRoll();
    }
}