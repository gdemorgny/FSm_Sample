using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// déclaration de l'enum pour avoir des noms d'états lisibles
public enum StatesSample
{
    None,
    Idle,
    Jumping,
    Running
}

public class SampleFSM : MonoBehaviour
{
    //variable stockant l'état actuel
    public StatesSample state = StatesSample.None;
    //Variable utilisée pour un changement d'état
    public StatesSample nextState = StatesSample.None;

    //Variable utilisée pour savoir si le joueur est au sol pour évier le multi-jump
    public bool isOnTheGround = true;

    //Variable de déplacement pour régler les vitesses
    public float speed = 5f;
    public float jumpForce = 100f;

    //Dans le start on initialise le premier state
    private void Start()
    {
        state = StatesSample.Idle;
    }

    // dans l'ordre : on vérifie si transition a été demandée 
    // si oui on applique l'action de transition s'il y en a une
    // dans tous les cas on applique le comportement de l'état en cours
    private void Update()
    {
        if (CheckForTransition())
        {
            TransitionOrChangeState();
        }
        StateBehaviour();
    }

    // on fait un switch par état, chaque état n'accepte que ses propres transitions.
    private bool CheckForTransition()
    {
        switch(state)
        {
            case StatesSample.None:
                break;
            // l'état Idle accept bien le Start Jump et le start running
            case StatesSample.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    nextState = StatesSample.Jumping;
                    return true;
                } else if (Input.GetAxis("Horizontal") != 0)
                {
                    nextState = StatesSample.Running;
                    return true;
                }
                break;
            // le jumping n'accepte que le stop jumping
            case StatesSample.Jumping:
                if (isOnTheGround)
                {
                    nextState = StatesSample.Idle;
                    return true;
                }
                break;
            // le running n'accepte que le stop running.
            case StatesSample.Running:
                if (Input.GetAxis("Horizontal") == 0)
                {
                    nextState = StatesSample.Idle;
                    return true;
                }
                break;
        }

        return false;
    }

    // on applique les actions associées au transitions
    private void TransitionOrChangeState()
    {
        switch(nextState)
        {
            case StatesSample.None:
                break;
            case StatesSample.Idle:
                break;
            // il n'y a que le jumping qui a une action : le saut
            case StatesSample.Jumping:
                GetComponent<Rigidbody>().AddForce(0f,jumpForce,0f);
                isOnTheGround = false;
                break;
            case StatesSample.Running:
                break;
        }

        state = nextState;
    }

    // on applique le comportement des états
    private void StateBehaviour()
    {
        switch(state)
        {
            case StatesSample.None:
                break;
            case StatesSample.Idle:
                break;
            case StatesSample.Jumping:
                break;
            // seul le running a un comportement spécifique : le déplacement du joueur
            case StatesSample.Running:
                GetComponent<Rigidbody>().AddForce(Input.GetAxis("Horizontal")*speed,0f,0f);
                break;
        }
    }

    // fonction pour vérifier que le joureur est bien retombé au sol.
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isOnTheGround = true;
        }
    }
}
