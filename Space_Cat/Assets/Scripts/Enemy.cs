using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int typeEnemy; // 0: asteroid/ 1: atirador/ 2: boss

    [Header("Components")]
    private Rigidbody2D rb2d;
    public GameObject GO_sprite;

    [Header("Skills")]
    public float speed;
    public float health;
    public float maxHealth;
    public float damageContact;

    [Header("Animation")]
    //public float speedRotation;
    public int numAnim; // 0 normar, 1 crack, 2 destroyed
    public bool boolAnim; //hit

    [Header("Shooter")]
    public Transform spawnBullet;
    public float fireRate;
    float nextFire;
    public bool onFire;
    public GameObject bulletObject;
    public float speedBullet;

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
    void Update()
    {
        
        if(health >= maxHealth)
        {
            health = maxHealth;
        }
        
        if(health <= 0)
        {
            if(typeEnemy == 2)//boss
            {
                Control.youWin = true;
            }

            Destroy(gameObject, .5f);
            onFire =  false;
            GetComponent<CircleCollider2D>().enabled = false;    
        }

        Anim();

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
        GO_sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        GetComponent<CircleCollider2D>().enabled = false;
        boolAnim = true; //hitshooter
        yield return new WaitForSeconds(0.1f);
        GO_sprite.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        GetComponent<CircleCollider2D>().enabled = true;
        boolAnim = false;
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
            cloneBullet.GetComponent<Bullet>().speed = speedBullet;
        }
    }

    public void MoveBoss()//chefão
    {
        if((Vector2.Distance(transform.position, wb.position) > 0))
        {
           transform.position = Vector2.MoveTowards(transform.position, wb.position, speed * Time.deltaTime);
        }  
    }

    public void Anim()
    {
        switch (typeEnemy)
        {
            case 0:
                //rotação do asteroid
                GO_sprite.transform.RotateAround(GO_sprite.transform.position, Vector3.forward, 99 * Time.deltaTime);

                GetComponent<Animator>().SetInteger("transition", numAnim);

                if((health <= (maxHealth/2)) && (health > 0))//rachadura
                {
                    numAnim = 1;
                }
                if(health <= 0) //destruido
                {
                    numAnim = 2;
                }

            break;
            case 1: 

                Vector3 targetPos = player.transform.position;
                Vector2 direction = new Vector2(targetPos.x  - GO_sprite.transform.position.x, targetPos.y - GO_sprite.transform.position.y);

                GetComponent<Animator>().SetInteger("transition", numAnim);
                GetComponent<Animator>().SetBool("hit", boolAnim);
                //quando acima de player true
                if(health > 0)
                {
                    if(this.transform.position.y >= player.transform.position.y)
                    {
                        numAnim = 0;
                        GO_sprite.transform.up = direction * -1;
                    }else{
                        numAnim = 1;
                        GO_sprite.transform.up = direction;
                    }
                }else{
                    numAnim = 2;
                }

            break;
        }    
    }             
}
