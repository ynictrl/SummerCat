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

    [Header("Animation")]
    public int numAnim; // 0 idle, 1 hit

    [Header("Skills")]
    public float speed;
    public float fireRate;
    public float fireRateDefault;
    float nextFire;
    public bool onFire;
    public bool isFire;

    [Header("Health")]
    public Transform healthBar;
    public GameObject healthBarObject;
    Vector3 healthBarScale;
    float healthPorcent;
    public float healthCurrent;
    public float healthMax;

    [Header("Rage_Skill")]

    public bool onRage = true;
    public bool isRage;//habilidade de rajada
    public float timeRage;//duração da habilidade
    public float breakRage;//intervalo da habilidade
    public int priceRage;//preço da habilidade
    public float fireRateRage;

    [Header("Rage_Bar")]
    public Transform rageBar;
    public GameObject rageBarObject;
    Vector3 rageBarScale;
    float ragePorcent;
    public float rageCurrent;
    public float rageMax;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        //barra de vida
        healthBarScale = healthBar.localScale;
        healthPorcent = healthBarScale.x / healthMax;
        UpdateHealthBar();

        //barra de rage
        rageBarScale = rageBar.localScale;
        ragePorcent = rageBarScale.x / rageMax;
        UpdateRageBar();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Control.isPaused)
        {
            speed = 4.5f;
            Move();
            Anim();

            if(Time.time > nextFire && onFire)
            {
                Fire();
            }

            if(healthCurrent <= 0)//gameover
            {
                SceneManager.LoadScene("SampleScene");
                //Invoke("Realoadlevel", 3f);
            }

            if(Input.GetButtonDown("Jump") && !isRage && rageCurrent >= rageMax && onRage)
            {
                //Control.coins -= priceRage;
                rageCurrent = 0;
                UpdateRageBar();
                StartCoroutine(Raging());
            }
        }else{
            speed = 0;
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

    public void UpdateHealth(float value)//atualizando vida
    {
        healthCurrent += value;
        UpdateHealthBar();
    }
    public void UpdateRageBar()//atualização da barra de rage
    {
        rageBarScale.x = ragePorcent * rageCurrent;
        rageBar.localScale = rageBarScale;

        if(rageBar.localScale.x < 0)
        {
            rageBar.localScale = new Vector3(0, 5f, 1f);
        }
        if(rageCurrent >= rageMax)
        {
            rageCurrent = rageMax;
            rageBar.localScale = new Vector3(2f, 5f, 1f);
        }
    }

    public void UpdateRage(float value)//atualizando rage
    {
        rageCurrent += value;
        UpdateRageBar();
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

    public void Heal(float cure)//levando dano
    {
        UpdateHealth(cure);
        //healthBarObject.SetActive(true);
    }

    public void Hit(float dmg)//levando dano
    {
        UpdateHealth(-dmg);
        //healthBarObject.SetActive(true);
        StartCoroutine(Hitting());
    }

    IEnumerator Hitting()//tomando o tiro
    {
        numAnim = 1;
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        numAnim = 0;
        yield return new WaitForSeconds(0.1f);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public void Anim()
    {
        GetComponent<Animator>().SetInteger("transition", numAnim);
    }

}
