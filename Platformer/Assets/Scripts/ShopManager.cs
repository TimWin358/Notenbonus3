using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public int kontostand;
    public Text kontostandText;
    // public ItemBehaviour currentItem;

    // Start is called before the first frame update
    void Start()
    {
        // Kontostand in Textfeld eintragen
        KontostandAktualisieren();
    }

    public void ItemKauf(int itemPreis)
    {
        // Kontostand überprüfen
        if(kontostand >= itemPreis)
        {
            switch (itemPreis)
            {
                case 20:
                    // Item in ItemBehaviour auf True setzen
                    PlayerPrefs.SetInt("Jump", 1);
                    break;
                case 30:
                    PlayerPrefs.SetInt("Dash", 1);
                    break;
                case 50:
                    PlayerPrefs.SetInt("Gleiten", 1);
                    break;
                case 100:
                    PlayerPrefs.SetInt("Teleport", 1);
                    break;
            }

            // Kontostand aktualisieren
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") - itemPreis);
            kontostand = kontostand - itemPreis;

            // Kontostand Textfeld aktualisieren
            KontostandAktualisieren();
        }
    }

    void KontostandAktualisieren()
    {
        kontostand = PlayerPrefs.GetInt("Score", 0);

        // Kontostand in Textfeld eintragen
        kontostandText.text = kontostand.ToString();
    }
}
