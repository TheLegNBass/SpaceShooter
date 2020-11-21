using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text Health;
    PlayerScript player;

    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<Text>();
        player = FindObjectOfType<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Health.text = player.GetHealth().ToString();
    }
}
