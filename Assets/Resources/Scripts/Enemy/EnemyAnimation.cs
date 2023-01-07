using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerTools;

public class EnemyAnimation : MonoBehaviour
{
	[Header("Animations")]
	public AnimationClip idleAnimation;
	public AnimationClip movingAnimation;
	public AnimationClip hurtAnimation;
	public AnimationClip deathAnimation;

	protected SpriteAnim spriteAnim;
    StateMachine stateMachine;

	void Start()
    {
		spriteAnim = transform.Find("Sprite").GetComponent<SpriteAnim>();
		stateMachine = GetComponent<BaseEnemy>().GetStateMachine();
    }

    void FixedUpdate()
    {
        switch(stateMachine.GetCurrentState())
        {
            case EnemyState.IDLE:
                spriteAnim.Play(idleAnimation); break;
			case EnemyState.MOVING:
				spriteAnim.Play(movingAnimation); break;
			// TODO telegraphing animation will go here
			//case EnemyState.ATTACK:
			//	spriteAnim.Play(attackA); break;
			case EnemyState.HURT:
				spriteAnim.Play(hurtAnimation); break;
			case EnemyState.DEAD:
				spriteAnim.Play(deathAnimation); break;
		}
    }
}
