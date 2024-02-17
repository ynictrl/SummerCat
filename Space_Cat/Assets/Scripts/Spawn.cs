using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] GO_enemy; //0 - asteroid, 1 - shooter
    public GameObject[] GO_item; //0 - fish
    public float timeSpw;
    public float minTimeSpw;
    public float maxTimeSpw;
    public bool isSpw; // start Game

    public bool spwBoss;
    public bool onBoss;
    public Transform wayBoss;

    // Start is called before the first frame update
    void Start()
    {
        minTimeSpw = 3f;
        maxTimeSpw = 5f;
    }

    // Update is called once per frame
    void Update()
    {       
        if(isSpw)
        {
            //porcentagens
            int percentageSpw = Random.Range(0, 100); 
            int percentageEnemySpw = Random.Range(0, 100); 
            int percentageItemSpw = Random.Range(0, 100); 

            timeSpw = Random.Range(minTimeSpw, maxTimeSpw);//(4f,10f)

            if(percentageSpw <= 75) // 75% enemy, 25% item
            {
                //ENEMY
                if(percentageEnemySpw <= 75) // 75% asteroid, 25% shooter
                {
                    StartCoroutine(SpawnerEnemy(0));
                }else{
                    StartCoroutine(SpawnerEnemy(1));
                }
            }else
            {
                //ITEM
                if(percentageItemSpw <= 90) // 90% coin, 10% heal
                {
                    StartCoroutine(SpawnerItem(0));
                }else{
                    StartCoroutine(SpawnerItem(1));
                } 
            }    
        }

        //velocidade de surgimento
        switch (Control.minutes)
        {
            case 0: minTimeSpw = 5f; maxTimeSpw = 8f; break;
            case 1: minTimeSpw = 4f; maxTimeSpw = 7f; break;
            case 2: minTimeSpw = 3f; maxTimeSpw = 6f; break;
            case 3: minTimeSpw = 2f; maxTimeSpw = 5f; break;
            case 4: minTimeSpw = 1f; maxTimeSpw = 4f; break;
            // boss
            case 5: 
                isSpw = false;
                if(spwBoss && Control.seconds >= 10f)
                {
                    SpawnBoss();
                } 
            break;
        } 
    }

    IEnumerator SpawnerEnemy(int i)
    {       
        if(isSpw)  
        {
            isSpw = false;
            yield return new WaitForSeconds(timeSpw);
            GameObject cloneObj = Instantiate(GO_enemy[i], transform.position, transform.rotation);
            isSpw = true;

            //cada minuto que passa os inimigos ganham velocidade
            switch (Control.minutes)
            {
                case 0: cloneObj.GetComponent<Enemy>().speed = -2.0f; break;
                case 1: cloneObj.GetComponent<Enemy>().speed = -2.5f; break;  
                case 2: cloneObj.GetComponent<Enemy>().speed = -3.0f; break; 
                case 3: cloneObj.GetComponent<Enemy>().speed = -3.5f; break; 
                case 4: cloneObj.GetComponent<Enemy>().speed = -4.0f; break;
                case 5: cloneObj.GetComponent<Enemy>().speed = -4.0f; isSpw = false; break;
            }       
        }
    }

    IEnumerator SpawnerItem(int i)
    {         
        if(isSpw)
        {
            isSpw = false;
            yield return new WaitForSeconds(timeSpw);
            GameObject cloneObj = Instantiate(GO_item[i], transform.position, transform.rotation);
            isSpw = true;

            switch (Control.minutes)
            {
                case 0: cloneObj.GetComponent<Item>().speed = -1.5f; break;
                case 1: cloneObj.GetComponent<Item>().speed = -2.0f; break; 
                case 2: cloneObj.GetComponent<Item>().speed = -2.5f; break;
                case 3: cloneObj.GetComponent<Item>().speed = -3.0f; break;
                case 4: cloneObj.GetComponent<Item>().speed = -3.5f; break;
                case 5: cloneObj.GetComponent<Item>().speed = -3.5f; isSpw = false; break;
            } 
        }
    }

    void SpawnBoss() //boss indo até o ponto
    {
        //onBoss = true;
        if(!onBoss)
        {
            GameObject cloneObj = Instantiate(GO_enemy[2], transform.position, transform.rotation);
            cloneObj.GetComponent<Enemy>().wb = wayBoss;
            onBoss = true;
        }
    }
}
