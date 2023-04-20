using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCard : MonoBehaviour {

    [SerializeField] private SceneController controller;
    [SerializeField] private GameObject Card_Back;

    private AudioSource cardFlipAudio;

    private void Awake()
    {
        cardFlipAudio = GetComponent<AudioSource>();
    }

    public void OnMouseDown()
    {
        Reveal();
    }

    private void Reveal()
    {
        if (Card_Back.activeSelf && controller.canReveal)
        {
            cardFlipAudio.Play();

            Card_Back.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Unreveal()
    {
        Card_Back.SetActive(true);
    }


}
