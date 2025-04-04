using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public LayerMask blockingLayer;
    public bool onGoal => goalCount > 0;

    private bool isMoving = false;
    private Material boxMaterial;
    private int goalCount = 0;

    private void Start()
    {
        boxMaterial = GetComponent<Renderer>().material;
    }

    public bool TryToPushBox(Vector3 direction, float speed)
    {
        if (isMoving) return false;

        var targetPosition = transform.position + direction;


        if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, blockingLayer))
        {
            StartCoroutine(MoveToPosition(targetPosition, speed));
            return true;
        }
        return false;
    }

    private IEnumerator MoveToPosition(Vector3 target, float speed)
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        isMoving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("Box reached the Goal");

            goalCount++;

            boxMaterial.color = GameManager.HighlightColor;
            GameManager.instance.StartCheckWin();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Debug.Log("Box left the Goal");

            goalCount--;
            if (goalCount <= 0)
            { 
                goalCount = 0;
                boxMaterial.color = GameManager.NormalColor;
            }
            else
            {
                boxMaterial.color = GameManager.HighlightColor;
            }
           
        }
    }
}

