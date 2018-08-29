
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_MemoramaManagr : MonoBehaviour {

    [HideInInspector]
    public int gridSize = 4;
    public GameObject cardPrefab;
    public bool isGamePaused = true; //whether the game is currently on or not
    public TextMeshProUGUI txtScore;

    public UnityEngine.UI.GridLayoutGroup gridLayout; //component in charge of making the grid

    [Tooltip("Possible Images to use on memorama")]
    public Sprite[] possibilities; //Images to use

    [Tooltip("Game Over Window")]
    public GameObject gameOverPanel;
    [Tooltip("Show the player's score")]
    public SCR_StarSystem starSystem;


    private int score = 0; //Initial Score
    //List to save the currently turned cards
    private List<SCR_Card> currentTurnedCards = new List<SCR_Card>();
    //List to save all the cards
    private List<SCR_Card> allCards = new List<SCR_Card>();

    private void Start()
    {
        gameOverPanel.SetActive(false);
        CreateGrid();
        ShowCardsForAFewSeconds();
    }

    private void CreateGrid()
    {
        gridLayout.constraintCount = gridSize;
        bool foundIt = true;
        int counter = 0;
        for(int i = 0; i < gridSize; i++)
        {
            for(int j = 0; j < gridSize; j++)
            {
                GameObject temp = Instantiate(cardPrefab, gridLayout.transform);
                foundIt = true;
                int randomCard = 0;
                counter = 0;
                //Make sure we dont use the same pair twice
                do
                {
                    randomCard = Random.Range(0, possibilities.Length);
                    counter = 0;
                    foreach (SCR_Card card in allCards)
                    {
                        if (card.front == possibilities[randomCard])
                        {
                            counter++;
                            if (counter >= 2) //2 to make the pair
                            {
                                foundIt = false;
                                counter = 0;
                                break;
                            }
                        }
                    }
                } while (foundIt == false);
                temp.GetComponent<SCR_Card>().Init(this, possibilities[randomCard]);
                allCards.Add(temp.GetComponent<SCR_Card>());
            }
        }

    }

    private void RestLife()
    {

    }

    #region CARD DISPLAY
    public void AddACard(SCR_Card _card)
    {
        currentTurnedCards.Add(_card);
        if (currentTurnedCards.Count > 2)
        {
            SCR_Card firstCard = currentTurnedCards[0];
            currentTurnedCards.Remove(currentTurnedCards[0]);
            firstCard.Turn();
        }

        if (currentTurnedCards.Count == 2)
        {
            //Check if shown cards arent a match
            if (currentTurnedCards[0].front != currentTurnedCards[1].front)
            {
                RestLife();
                //Voltear las dos
                currentTurnedCards[0].Turn();
                currentTurnedCards[1].Turn();
                currentTurnedCards.Clear();
            }
            else
            {
                currentTurnedCards[0].FoundMatch(currentTurnedCards[1]);
            }
        }
    }

    public bool IsCardTurned(SCR_Card _card)
    {
        return currentTurnedCards.Find(x => _card);
    }
    public void RemoveACard(SCR_Card _card)
    {
        currentTurnedCards.Remove(_card);
    }

    #endregion

    #region SCORE
    public void AddScore()
    {
        score += 10;
        txtScore.text = "Score: " + score.ToString();
    }

    #endregion

    #region WIN LOOSE CONDITIONS
    public void CheckIfGameWon()
    {
        int activeCards = 0;
        foreach (SCR_Card card in allCards)
        {
            if (card.gameObject.activeSelf)
                activeCards++;
        }
        if (activeCards == 0)
        {
            GameOver("Game Won!", true);
            isGamePaused = true;
        }
    }

    private void GameOver(string _mssg, bool gameWon = false)
    {
        starSystem.FillStarsWithScore((score * 6)/80);
        gameOverPanel.SetActive(true);
    }
    #endregion

    private void ShowCardsForAFewSeconds()
    {
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        foreach(SCR_Card card in allCards)
        {
            card.Turn();
        }
        yield return new WaitForSeconds(4.0f);

        foreach (SCR_Card card in allCards)
        {
            card.Turn();
        }

        isGamePaused = false;

        gridLayout.enabled = false;
    }
}
