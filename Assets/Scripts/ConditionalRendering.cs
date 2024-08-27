using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalRendering : MonoBehaviour
{
    public GameObject mayor;
    public GameObject tent;
    public GameObject hamper;
    public GameObject abbzLights;
    public GameObject brokenGenerator;
    public GameObject sutreMayor;
    public GameObject atmosphere;
    public GameObject Oliver;
    public GameObject shopSparePart;
    public GameObject shopCatFood;
    public GameObject shopTent;
    public GameObject hamperTranslator;
    public GameObject hamperHarmonica;
    public GameObject hamperSparePart;

    private bool hideOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mayor != null) {
            if (PlayerData.mayorStatus == "discovered" || PlayerData.mayorStatus == "saved")
            {
                mayor.SetActive(true);
            }
            else
            {
                mayor.SetActive(false);
            }
        }

        if (tent != null) {
            if (PlayerData.hamperState == "happy")
            {
                tent.SetActive(true);
            }
            else
            {
                tent.SetActive(false);
            }
        }
        if (hamper != null) {
            if (PlayerData.hamperState == "happy")
            {
                hamper.SetActive(false);
            }
            else
            {
                hamper.SetActive(true);
            }
        }


        if (abbzLights != null) {
            if (PlayerData.generatorFixed)
            {
                abbzLights.SetActive(true);
            }
            else
            {
                abbzLights.SetActive(false);
            }
        }

        if (brokenGenerator != null) {
            if (PlayerData.generatorFixed)
            {
                brokenGenerator.SetActive(false);
            }
            else
            {
                brokenGenerator.SetActive(true);
            }
        }

        if (sutreMayor != null) {
            if (PlayerData.mayorStatus == "saved")
            {
                sutreMayor.SetActive(false);
            }
            else if (PlayerData.mayorStatus == "discovered")
            {
                sutreMayor.SetActive(true);
            }
        }

        if (atmosphere != null) {
            if (PlayerData.atmosphereDestroyed)
            {
                atmosphere.SetActive(false);
            }
            else
            {
                atmosphere.SetActive(true);
            }
        }

        if (Oliver != null) {
            if (!PlayerData.generatorFixed && PlayerData.abbzState == "neutral" && PlayerData.talkedWithOliverAfterAbbz)
            {
                Oliver.SetActive(true);
                hideOnce = true;
            }
            else
            {
                Oliver.SetActive(false);

                if (hideOnce) {
                    GameObject.Find("SceneScripts").GetComponent<Tooltip>().Hide();
                    hideOnce = false;
                }
            }
        }

        if (shopSparePart != null) {
            if (PlayerData.shopSparePart)
            {
                shopSparePart.SetActive(true);
            }
            else
            {
                shopSparePart.SetActive(false);
            }
        }

        if (shopCatFood != null) {
            if (PlayerData.shopCatFood)
            {
                shopCatFood.SetActive(true);
            }
            else
            {
                shopCatFood.SetActive(false);
            }
        }

        if (shopTent != null) {
            if (PlayerData.shopTent)
            {
                shopTent.SetActive(true);
            }
            else
            {
                shopTent.SetActive(false);
            }
        }

        if (hamperTranslator != null) {
            if (PlayerData.hamperTranslator)
            {
                hamperTranslator.SetActive(true);
            }
            else
            {
                hamperTranslator.SetActive(false);
            }
        }

        if (hamperHarmonica != null) {
            if (PlayerData.hamperHarmonica)
            {
                hamperHarmonica.SetActive(true);
            }
            else
            {
                hamperHarmonica.SetActive(false);
            }
        }

        if (hamperSparePart != null) {
            if (PlayerData.hamperSparePart)
            {
                hamperSparePart.SetActive(true);
            }
            else
            {
                hamperSparePart.SetActive(false);
            }
        }

    }
}
