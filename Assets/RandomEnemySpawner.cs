using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEnemySpawner : MonoBehaviour {

    public List<GameObject> monsters;

    void Start()
    {
        StartCoroutine(SpawnMonster());
    }

    public IEnumerator SpawnMonster()
    {
        float delay = Random.Range(10f, 30f);

        yield return new WaitForSeconds(delay);

        int monsterToSpawn = Random.Range(0, monsters.Count);

        GameObject monster = Instantiate(monsters[monsterToSpawn], transform.GetChild(0).transform.position, Quaternion.identity) as GameObject;

        monster.GetComponent<Stats>().currentHealth = 2;

        monster.GetComponent<EnemyAI>().detectDistance = 10;

        StartCoroutine(SpawnMonster());
    }
}
