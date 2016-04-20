using UnityEngine;
using System.Collections;

public class Stats_KingHellfireSlime : Stats {

    public override void Die()
    {
        dead = true;
        if (!deathSound == null)
        {
            GetComponent<AudioSource>().pitch = 1;
            AudioSource.PlayClipAtPoint(deathSound, transform.position, 1f);
        }

        float num = Random.Range(0f, 1f);
        if (transform.tag.Equals("Enemy")) GetComponent<EnemyAI>().Die();

        GameController.ChangePoints(pointValue);

        GameController.WinGame(); 
    }
}
