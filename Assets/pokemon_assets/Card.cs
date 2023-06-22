using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Element element;
    public Sprite front_image;
    public Sprite back_image;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = front_image;


    }
    public void clickCard(){
        if(sprite.sprite == front_image){
            sprite.sprite = back_image;
        }else if (sprite.sprite == back_image){
            sprite.sprite = front_image;
        }
    }
    void OnMouseDown (){
        clickCard();
    }

}
