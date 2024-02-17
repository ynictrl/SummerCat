using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Status")]
    public bool enemyBullet;

    [Header("Components")]
    private GameObject player;
    private Rigidbody2D rb2d;
    public GameObject GO_sprite;

    [Header("Skills")]
    public float speed;
    public float timeDestroy;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
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

    void DeadBullet() // quando bullet colidir em um objeto
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GO_sprite.SetActive(false);
        Destroy(gameObject, .5f);
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
                player.GetComponent<Player>().UpdateRage(+0.25f);
                DeadBullet();
            }
  
        }else
        {
            //ENEMY BULLET
            // tocando no player
            if(other.gameObject.tag == "Player") 
            {
                other.gameObject.GetComponent<Player>().Hit(damage);
                DeadBullet();
            }
        }

        // tocando na parede
        if(other.gameObject.tag == "Wall") 
        {
            DeadBullet();
        }       
    }
}
