using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public GameObject hitVFX;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
       OnInit();
    }

    public void OnInit(){
            rb.velocity = transform.right * 5f;
            Invoke(nameof(OnDespawn), 4f);
    }
    public void OnDespawn(){
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag=="Enemy"){
            other.GetComponent<Characters>().OnHit(30f);
            Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }
}
