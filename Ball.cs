using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    public GameObject hEins, hZwei, hDrei;
    Rigidbody2D rb;
    bool aufschlagRechts = true;
    bool spielGestartet = true;

    public GameObject spielerLinks;
    public GameObject spielerRechts;
    float eingabeFaktor = 10;
    float eingabeFaktorCpu = 6;

    int punkteLinks = 0;
    int punkteRechts = 0;
    public Text punkteLinksAnzeige;
    public Text punkteRechtsAnzeige;
    public Text infoAnzeige;
    public GameObject Exit;
    public GameObject AnwendungStart;

    public AudioClip Aufschlag;
    public AudioClip Abprallen;
    public AudioClip Punkt;
    public AudioClip horizontaleBewegung;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HindernisseSetzen();
        //Debug.Log("Speed x: " + rb.velocity.x + "Speed Y:" + rb.velocity.y);
        infoAnzeige.text= "use the left-arrow key to start";
        Exit.SetActive(false);
        AnwendungStart.SetActive(false);
    }

   
    void Update()
    {
        if (rb.velocity.x > 0 && spielGestartet == true)
        {
            infoAnzeige.text = "";
            

            if (Input.GetKey(KeyCode.UpArrow))
            {
                float yNeu = spielerRechts.transform.position.y + eingabeFaktor * Time.deltaTime;
                if (yNeu > 3.5f) yNeu = 3.5f;
                spielerRechts.transform.position = new Vector3(8.44f, yNeu, 0);                      

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                float yNeu = spielerRechts.transform.position.y - eingabeFaktor * Time.deltaTime;
                if (yNeu < -3.5f) yNeu = -3.5f;
                spielerRechts.transform.position = new Vector3(8.44f, yNeu, 0);                       
               
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                spielerRechts.transform.Translate(-0.3f, 0, 0);
                /*
                 * if(spielerLinks.transform.position.x > -8.44f)                                         // manual control
                 * Invoke("SpielerLinksZurueck", 0.2f);
                */
                AudioSource.PlayClipAtPoint(horizontaleBewegung, transform.position);
                rb.velocity = new Vector2(rb.velocity.x * 1.02f, rb.velocity.y * 1.02f);
            }
        }

        else if(rb.velocity.x < 0 && spielGestartet == true)
        {
            infoAnzeige.text = "";
           // if(Input.GetKey (KeyCode.W))                                                              // manual control
           //{
                float yNeu = spielerLinks.transform.position.y + eingabeFaktorCpu * Time.deltaTime;
                if (yNeu > 3.5f) yNeu = 3.5f;
                // spielerLinks.transform.position = new Vector3(-8.44f, yNeu, 0);                      // manual control
                Vector3 oben = new Vector3(-8.44f, yNeu, 0);                                            // K.i.
           // }
            //else if (Input.GetKey(KeyCode.S))
            //{
                /*float*/yNeu = spielerLinks.transform.position.y - eingabeFaktorCpu * Time.deltaTime;    // manual control
                if (yNeu < -3.5f) yNeu = -3.5f;
                // spielerLinks.transform.position = new Vector3(-8.44f, yNeu, 0);                      // manual control
                Vector3 unten = new Vector3(-8.44f, yNeu, 0);                                           // K.i.
            //}
            /*
             * else if (Input.GetKeyDown(KeyCode.D))                                                    // manual control
            {
                spielerLinks.transform.Translate(0.3f, 0, 0);
                if (spielerRechts.transform.position.x < 8.44f)
                    Invoke("SpielerRechtsZurueck", 0.2f);
            }
            */
            if ((oben - transform.position).magnitude < (unten - transform.position).magnitude)         // K.i.
            {
                spielerLinks.transform.position = oben;
            }
            else
            {
                spielerLinks.transform.position = unten;
            }
        }

        else if (rb.velocity.x == 0 && spielGestartet == true)
        {
            if(aufschlagRechts && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                rb.AddForce(new Vector2(-200, 300));
                AudioSource.PlayClipAtPoint(Aufschlag, transform.position);
            }
            else if (!aufschlagRechts /*&& Input.GetKeyDown(KeyCode.D)*/)                           // manual control
            {
                rb.AddForce(new Vector2(200, 300));
                AudioSource.PlayClipAtPoint(Aufschlag, transform.position);
            }
        }
    }


    void HindernisseSetzen()
    {
        hEins.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-4.5f, 4.5f), 0);

        do
        {
            hZwei.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-4.5f, 4.5f), 0);
        }
        while ((hZwei.transform.position - hEins.transform.position).magnitude < 2);

        do
        {
            hDrei.transform.position = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-4.5f, 4.5f), 0);
        }
        while ((hDrei.transform.position - hEins.transform.position).magnitude < 2 
             ||(hDrei.transform.position - hZwei.transform.position).magnitude <2);
    }


   /* void SpielerRechtsZurueck()
    {
        spielerRechts.transform.Translate(0.3f, 0, 0);
    }
    
    /* 
    void SpielerLinksZurueck()                            // manual control (horizontal interaction)
    {
        spielerLinks.transform.Translate(-0.3f, 0, 0);
    } 
    */


    void OnCollisionEnter2D(Collision2D coll)
    {
       
            if (coll.gameObject.tag == "WandLinks" && spielGestartet == true)
            {
                AudioSource.PlayClipAtPoint(Punkt, transform.position);
                punkteRechts++;
                punkteRechtsAnzeige.text = punkteRechts + " /10";
                aufschlagRechts = true;
                gameObject.SetActive(false);

                Invoke("BallAufStartRechts", 1);
            }
            else if (coll.gameObject.tag == "WandRechts" && spielGestartet == true)
            {
                AudioSource.PlayClipAtPoint(Punkt, transform.position);
                punkteLinks++;
                punkteLinksAnzeige.text = punkteLinks + " /10";
                aufschlagRechts = false;
                gameObject.SetActive(false);

                Invoke("BallAufStartLinks", 1);
            }
            else if (coll.gameObject.tag == "Spieler")
            {
                rb.velocity = new Vector2(rb.velocity.x * 1.02f, rb.velocity.y * 1.02f);
                AudioSource.PlayClipAtPoint(Abprallen, transform.position);
            }
            else if (coll.gameObject.tag == "Hindernis")
            {
                AudioSource.PlayClipAtPoint(Abprallen, transform.position);
            }

        if (punkteRechts == 10)
        {
            infoAnzeige.text = "YOU WIN!!!";
            Exit.SetActive(true);
            AnwendungStart.SetActive(true);
            aufschlagRechts = true;

            punkteLinks = 0;
            punkteLinksAnzeige.text = "0";
            punkteRechts = 0;
            punkteRechtsAnzeige.text = "0";

            spielGestartet = false;

            //Invoke("BallAufStartRechts", 5);
        }
        else if (punkteLinks == 10)
        {
            infoAnzeige.text = "You LOOSE!!!";
            Exit.SetActive(true);
            AnwendungStart.SetActive(true);
            aufschlagRechts = true;

            punkteLinks = 0;
            punkteLinksAnzeige.text = "0";
            punkteRechts = 0;
            punkteRechtsAnzeige.text = "0";

            spielGestartet = false;

            //Invoke("BallAufStartRechts", 5);
        }
    }

    void BallAufStartRechts()
    {
        transform.position = new Vector3(7.99f, 0, 0);
        BallAufStart();
    }

    void BallAufStartLinks()
    {
        transform.position = new Vector3(-7.99f, 0, 0);
        BallAufStart();
    }

    void BallAufStart()
    {
        HindernisseSetzen();
        spielerLinks.transform.position = new Vector3(-8.44f, 0, 0);
        spielerRechts.transform.position = new Vector3(8.44f, 0, 0);
        gameObject.SetActive(true);
        rb.velocity = new Vector3(5.0f, 5.0f, 0);
    }

    public void Exit_Click()
    {
        Application.Quit();
    }

    public void AnwendungStart_Click()
    {
        spielGestartet = true;
        BallAufStartRechts();

        AnwendungStart.SetActive(false);
        Exit.SetActive(false);
    }
}
