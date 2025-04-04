using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController_Sokoban : MonoBehaviour
{
    private bool ismoving = false;

    public LayerMask blockingLayer;
    public float moveSpeed = 5f;


    void Update()
    {
        if (ismoving) return;

        var movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) movement = Vector3.forward;

        if (Input.GetKey(KeyCode.S)) movement = Vector3.back;

        if (Input.GetKey(KeyCode.A)) movement = Vector3.left;

        if (Input.GetKey(KeyCode.D)) movement = Vector3.right;

        if (movement != Vector3.zero)
        {
            TryToMove(movement);
        }
    }

    private void TryToMove(Vector3 direction)

    {

        var targetPosition = transform.position + direction;

        if (!Physics.Raycast(transform.position, direction, out RaycastHit hit, 1f, blockingLayer))
        {
            StartCoroutine(MoveToPosition(targetPosition));
        }

        else if (hit.collider.CompareTag("Box"))
        {
            var box = hit.collider.GetComponent<BoxController>();
            if (box != null && box.TryToPushBox(direction, moveSpeed))
            {
                StartCoroutine(MoveToPosition(targetPosition));
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target)

    {

        ismoving = true;

        while (Vector3.Distance(transform.position, target) > 0.01f)

        {
            transform.position = Vector3.MoveTowards(current: transform.position, target, moveSpeed * Time.deltaTime);

            yield return null;

        }
        transform.position = target;

        ismoving = false;
    }

}