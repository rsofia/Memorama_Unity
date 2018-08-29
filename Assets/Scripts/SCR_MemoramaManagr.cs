
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCR_MemoramaManagr : MonoBehaviour {

    public int gridSize = 4;
    public GameObject cardPrefab;
    public bool isGamePaused = true; //whether the game is currently on or not
    public TextMeshProUGUI txtScore;

    public UnityEngine.UI.GridLayoutGroup gridLayout; //component in charge of making the grid

    public Sprite[] possibilities; //Images to use

    private int score = 0; //Initial Score
    //List to save the currently turned cards
    private List<SCR_Card> currentTurnedCards = new List<SCR_Card>();
    //List to save all the cards
    private List<SCR_Card> allCards = new List<SCR_Card>();

    private void Start()
    {
        CreateGrid();
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
                int randomCard = 0;
                //Make sure we dont use the same pair twice
                do
                {
                    randomCard = Random.Range(0, possibilities.Length);
                    foreach (SCR_Card card in allCards)
                    {
                        if (card.front == possibilities[randomCard])
                        {
                            counter++;
                            if (counter == 2) //2 to make the pair
                            {
                                foundIt = false;
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
            //Check if shown cards arent a match (if they are a match, it's automatically check inside each MemoramaCard)
            if (currentTurnedCards[0].connection != currentTurnedCards[1])
            {
                RestLife();
                //Voltear las dos
                currentTurnedCards[0].Turn();
                currentTurnedCards[1].Turn();
                currentTurnedCards.Clear();
            }

#if UNITY_EDITOR
            else
                Debug.Log("1. Connections do match!");
#endif
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
        score += 1 * 10;
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

    }
    #endregion
}
