using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitcher : MonoBehaviour
{
    public Camera firstPersonCamera;
    public Camera secondCamera;
    public Camera thirdCamera;
    public Camera fourthCamera;
    public Image fadeScreen;
    public bool switchToSecondCamera;
    public bool switchToThirdCamera;
    public bool switchToFourthCamera;

    private bool isSwitched = false;

    void Start()
    {
        fadeScreen.color = new Color(0, 0, 0, 0);

        firstPersonCamera.enabled = true;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        fourthCamera.enabled = false;
    }

    void Update()
    {
        if (switchToSecondCamera && !isSwitched)
        {
            StartCoroutine(SwitchCameras(secondCamera));
        }
        else if (switchToThirdCamera && !isSwitched)
        {
            StartCoroutine(SwitchCameras(thirdCamera));
        }
        else if (switchToFourthCamera && !isSwitched)
        {
            StartCoroutine(SwitchCameras(fourthCamera));
        }
    }

    IEnumerator SwitchCameras(Camera targetCamera)
    {
        isSwitched = true;
        yield return StartCoroutine(FadeToBlack());

        firstPersonCamera.enabled = false;
        secondCamera.enabled = false;
        thirdCamera.enabled = false;
        fourthCamera.enabled = false;

        targetCamera.enabled = true;

        yield return StartCoroutine(FadeFromBlack());

        yield return new WaitForSeconds(5);

        yield return StartCoroutine(FadeToBlack());

        targetCamera.enabled = false;
        firstPersonCamera.enabled = true;

        yield return StartCoroutine(FadeFromBlack());

        isSwitched = false;
        switchToSecondCamera = false;
        switchToThirdCamera = false;
        switchToFourthCamera = false;
    }

    IEnumerator FadeToBlack()
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            fadeScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }
        fadeScreen.color = new Color(0, 0, 0, 1);
    }

    IEnumerator FadeFromBlack()
    {
        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            fadeScreen.color = new Color(0, 0, 0, t);
            yield return null;
        }
        fadeScreen.color = new Color(0, 0, 0, 0);
    }
}
