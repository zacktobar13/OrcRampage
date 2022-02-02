using UnityEngine;
using UnityEngine.SceneManagement;
public class SmoothTrackTarget : MonoBehaviour
{
	Transform target;
    Transform cursor;

	public float smoothSpeed;
    float cursorBiasX;
    float cursorBiasY;

    bool screenFadeDisableMouseBias = false;
    bool affixMenuDisableMouseBias = true;

    private void Awake()
    {
        AffixButton.onAffixChosen += EnableAffixMenuMouseBias;
        ChooseAffixMenu.onShowAffixMenu += DisableAffixMenuMouseBias;

        GameplayUI.onFadeCompleted += EnableScreenFadeMouseBias;
        WoodSignBehavior.onPlayerInteracted += DisableScreenFadeMouseBias;
    }

    private void Start()
    {
        cursor = GameObject.Find("Cursor").GetComponent<Transform>();
        target = PlayerManagement.player.transform;
        transform.position = new Vector3(target.position.x, target.position.y, -20f);
    }

    // Logic is placed in LateUpdate to ensure it occurs after player movement
    // which is located in Update.
    void LateUpdate ()
    {
        if (!target)
        {
            return;
        }

        if (screenFadeDisableMouseBias && affixMenuDisableMouseBias)
        {
            cursorBiasX = cursor.transform.position.x - target.transform.position.x;
            cursorBiasY = cursor.transform.position.y - target.transform.position.y;
        }

		// Create a reference to our current position and our target goal.
		Vector3 currentPosition = transform.position;
		Vector3 desiredPosition = target.position;

		// Calculating interpolation of X axis between our current and target values.
		currentPosition.x = transform.position.x + (cursorBiasX * .005f);
		currentPosition.x = currentPosition.x + (smoothSpeed * (desiredPosition.x - currentPosition.x));

		// Calculating interpolation of Z axis between our current and target values.
		currentPosition.y = transform.position.y+ (cursorBiasY * .005f);
		currentPosition.y = currentPosition.y + (smoothSpeed * ((desiredPosition.y) - currentPosition.y));

		// Applying our calculated interpolation to our camera transform.
		transform.position = currentPosition;
	}

    // Screen Fade
    private void EnableScreenFadeMouseBias()
    {
        screenFadeDisableMouseBias = true;
    }

    private void EnableScreenFadeMouseBias(BaseAffix affix)
    {
        screenFadeDisableMouseBias = true;
    }
    private void DisableScreenFadeMouseBias()
    {
        screenFadeDisableMouseBias = false;
    }

    // Affix Menu
    private void EnableAffixMenuMouseBias()
    {
        affixMenuDisableMouseBias = true;
    }

    private void EnableAffixMenuMouseBias(BaseAffix affix)
    {
        affixMenuDisableMouseBias = true;
    }

    private void DisableAffixMenuMouseBias()
    {
        affixMenuDisableMouseBias = false;
    }

    private void OnDestroy()
    {
        AffixButton.onAffixChosen -= EnableAffixMenuMouseBias;
        ChooseAffixMenu.onShowAffixMenu -= DisableAffixMenuMouseBias;

        WoodSignBehavior.onPlayerInteracted -= DisableScreenFadeMouseBias;
        GameplayUI.onFadeCompleted -= EnableScreenFadeMouseBias;
    }
}
