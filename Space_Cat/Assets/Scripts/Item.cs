using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int typeItem; //0-fish, 1-lif
    public float speed;
    public static int saveCoin;

    public GameObject GO_sprite;
    public GameObject collected;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
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
            //other.gameObject.GetComponent<Player>().Hit(damageContact);
            switch (typeItem)
            {
                case 0: Control.TakeCoin(); player.GetComponent<Player>().UpdateRage(+2); break;
                case 1: player.GetComponent<Player>().Heal(4); break;
            }
            GO_sprite.SetActive(false);
            collected.SetActive(true);
            Destroy(gameObject, .5f);
        }
    }
}
