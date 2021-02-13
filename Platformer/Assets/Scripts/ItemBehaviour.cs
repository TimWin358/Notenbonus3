using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{

    //Speichert ob der Spieler ein gewisses Item besitzt.
    //Die Variablen sind Public damit ein Shop Script diese einstellen kann.
    #region Items

    [Header("Items")]

    public bool Jump;
    public bool Dash;
    public bool Teleport;
    public bool Gleiten;

    //Man könnte es auch "public bool Jump, Dash, Teleport, Gleiten;" schreiben aber dann sieht es nichtmehr so schön im Editor aus :(

    #endregion

    //Merkt sich ob ein Item benutzt werden kann
    //Zum Beispiel: 1. canJump = true 2. Spieler benutzt Doppelsprung 3. canJump = false
    //4. Spieler landet wieder auf den Boden 5. canJump = true
    #region canItems?

    private bool canJump;
    private bool canDash;
    private bool canTeleport;
    private bool canGleiten;

    #endregion

    //Merkt sich Item Settings
    //Ich hab es jetzt einfach mal public gemacht, falls man es vielleicht upgraden kann oder so
    //Ansonsten [SerializedField] benutzen!
    #region ItemSettings

    [Header("Item Settings")]

    public float Jump_Height;
    public float Dash_Distance;
    public float Teleport_Distance;
    public float Gleiten_Time;
    public float JumpGleitDelay;

    //Kollision für Marker bitte ausmachen, Danke. :)
    public GameObject Marker;

    #endregion

    //Hilfsvariablen
    #region Hilfsvariablen

    //Siehe JumpOrGleiten
    private float gleitTimer = 0;

    //Siehe teleport()
    private bool markerOn = false;

    #endregion 

    //Variablen für den Player
    #region Player

    private GameObject player;
    private Rigidbody playerRigidbody;
    private PlayerBehaviour playerScript;

    #endregion

    //ACHTUNG: Funktioniert nur wenn Player auch den Tag "Player" hat UND
    //das Script zum kontrollieren des Players auch "PlayerBehaviour" heißt
    //FIX (falls nötig): FindWithTag(<AndererTagHier>), GetComponent<<AndererScriptNameHier>>
    private void Start()
    {
        //Setzt Player Variablen
        player = GameObject.FindWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerScript = player.GetComponent<PlayerBehaviour>();

        //Sorgt dafür das der Marker richtig positioniert ist
        Marker.transform.position = new Vector3(0, 1, 0);
        Marker.transform.parent = player.transform;
        Marker.SetActive(markerOn);
    }

    void Update()
    {

        //"LeftShift" -> Dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        { 
            dash(); 
        }

        //"F" -> Telepotieren
        if (Input.GetKeyDown(KeyCode.F))
        { 
            teleport(); 
        }

        //"Space" -> Gleiten/Jump
        JumpOrGleiten(KeyCode.Space);

        //Bewegt den Marker mit Maus Drehrad
        if (markerOn)
        {
            marker();
        }

        //Falls der Spieler wieder auf den Boden gelandet ist, können die Items wieder benutzt werden
        if (playerScript.Grounded()) 
        { 
            resetCan(); 
        }

    }

    private void jump()
    {

        //1. Hat man springen freigeschaltet?
        //2. Ist man in der Luft?
        //3. Kann man noch springen?
        if (Jump && !playerScript.Grounded() && canJump)
        {
            canJump = false;

            //Springt ähnlich wie beim PlayerBehaviour Script (setzt velocity nach oben)
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, Jump_Height, playerRigidbody.velocity.z);
        }
        
    }

    private void dash()
    {
        //1. Hat man dashen freigeschaltet?
        //2. Kann man noch dashen?
        if (Dash && canDash)
        {
            canDash = false;

            Vector3 vector;

            //Je nachdem welche Richtung unser Player grad drückt, dasht er in diese Richtung

            //Rechts
            if (Input.GetAxis(playerScript.inputSettings.SIDEWAYS_AXIS) > 0)
            {
                vector = player.transform.right;
            }
            //Links
            else if (Input.GetAxis(playerScript.inputSettings.SIDEWAYS_AXIS) < 0)
            {
                vector = -player.transform.right;
            }
            //Rückwärts
            else if (Input.GetAxis(playerScript.inputSettings.FORWARD_AXIS) < 0)
            {
                vector = -player.transform.forward;
            }
            //Ansonsten einfach nach vorne
            else
            {
                vector = player.transform.forward;
            }

            //Wendet eine Vorwärtskraft auf den Player an
            playerRigidbody.AddForce(vector * Dash_Distance, ForceMode.Impulse);
        }

    }

    private void teleport()
    {
        //1. Marker erscheint
        //2. Man teleportiert zum Marker und der Marker verschwindet wieder
        if (markerOn)
        {

            //1. Hat man telepotieren freigeschaltet?
            //2. Befindet man sich auf dem Boden?
            //3. Kann man noch teleportieren?
            if (Teleport && playerScript.Grounded() && canTeleport)
            {

                canTeleport = false;

                //Vector der vom Player zum Marker zeigt
                Vector3 PlayerToMarker = Marker.transform.position - player.transform.position;

                //Schießt einen Raycast vom Spieler zum Marker
                //Falls etwas dazwischen ist, passiert nichts
                //Ansonsten wird der Spieler teleportiert

                Ray ray = new Ray(player.transform.position, PlayerToMarker);

                if (!Physics.Raycast(ray, PlayerToMarker.magnitude))
                {
                    player.transform.position = Marker.transform.position;
                }
             
            }

            markerOn = false;
        }
        else
        {
            markerOn = true;
        }

        //Zeigt / Versteckt marker
        Marker.SetActive(markerOn);


    }

    private IEnumerator gleiten()
    {

        //1. Hat man gleiten freigeschaltet?
        //2. Kann man noch gleiten?
        if (Gleiten && canGleiten)
        {
            canGleiten = false;

            //Friert Y Position ein, somit gleitet der Player
            playerRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;

            yield return new WaitForSeconds(Gleiten_Time);

            //"Taut" Y Position wieder auf (schaltet Y wieder frei)
            playerRigidbody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        yield return null;
        
    }

    private void JumpOrGleiten(KeyCode code)
    {
        //Solange der Key gedrückt ist, zählen wir in gleitTimer
        //Wenn wir den Key für JumpGleitDelay Sekunden lang drücken dann gleiten wir
        //Ansonsten springt man (wenn man schon gegleitet hat, springt nicht mehr)

        if (Input.GetKey(code))
        {
            gleitTimer += Time.deltaTime;

            if (gleitTimer > JumpGleitDelay)
            {
                StartCoroutine(gleiten());
            }

        }

        else if (Input.GetKeyUp(code))
        {

            if (!(gleitTimer > JumpGleitDelay))
            {
                jump();
            }

            gleitTimer = 0;
        };

    }

    private void marker()
    {
        //Rechnet Findet zwischen Spieler und Marker
        float distance = Vector3.Distance(player.transform.position, Marker.transform.position);

        //Solange der Marker sich in Teleport_Distance befindet, kann es sich bewegen
        //(So wie distance berechnet wird, könnte man sich aber auch nach hinten teleportieren ^^* ups...)
        if (distance < Teleport_Distance)
        {
            Marker.transform.position += player.transform.forward.normalized * Input.GetAxis("Mouse ScrollWheel") * 10;
        }
        //Ansonsten wird es zum Spieler zurückgesetzt
        else
        {
            Marker.transform.position = player.transform.position - new Vector3(0, 0.8f, 0);
        }
    }

    //Setzt alle can? Variablen wieder auf true zurück
    private void resetCan()
    {
        canJump = canDash = canTeleport = canGleiten = true;
    }

   
}
