using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{   
    public Rigidbody2D rb;
    public float ReleaseSpeed;
    void Start()
    {
        float direction = -transform.parent.localScale.y * ReleaseSpeed;
        rb.velocity = new Vector2(0f, direction);
    }
    public bool rangeMet = false;

    void OnTriggerEnter2D(Collider2D other) {
        Vector3 HitDirection = other.gameObject.transform.position - transform.position;
        HitDirection *= 1 / HitDirection.magnitude;
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<PlayerController>().TakeDamage(25, HitDirection);
        else if (other.gameObject.tag == "Enemies")
            other.gameObject.GetComponent<EnemyController>().TakeDamage(1, HitDirection);
        else if (other.gameObject.name == "Range")
        {
            rangeMet = true;
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(playAnim("End"));
        }
        else Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
    }

    IEnumerator playAnim(string AnimationCall)
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(AnimationCall);

        //Wait until Animator is done playing
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationCall) &&
        animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        rb.velocity = new Vector2(0, 0);
        Destroy(gameObject, 0.1f);

    }
}
