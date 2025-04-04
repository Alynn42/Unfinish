using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraSwitcher cameraSwitcher; // Reference to CameraSwitcher

    public static Color NormalColor = new Color(88f / 255f, 47f / 255f, 14f / 255f, 1f);
    public static Color HighlightColor = new Color(214f / 255f, 245f / 255f, 96f / 255f, 1f);

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>(); // Find CameraSwitcher in the scene
    }

    public void StartCheckWin()
    {
        StartCoroutine(CheckWin());
    }

    private IEnumerator CheckWin()
    {
        int num = 0;
        var boxArray = GameObject.FindGameObjectsWithTag("Box");
        var goalArray = GameObject.FindGameObjectsWithTag("Goal");

        foreach (var box in boxArray)
        {
            if (box.GetComponent<BoxController>().onGoal)
            {
                num++;
            }
        }

        if (num == goalArray.Length)
        {
            Debug.Log("Level Complete!");
                yield return new WaitForSeconds(3f); // Wait 5 seconds for camera transition
                if (cameraSwitcher != null)
                {
                    cameraSwitcher.switchToSecondCamera = true; // Trigger camera switch
                }

            

            SceneManager.UnloadSceneAsync("In-game", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects); // Load the "In-game" scene
            
        }
    }
}
