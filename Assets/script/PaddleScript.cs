using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PaddleScript : MonoBehaviour
{
    public float speed = 5;
    PhotonView pview;

    void Start()
    {
        spawner.player1exists = true;
        pview = this.gameObject.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pview.IsMine)
        {
            float verticalInput = Input.GetAxis("Vertical");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, verticalInput * speed);

            if (transform.position.y < -speed)
            {
                transform.position = new Vector2(transform.position.x, -speed);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (transform.position.y > speed)
            {
                transform.position = new Vector2(transform.position.x, speed);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
