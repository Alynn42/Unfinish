using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variable : MonoBehaviour
{
    public bool Puzzle1Clear = false;
    public bool Puzzle2Clear = false;
    public bool Puzzle3Clear = false;
    public bool Puzzle4Clear = false;

    [SerializeField] public GameObject[] FirstRock;
    [SerializeField] public GameObject[] SecondRock;
    [SerializeField] public GameObject[] FirstGem;
    [SerializeField] public GameObject[] SecGem;
    [SerializeField] public GameObject[] TriGem;
    [SerializeField] public GameObject[] LastGem;

    private float moveSpeed = 2f;
    private float moveDistance = 3.48f;
    private float delaybetweenRock = 0.5f;
    private bool hasStarted = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Puzzle1Clear = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Puzzle2Clear = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Puzzle3Clear = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Puzzle4Clear = true;
        }

        if (Puzzle1Clear) GlowGems(FirstGem, Color.red);
        if (Puzzle2Clear) GlowGems(SecGem, Color.blue);
        if (Puzzle3Clear) GlowGems(TriGem, Color.magenta); // Purple
        if (Puzzle4Clear) GlowGems(LastGem, Color.green);

        if (Puzzle1Clear && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(RockDlay1());
        }
        if (Puzzle2Clear && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(RockDlay2());
        }
        
        
    }

    private IEnumerator RockDlay1()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < FirstRock.Length; i++)
        {
            StartCoroutine(MoveRock(FirstRock[i]));
            yield return new WaitForSeconds(delaybetweenRock);
        }
    }
    private IEnumerator RockDlay2()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < SecondRock.Length; i++)
        {
            StartCoroutine(MoveRock(SecondRock[i]));
            yield return new WaitForSeconds(delaybetweenRock);
        }
    }
    
    

    private IEnumerator MoveRock(GameObject rock)
    {
        float elapsedTime = 0f;
        Vector3 startPos = rock.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, moveDistance, 0);

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;
            rock.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime);
            yield return null;
        }
        rock.transform.position = targetPos;
    }
    

    private void GlowGems(GameObject[] gems, Color glowColor)
    {
        foreach (GameObject gem in gems)
        {
            if (gem != null)
            {
                Renderer gemRenderer = gem.GetComponent<Renderer>();
                if (gemRenderer != null)
                {
                    Material mat = gemRenderer.material;
                    mat.EnableKeyword("_EMISSION");
                    mat.SetColor("_EmissionColor", glowColor * 2f); // Adjust brightness
                }
            }
        }
    }
}
