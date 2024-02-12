using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Components")]// *organização*
    private Rigidbody2D rb2d;
    public SpriteRenderer sprite;
    public Transform spawnBullet;
    public GameObject bulletObject;

    [Header("Skills")]
    public float speed;
    public float fireRate;
    public float fireRateDefault;
    float nextFire;
    public bool isFire;

    [Header("Rage")]

    public bool onRage = true;
    public bool isRage;//habilidade de rajada
    public float timeRage;//duração da habilidade
    public float breakRage;//intervalo da habilidade
    public int priceRage;//preço da habilidade
    public float fireRateRage;

    [Header("Health")]
    public Transform healthBar;
    public GameObject healthBarObject;
    Vector3 healthBarScale;
    float healthPorcent;
    public float healthCurrent;
    public float healthMax;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        //barra de vida
        healthBarScale = healthBar.localScale;
        healthPorcent = healthBarScale.x / healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Time.time > nextFire)
        {
            Fire();
        }

        if(healthCurrent <= 0)//gameover
        {
            SceneManager.LoadScene("SampleScene");
        }

        if(Input.GetButtonDown("Jump") && !isRage && Control.coins >= priceRage && onRage)
        {
            Control.coins -= priceRage;
            StartCoroutine(Raging());
        }
    }

    public void UpdateHealthBar()//atualização da barra de vida
    {
        healthBarScale.x = healthPorcent * healthCurrent;
        healthBar.localScale = healthBarScale;

        if(healthBar.localScale.x < 0)
        {
            healthBar.localScale = new Vector3(0, 5f, 1f);
        }
        if(healthCurrent >= healthMax)
        {
            healthCurrent = healthMax;
            healthBar.localScale = new Vector3(2f, 5f, 1f);
        }
    }

    public void Move()//movimentação
    {
        float moviment_x = Input.GetAxisRaw("Horizontal");
        float moviment_y = Input.GetAxisRaw("Vertical");

        rb2d.velocity = new Vector2(moviment_x, moviment_y).normalized * speed;
    }

    public void Fire()//disparo
    {
        nextFire = Time.time + fireRate;
        GameObject cloneBullet = Instantiate(bulletObject, spawnBullet.position, spawnBullet.rotation);
        //StartCoroutine(FireBreak());
    }

    IEnumerator Raging()//habilidade
    {
        onRage = false;
        fireRate = fireRateRage;
        yield return new WaitForSeconds(timeRage);
        fireRate = fireRateDefault;
        isRage = false;
        yield return new WaitForSeconds(breakRage);
        onRage = true;
    }

    /**IEnumerator FireBreak()
    {   
        onFire = false;
        isFire = true;      
        //slow = true;
        yield return new WaitForSeconds (fireRate);
        isFire = false; 
        onFire = true;
    }*/

    public void Heal(float cure)//levando dano
    {
        healthCurrent += cure;
        UpdateHealthBar();
        //healthBarObject.SetActive(true);
    }

    public void Hit(float dmg)//levando dano
    {
        healthCurrent -= dmg;
        UpdateHealthBar();
        //healthBarObject.SetActive(true);
        StartCoroutine(Hitting());
    }

    IEnumerator Hitting()//tomando o tiro
    {
        sprite.color = new Color(1f, 0.365655f, 0.365655f);
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(0.9258013f, 0.9339623f, 0.365655f);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

}
