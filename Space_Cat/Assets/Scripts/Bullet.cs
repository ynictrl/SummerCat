using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Status")]
    public bool enemyBullet;

    [Header("Components")]
    private Rigidbody2D rb2d;
    public SpriteRenderer sprite;

    [Header("Skills")]
    public float speed;
    public float timeDestroy;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Shot();
    }

    //saindo em disparada
    void Shot()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        Destroy(gameObject, timeDestroy);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(!enemyBullet)
        {
            //PLAYER BULLET
            // tocando no inimigo
            if(other.gameObject.tag == "Enemy") 
            {
                other.gameObject.GetComponent<Enemy>().Hit(damage);
                Destroy(gameObject);
            }
  
        }else
        {
            //ENEMY BULLET
            // tocando no player
            if(other.gameObject.tag == "Player") 
            {
                other.gameObject.GetComponent<Player>().Hit(damage);
                Destroy(gameObject);
            }
        }

        // tocando na parede
        if(other.gameObject.tag == "Wall") 
        {
            Destroy(gameObject, 0.25f);
        }       
    }
}
