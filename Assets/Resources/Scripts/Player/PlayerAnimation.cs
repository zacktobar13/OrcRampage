using UnityEngine;
using PowerTools;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public SpriteAnim spriteAnim;

    public AnimationClip idleAnimation;
    public AnimationClip runAnimation;
    public AnimationClip dodgeRollAnimation;
    public AnimationClip hurtAnimation;
    public AnimationClip deathAnimation;
    public AnimationClip aimIdleAnimation;
    public AnimationClip aimRunAnimation;
    public AnimationClip fallAnimation;

    public AudioSource audioSource;
    public AudioClip footstepAudioClip;
    public GameObject footstepDust;

    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    PlayerAttack playerAttack;
    public GameObject sprite;
    bool spriteFlipEnabled = false;

    bool scalingDown = false;
    bool fadingTransparency = false;
    float spriteAlpha = 1f;
    TimeManager timeManager;

    enum anim
    {
        idle,
        running,
        dodgeRoll,
        hurt,
        death,
        aimIdle,
        aimWalk,
        falling,
    }

    private bool playedIdleAnimation = false;
    private bool playedRunningAnimation = false;
    private bool playedHurtAnimation = false;
    private bool playedDodgeRollAnimation = false;
    private bool playedDeathAnimation = false;
    private bool playedAimIdleAnimation = false;
    private bool playedAimWalkAnimation = false;
    private bool playedFallingDownPitAnimation = false;
    private bool hurt = false;


    Vector3 mousePosition;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponentInChildren<PlayerAttack>();
        timeManager = GameObject.Find("Game Management").GetComponent<TimeManager>();

        TriggerToNextLevel.onCollideWithPlayer += PlayWalkIntoLevelAnimation;
        WoodSignBehavior.onPlayerInteracted += PlayInteractWithSignAnimation;
        WoodSignBehavior.onPlayerInteracted += DisableSpriteFlipping;
        LevelInfo.onEnterHubWorld += DisableSpriteFlipping;
        TriggerToNextLevel.onCollideWithPlayer += DisableSpriteFlipping;
        LevelInfo.onNewLevel += EnableSpriteFlipping;

        spriteFlipEnabled = true;
    }

    private void Update()
    {
        if (timeManager.IsGamePaused())
            return;

        if (playerHealth.health <= 0)
        {
            if (!playedDeathAnimation)
            {
                spriteAnim.Play(deathAnimation);
                MostRecentAnim(anim.death);
            }
        }
        else if (hurt)
        {
            if (!playedHurtAnimation)
            {
                spriteAnim.Play(hurtAnimation);
                MostRecentAnim(anim.hurt);
            }
        }
        else if (playerMovement.isDodgeRolling)
        {
            if (!playedDodgeRollAnimation)
            {
                spriteAnim.Play(dodgeRollAnimation);
                MostRecentAnim(anim.dodgeRoll);
            }
        }
        else if (playerMovement.isRunning)
        {
            if (!playedRunningAnimation)
            {
                spriteAnim.Play(runAnimation);
                MostRecentAnim(anim.running);
            }
        }
        else
        {
            if (!playedIdleAnimation)
            {
                spriteAnim.Play(idleAnimation);
                MostRecentAnim(anim.idle);
            }
        }

        mousePosition = PlayerInput.mousePosition;

        // Prevent sprite flipping when dead.
        if (playerHealth.isCurrentlyDead)
            return;

        if (!playerMovement.isDodgeRolling)
        {
            bool isMouseOnRightSide = mousePosition.x - gameObject.transform.position.x > 0;
            FlipSprites(!isMouseOnRightSide);
        }
        else
        {
            float horizontalMovement = playerMovement.dodgeRollDir.x;
            if (horizontalMovement == 0)
            {
                FlipSprites(mousePosition.x - gameObject.transform.position.x < 0);
            }
            else
            {
                FlipSprites(playerMovement.dodgeRollDir.x < 0);
            }
        }

        if (fadingTransparency)
        {
            Color spriteColor = new Color(spriteRenderer.material.color.r, spriteRenderer.material.color.g, spriteRenderer.material.color.b, spriteAlpha);
            spriteRenderer.material.color = spriteColor;
            spriteAlpha *= .95f;
        }

        if (scalingDown)
        {
            float tempX = sprite.transform.localScale.x;
            float tempY = sprite.transform.localScale.y;
            tempX *= .99f;
            tempY *= .99f;
            sprite.transform.localScale = new Vector2(tempX, tempY);
        }
    }

    void MostRecentAnim(anim current)
    {
        playedIdleAnimation = current == anim.idle;
        playedRunningAnimation = current == anim.running;
        playedDodgeRollAnimation = current == anim.dodgeRoll;
        playedHurtAnimation = current == anim.hurt;
        playedDeathAnimation = current == anim.death;
        playedAimIdleAnimation = current == anim.aimIdle;
        playedAimWalkAnimation = current == anim.aimWalk;
        playedFallingDownPitAnimation = current == anim.falling;
    }

    void FlipSprites(bool facingRight)
    {
        if (spriteFlipEnabled)
            spriteRenderer.flipX = facingRight;
    }

    public void FootStep()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(footstepAudioClip, 4f);
        Instantiate(footstepDust, transform.position, transform.rotation);
    }

    public void DustCloud()
    {
        Instantiate(footstepDust, transform.position, transform.rotation);
    }

    public void PlayIdleAnimation()
    {
        spriteAnim.Play(idleAnimation);
    }

    public void PlayRunAnimation()
    {
        spriteAnim.Play(runAnimation);
    }

    public void PlayDodgeRollAnimation()
    {
        spriteAnim.Play(dodgeRollAnimation);
    }

    public void PlayAimIdleAnimation()
    {
        spriteAnim.Play(aimIdleAnimation);
    }

    public void PlayAimRunAnimation()
    {
        spriteAnim.Play(aimRunAnimation);
    }

    public void PlayHurtAnimation()
    {
        spriteAnim.Play(hurtAnimation);
    }

    public void PlayDeathAnimation()
    {
        spriteAnim.Play(deathAnimation);
    }

    public void PlayFallAnimation()
    {
        spriteAnim.Play(fallAnimation);
    }

    public void PlayInteractWithSignAnimation()
    {
        spriteAnim.Play(idleAnimation);
    }

    public void PlayWalkIntoLevelAnimation()
    {
        spriteAnim.Play(idleAnimation);
    }

    public IEnumerator PlayHurtAnimation(Vector2 damageDir)
    {
        const float knockbackDuration = .3f;
        hurt = true;
        playerMovement.gettingKnockedBack = true;
        playerMovement.knockbackDir = damageDir;
        playerMovement.knockbackDuration = knockbackDuration;
        playerAttack.DisableWeapon();
        yield return new WaitForSeconds(knockbackDuration);
        playerAttack.EnableWeapon();
        playerMovement.gettingKnockedBack = false;
        hurt = false;
    }


    public void EnableSpriteFlipping()
    {
        spriteFlipEnabled = !spriteFlipEnabled;
    }

    public void EnableSpriteFlipping(BaseAffix affix)
    {
        spriteFlipEnabled = !spriteFlipEnabled;
    }

    public void DisableSpriteFlipping()
    {
        spriteFlipEnabled = !spriteFlipEnabled;
    }

    private void OnDestroy()
    {
        TriggerToNextLevel.onCollideWithPlayer -= PlayWalkIntoLevelAnimation;
        WoodSignBehavior.onPlayerInteracted -= PlayInteractWithSignAnimation;
        WoodSignBehavior.onPlayerInteracted -= DisableSpriteFlipping;
        LevelInfo.onEnterHubWorld -= DisableSpriteFlipping;
        TriggerToNextLevel.onCollideWithPlayer -= DisableSpriteFlipping;
    }
}
