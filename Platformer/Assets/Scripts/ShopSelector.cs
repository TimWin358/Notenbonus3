using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopSelector : MonoBehaviour
{

    // Verlinkung zum MainMenue Panel + Shop Panel
    //public GameObject mainMenuPanel;
    //public GameObject shopPanel;

    // Start is called before the first frame update
    void Start()
    {
        //mainMenuPanel.SetActive(true);
        //shopPanel.SetActive(false);
    }

    public void ShowShopPanel()
    {
        //shopPanel.SetActive(true);
        //mainMenuPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowMainMenu()
    {
        shopPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
