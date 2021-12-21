using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerExperience : MonoBehaviour
{
    // This is a workaround so other game objects can just call
    // the static function instead of needing to get a reference to player experience
    static PlayerExperience playerExperience;

    Image experienceBar;

    public int playerLevel = 1;
    public int currentExperience;
    public int levelExperience;

    public delegate void OnLevelUp();
    public static event OnLevelUp onLevelUp;

    GameObject floatingExperienceNumber;
    TextMeshProUGUI playerLevelText;
    TextMeshProUGUI experienceText;


    private void Awake()
    {
        experienceBar = GameObject.Find("Gameplay UI/Experience Bar").GetComponent<Image>();
        playerLevelText = GameObject.Find("Gameplay UI/Player Level Text").GetComponent<TextMeshProUGUI>();
        experienceText = GameObject.Find("Gameplay UI/Experience Text").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        playerExperience = gameObject.GetComponent<PlayerExperience>();
        experienceBar.fillAmount = (float)currentExperience / (float)levelExperience;
        floatingExperienceNumber = StaticResources.floatingExperienceNumber;

        playerLevelText.SetText("LVL: " + playerLevel.ToString());
        experienceText.SetText(currentExperience.ToString() + "/" + levelExperience.ToString());
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerLevelText = GameObject.Find("Player Level Text").GetComponent<TextMeshProUGUI>();
        experienceText = GameObject.Find("Experience Text").GetComponent<TextMeshProUGUI>();
        experienceBar = GameObject.Find("Experience Bar").GetComponent<Image>();
        experienceBar.fillAmount = (float)currentExperience / (float)levelExperience;
        playerLevelText.SetText("LVL: " + playerLevel.ToString());
        experienceText.SetText(currentExperience.ToString() + "/" + levelExperience.ToString());
    }

    public static void GiveExperience(int amount)
    {
        playerExperience.GiveExperience_Internal(amount);
    }

    public static int GetPlayerLevel()
    {
        return playerExperience.playerLevel;
    }

    private void GiveExperience_Internal(int amount)
    {
        currentExperience += amount;
        GameObject experienceNumber = Instantiate(floatingExperienceNumber, new Vector2(transform.position.x, transform.position.y + 7f), transform.rotation);
        experienceNumber.SendMessage("SetNumber", ("+ " + amount + " XP"));

        if (currentExperience >= levelExperience)
            LevelUp();

        experienceBar.fillAmount = (float)currentExperience / (float)levelExperience;
        experienceText.SetText(currentExperience.ToString() + "/" + levelExperience.ToString());
    }

    void LevelUp()
    {
        Debug.Assert(currentExperience >= levelExperience);

        if (onLevelUp != null)
            onLevelUp();

        playerLevel += 1;
        playerLevelText.SetText("LVL: " + playerLevel.ToString());
        int experienceAfterLevel = currentExperience - levelExperience;
        currentExperience = experienceAfterLevel;
        levelExperience = (int)(levelExperience * 1.15f);
    }
}
