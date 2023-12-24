using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySign : MonoBehaviour
{
    public Enemy enemy;
  private void OnTriggerEnter2D(Collider2D collider){
    if(collider.tag == "Player"){
        enemy.SetTarget(collider.GetComponent<Characters>());
    }
  }
    private void OnTriggerExit2D(Collider2D collider){
    if(collider.tag == "Player"){
        enemy.SetTarget(null);
    }
  }
}
