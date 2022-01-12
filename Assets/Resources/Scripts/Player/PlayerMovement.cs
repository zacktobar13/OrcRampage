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
    PlayerAttack playerShoot;
    public Rigidbody2D rigidBody;

    Vector2 fallRecoveryPoint;
    Vector2 knockbackEndPos;
    float currentSpeed;
    bool firstKnockback = true;
    float knockbackTime;

    public delegate void OnPitFallRecovery();
    public static event OnPitFallRecovery onPitFallRecovery;

    public delegate void OnPitFalling();
    public static event OnPitFalling onPitFalling;

    private void Start()
    {
        playerShoot = GetComponent<PlayerAttack>();
        SceneManager.sceneLoaded += ResetPositionOnNewLevel;
        PitEdge.onPlayerEnterPitEdge += EnterPitEdge;
        PitHazard.onPlayerEnterPitHazard += FallIntoPit;
        PitHazard.onPlayerExitPitHazard += ExitPit;
    }

    // Slow down when drawing bow, and speed back up after it's fired
    void OnBowDraw(float time)
    {
        movementSpeed *= (1f / 3f);
    }

    void OnDrawEnd(PlayerAttack playerShoot)
    {
        movementSpeed *= 3f;
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
        playerShoot.enabled = false;
        dodgeRollDir = direction;
        isDodgeRolling = true;

        foreach (GameObject weapon in weapons)
            weapon.SetActive(false);
    }

    public void StopDodgeRoll()
    {
        foreach (GameObject weapon in weapons)
            weapon.SetActive(true);

        dodgeRollDir = Vector2.zero;
        playerShoot.enabled = true;
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
        PitEdge.onPlayerEnterPitEdge -= EnterPitEdge;
        PitHazard.onPlayerEnterPitHazard -= FallIntoPit;
        PitHazard.onPlayerExitPitHazard -= ExitPit;
    }

    private void OnDisable()
    {
        if (isDodgeRolling)
            StopDodgeRoll();
    }

    private void EnterPitEdge()
    {

    }

    private void ExitPit()
    {
        pitFallDodgeRollStart = Vector2.zero;
    }

    IEnumerator pitFall;
    public bool isFallingDownPit;
    Vector2 pitFallDodgeRollStart = Vector2.zero;
    private void FallIntoPit(Collider2D hazard, Collider2D outerEdge)
    {
        if (isDodgeRolling)
        {
            if (pitFallDodgeRollStart == Vector2.zero)
                pitFallDodgeRollStart = transform.position;
            return;
        }

        // we're already falling
        if (pitFall != null)
            return;

        isFallingDownPit = true;

        // Move player to start of dodge roll position so we can calculate where to put him after pit fall
        Vector3 oldPosition = transform.position;
        if (pitFallDodgeRollStart != Vector2.zero)
        {
            transform.position = pitFallDodgeRollStart;
        }
        pitFallDodgeRollStart = Vector2.zero;

        ColliderDistance2D distance = outerEdge.Distance(transform.Find("World Collider").GetComponent<Collider2D>());
        fallRecoveryPoint = distance.pointA;

        transform.position = oldPosition;

        if (onPitFalling != null)
            onPitFalling();

        pitFall = PitFallCoroutine();
        StartCoroutine(pitFall);
    }

    IEnumerator PitFallCoroutine()
    {
        PlayerManagement.TogglePlayerControl(false);
        yield return new WaitForSeconds(1f);
        PitRecovery();
        pitFall = null;
    }

    private void PitRecovery()
    {
        if ( onPitFallRecovery != null )
            onPitFallRecovery();

        transform.position = fallRecoveryPoint;
        isFallingDownPit = false;
        PlayerManagement.TogglePlayerControl(true);
    }
}