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
    public Color32 InitialColor;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
        InitialColor = gameObject.transform.GetComponentInChildren<Renderer>().material.color;
        }
        catch
        {
            InitialColor = new Color32(0, 0, 0, 0);
        }
        Name = gameObject.name;
        currentHealth=initialHealth;
        HealthBarSlider.value=CalculateHealth();
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarSlider.value=CalculateHealth();
        if(currentHealth<=0 ) {
            if(GetComponent<PhotonView>())
            {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            if(gameObject.transform.GetParentComponent<EnamySpawnPoint>())
            {
                gameObject.transform.GetParentComponent<EnamySpawnPoint>().CardMonster = null;
            }
            this.GameManager.EnamyCards.Remove(this.gameObject);
            Destroy(gameObject);

            }
            else
            {
                var removeIndex = this.GameManager.ActiveCardList.FindIndex(x => x.Name == Name);
                if(removeIndex != -1)
                {
                    this.GameManager.ActiveCardList.RemoveAt(removeIndex);
                    this.GameManager.DestroyedCardList.Add(this.Name);
                    Destroy(gameObject);
                }

            }
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

    public void ResetAttack()
    {
        gameObject.transform.GetComponentInChildren<Renderer>().material.color = InitialColor;
        HasAttacked = false;
    }

    private void OnMouseDown()
    {
        if(GetComponent<PhotonView>())
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetEnamySelectedCard(this);
        }
        else if(HasAttacked == false)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().SetFrendlySelectedCard(this);
        }
    }

}
