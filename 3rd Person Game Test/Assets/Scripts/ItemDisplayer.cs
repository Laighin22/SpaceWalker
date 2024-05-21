using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayer : MonoBehaviour
{
    public int currentShipPartIndex = 0;

    public Store store;
    public Light spotLight;
    public AudioSource spotLightAudio;
    public AudioSource musicAudio;

    private AudioClip music;
    // Start is called before the first frame update
    void Start()
    {
        musicAudio = GetComponent<AudioSource>();
        music = Resources.Load<AudioClip>("Music/SUPERMARKET MUSIC Royalty-free - Bit Orchestra");
        musicAudio.clip = music;
        musicAudio.Play();
        if (spotLight!=null)
        {
            StartCoroutine(TurnOnLights());
        }
        currentShipPartIndex = 0;
        foreach (StoreItem part in store.StoreItems)
        {
            part.GameObject.SetActive(false);
        }

        store.StoreItems[currentShipPartIndex].GameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextItem()
    {
        store.StoreItems[currentShipPartIndex].GameObject.SetActive(false);
        currentShipPartIndex++;
        if (currentShipPartIndex == store.StoreItems.Count)
        {
            currentShipPartIndex = 0;
        }
        store.StoreItems[currentShipPartIndex].GameObject.SetActive(true);
    }

    public void PreviousItem()
    {
        store.StoreItems[currentShipPartIndex].GameObject.SetActive(false);
        currentShipPartIndex--;
        if (currentShipPartIndex < 0)
        {
            currentShipPartIndex = store.StoreItems.Count-1;
        }
        store.StoreItems[currentShipPartIndex].GameObject.SetActive(true);
    }

    public IEnumerator TurnOnLights()
    {
        AudioClip clip = Resources.Load<AudioClip>("AudioFiles/spotlight-sound-effect-epicsoundfx");
        spotLightAudio.clip = clip;
        spotLightAudio.Play();
        yield return new WaitForSeconds(0.2f);
        spotLight.gameObject.SetActive(true);
    }
}
