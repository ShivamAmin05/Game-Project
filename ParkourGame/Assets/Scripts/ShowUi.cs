using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUi : MonoBehaviour
{
    public GameObject uiObject;
    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider player) {
        if(player.gameObject.tag == "PlayerBody")
        {
            uiObject.SetActive(true);

        }
    }

    public void OnTriggerExit(Collider player)
    {
        if(player.gameObject.tag == "PlayerBody")
        {
            uiObject.SetActive(false);
            Destroy(uiObject);
            Destroy(gameObject);
        }
    }
}
