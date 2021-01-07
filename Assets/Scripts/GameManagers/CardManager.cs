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
    public GameManager GameManager;
    public bool HasAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        Name = gameObject.name;
        currentHealth=initialHealth;
        HealthBarSlider.value=CalculateHealth();
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
            this.GameManager.DestroyedCardList.Add(this.Name);
            this.GameManager.ActiveCardList.RemoveAt(this.GameManager.ActiveCardList.FindIndex(x => x.Name == this.Name));
            Destroy(gameObject);
        }
        if(GameManager.EnamyMarker != null)
        {
            transform.LookAt(GameManager.EnamyMarker.transform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
    float CalculateHealth(){
        return (float)currentHealth/initialHealth;
    }

    private void DisableAttackAnimation()
    {
        Debug.Log("dsaD");
        GetComponent<Animator>().SetBool("Attack", false);
    }

    public void DisableAnimation()
    {
        Invoke("DisableAttackAnimation", 100);
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
