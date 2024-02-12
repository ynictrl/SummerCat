using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int typeEnemy; // 0: asteroid/ 1: atirador/ 2: boss

    [Header("Components")]
    private Rigidbody2D rb2d;
    public GameObject sprite;

    [Header("Skills")]
    public float speed;
    public float health;
    public float damageContact;

    //[Header("Asteroid")]
    //public float speedRotation;
    //public GameObject sprite;

    [Header("Shooter")]
    public Transform spawnBullet;
    public float fireRate;
    float nextFire;
    public bool onFire;
    public GameObject bulletObject;

    [Header("Boss")]
    public Transform wb;

    [Header("Other")]
    GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        switch (typeEnemy)
        {
            case 0: Move(); break;
            case 1: Move(); Shooter(); break;
            case 2: MoveBoss(); Shooter(); break;
        }
    }

    void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    IEnumerator Hitting()//tomando o tiro
    {
        sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void Hit(float dmg)//levndo dano
    {
        health -= dmg;
        StartCoroutine(Hitting());
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        // tocando no limite do mapa
        if(other.gameObject.tag == "Limit") 
        {
            Destroy(gameObject);
        }  

        // tocando no player
        if(other.gameObject.tag == "Player") 
        {
            other.gameObject.GetComponent<Player>().Hit(damageContact);
            //Destroy(gameObject);
        }

        if(typeEnemy == 1 || typeEnemy == 2)//se shoter
        {
            if(other.gameObject.tag == "Wall") 
            {
                onFire = false;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(typeEnemy == 1 || typeEnemy == 2)//se shotter
        {
            if(other.gameObject.tag == "Wall") 
            {
                onFire = true;
            }
        }
    }

    void Shooter()//atirador
    {
        //mirando no player
        Vector3 targetPos = player.transform.position;
        Vector2 direction = new Vector2(targetPos.x  - spawnBullet.position.x, targetPos.y - spawnBullet.position.y);
        spawnBullet.up = direction;

        //cadencia disparos
        if((Time.time > nextFire) && onFire) 
        {
            Fire();
        }

        void Fire()//disparo
        {
            nextFire = Time.time + fireRate;
            GameObject cloneBullet = Instantiate(bulletObject, spawnBullet.position, spawnBullet.rotation);
            //cloneBullet.GetComponent<Bullet>().speed = 5f;
        }
    }

    public void MoveBoss()//chefão
    {
        if((Vector2.Distance(transform.position, wb.position) > 0))
        {
           transform.position = Vector2.MoveTowards(transform.position, wb.position, speed * Time.deltaTime);
        }  
    }

    /*public void RotationAsteroid()
    {
        //Transform target = sprite.gameObject.transform;
        transform.RotateAround(this.transform.position, Vector3.forward, speedRotation * Time.deltaTime);
    }*/
                
}
