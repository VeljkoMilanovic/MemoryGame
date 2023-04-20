﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour {

    public const int gridRows = 2;
    public const int gridCols = 6;
    public const float offsetX = 3f;
    public const float offsetY = 5f;

    [SerializeField] private MainCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMeshProUGUI timePassedLabel;

    private float timePassed = 0f;

    private void Start()
    {
        PlaceCards();
    }

    private void Update()
    {
        TimePassed();
    }

    private void PlaceCards()
    {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5 };
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MainCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MainCard;
                }

                int index = j * gridCols + i;
                int id = numbers[index];
                card.ChangeSprite(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for(int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    private MainCard _firstRevealed;
    private MainCard _secondRevealed;

    private int _score = 0;
    private int _numOfTries;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI numOfTriesLabel;

    public bool canReveal
    {
        get { return _secondRevealed == null; }
    }

    public void CardRevealed(MainCard card)
    {
        if(_firstRevealed == null)
        {
            _firstRevealed = card;
        }
        else
        {
            _secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        if(_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;

            _numOfTries++;
            numOfTriesLabel.text = "Tries: " + _numOfTries;
        }
        else
        {
            _numOfTries++;
            numOfTriesLabel.text = "Tries: " + _numOfTries;

            yield return new WaitForSeconds(0.5f);

            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }

        _firstRevealed = null;
        _secondRevealed = null;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void TimePassed()
    {
        timePassed += Time.deltaTime;
        timePassedLabel.text = "Time: " + timePassed.ToString("F0") + "s";
    }
}
