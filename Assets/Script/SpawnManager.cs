using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    private int pivot = 0;

    public Transform[] spawnPosArray;
    public GameObject[] zombie;

    private int level = 3;
    // Start is called before the first frame update
    void Start()
    {
        gameObjects = new GameObject[500]; ///오브젝트 풀링 기법
        for (int i = 0; i < 500; i++)
        {
            GameObject gameObject = Instantiate(zombie[Random.Range(0, 9)], spawnPosArray[Random.Range(0, 9)].position, Quaternion.identity);
            gameObjects[i] = gameObject;
            gameObject.SetActive(false);
        }
        StartCoroutine(SpawnZombie());
    }
    IEnumerator SpawnZombie()
    {
        yield return new WaitForSeconds(level);
        gameObjects[pivot++].SetActive(true);
        if (pivot == 500) pivot = 0;
        StartCoroutine(SpawnZombie());
        //for(int i=0; i<3; i++) 난이도 조절하는 라인
    }
}