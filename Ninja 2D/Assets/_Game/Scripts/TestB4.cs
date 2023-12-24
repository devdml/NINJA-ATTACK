using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

  class CoinContrast{
    public static string nameCoin = "my coin";
}
public class TestB4 : MonoBehaviour
{
    [SerializeField] Button buttonB4;
    [SerializeField] GameObject coin;
    void Start()
    {
        buttonB4.onClick.AddListener(()=> OnButtonB4Click());
    }
    private void OnButtonB4Click(){
        for(int i=0; i< 3; i++){
            Invoke(nameof(Shoot), i*3);
        }
    }
    private void Shoot(){
        GameObject myGo = Instantiate(coin);
        myGo.name = CoinContrast.nameCoin;
        myGo.transform.position = new Vector3(-3,-3);
    }

    
}
