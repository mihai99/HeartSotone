using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public string Name;
    public int initialHealth;
    public int attack;
    public int mana;
    public int currentHealth;
    public Slider HealthBarSlider;
    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        currentHealth=initialHealth;
        HealthBarSlider.value=CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarSlider.value=CalculateHealth();
        if(currentHealth<=0){
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            if(gameObject.transform.GetParentComponent<EnamySpawnPoint>())
            {
                gameObject.transform.GetParentComponent<EnamySpawnPoint>().CardMonster = null;
            }
            Destroy(gameObject);
        }
    }
    float CalculateHealth(){
        return (float)currentHealth/initialHealth;
    }

    private void OnMouseDown()
    {
        if(GetComponent<PhotonView>())
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetEnamySelectedCard(this);
        } else
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetFrendlySelectedCard(this);
        }
    }
}
