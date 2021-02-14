using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public int kontostand;
    public Text kontostandText;
    public ItemBehaviour currentItem;

    // Start is called before the first frame update
    void Start()
    {
        // Kontostand in Textfeld eintragen
        KontostandAktualisieren();
    }

    public void ItemKauf(int itemPreis, string item)
    {
        // Kontostand überprüfen
        if(kontostand >= itemPreis)
        {
            // Kontostand aktualisieren
            kontostand = kontostand - itemPreis;

            // Item in ItemBehaviour auf True setzen
            currentItem.item = true;

            // Kontostand Textfeld aktualisieren
            KontostandAktualisieren();
        }
    }

    void KontostandAktualisieren()
    {
        // Kontostand in Textfeld eintragen
        kontostandText.text = kontostand.ToString();
    }
}
