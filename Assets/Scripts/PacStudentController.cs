using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;
    public Animator animator;
    public List<Tilemap> tilemaps;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private bool isLerping = false;
    private KeyCode lastInput;
    private KeyCode currentInput;
    private float startTime;
    public ParticleSystem snowParticles;

    public float particleRate = 20.0f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isLerping)
        {
            if (Input.GetKey(KeyCode.W)) 
            {
                animator.SetBool("moveUp", true);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", false);
                TryMove(Vector3.up, KeyCode.W);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                animator.SetBool("moveUp", false);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveLeft", true);
                animator.SetBool("moveRight", false);
                TryMove(Vector3.left, KeyCode.A);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("moveUp", false);
                animator.SetBool("moveDown", true);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", false);
                TryMove(Vector3.down, KeyCode.S);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator.SetBool("moveUp", false);
                animator.SetBool("moveDown", false);
                animator.SetBool("moveLeft", false);
                animator.SetBool("moveRight", true);
                TryMove(Vector3.right, KeyCode.D);
            }

        }

        if (isLerping)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float journeyLength = Vector3.Distance(startingPosition, targetPosition);
            float journeyFraction = distanceCovered / journeyLength;

            transform.position = Vector3.Lerp(startingPosition, targetPosition, journeyFraction);

            if (journeyFraction >= 1.0f)
            {
                isLerping = false;
            }
        }
    }

    private void TryMove(Vector3 direction, KeyCode input)
    {
        Vector3 nextPosition = transform.position + direction;
        if (IsWalkable(nextPosition))
        {
            currentInput = input;
            targetPosition = nextPosition;
            startingPosition = transform.position;
            startTime = Time.time; // Store the starting time
            StartCoroutine(LerpToPosition());
        }
    }

    private bool IsWalkable(Vector3 position)
    {
        foreach (Tilemap map in tilemaps)
        {
            Vector3Int cellPosition = map.WorldToCell(position);
            TileBase tile = map.GetTile(cellPosition);

            if (tile != null && (tile.name != "FlashingFish" && tile.name != "Fish"))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator LerpToPosition()
    {
        isLerping = true;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / moveSpeed;
        var emission = snowParticles.emission;

        emission.rateOverTime = particleRate;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioClip);
        }

        while (Time.time - startTime < duration)
        {
            yield return null;
        }
        
        emission.rateOverTime = 0;
        isLerping = false;
    }
}
