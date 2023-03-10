using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody rb;
    [Header("References")]
    private PlayerMovement move;
    [SerializeField] LayerMask deathMask;
    public bool isDead;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance;
    private GameObject Player;
    [SerializeField] Vector3 respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
        Player = GameObject.Find("Player");
        move = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        isDead = Physics.CheckSphere(groundCheck.position, groundDistance, deathMask);
    }

    public void Death()
    {
        if(isDead)
        {
            rb.position = respawnPoint;
        }
    }
}
