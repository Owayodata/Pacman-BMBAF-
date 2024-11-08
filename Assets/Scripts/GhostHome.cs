using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform inside;

    public Transform outside;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        StartCoroutine(ExitTransition());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition()
    {
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.GetComponent<Rigidbody2D>().isKinematic = true;
        ghost.movement.enabled = false;

       Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(inside.position, this.outside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
        ghost.GetComponent<Rigidbody2D>().isKinematic = false;
        ghost.movement.enabled = true;
    }
}
