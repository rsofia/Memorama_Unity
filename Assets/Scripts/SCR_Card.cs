using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class SCR_Card : MonoBehaviour {
        public SCR_Card connection;
        public Sprite front;
        public Sprite back;

        private Image myImg;
        private Animator myAnim;
        private bool wasAMatch = false;
        private bool isHidden = true;
        public SCR_MemoramaManagr memoramaManager;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            myImg = GetComponent<Image>();
            memoramaManager = FindObjectOfType<SCR_MemoramaManagr>();
#if UNITY_EDITOR
            if (memoramaManager == null)
                Debug.LogError("HAY QUE AGREGAR MEMORAMA MANAGER");
#endif
            myAnim = GetComponent<Animator>();
            AssignConnectionToMyConnection();


            Hide();
        }

    public void Init(SCR_MemoramaManagr memo, Sprite front)
    {
        myImg = GetComponent<Image>();
        memoramaManager = memo;
        myAnim = GetComponent<Animator>();
        this.front = front;
        AssignConnectionToMyConnection();


        Hide();
    }

    public bool IsShowing()
        {
            return !isHidden;
        }

        //This function is in case the developer forgot to assing either connection
        private void AssignConnectionToMyConnection()
        {
            if (connection != null && connection.connection == null)
            {
                connection.connection = gameObject.GetComponent<SCR_Card>();
            }
        }

        public void Turn()
        {
            if (memoramaManager == null)
                memoramaManager = FindObjectOfType<SCR_MemoramaManagr>();
            if (!memoramaManager.isGamePaused)
            {
                myAnim.SetTrigger("turn");
            }
#if UNITY_EDITOR
            else
                Debug.Log("Game paused for some reason");
#endif
        }

        public void ChangeTurn()
        {
            if (isHidden)
                Show();
            else
                Hide();

        }

        private void Hide()
        {
            myImg.sprite = back;
            memoramaManager.RemoveACard(this);
            isHidden = true;
        }

        private void Show()
        {
            myImg.sprite = front;
            if (!memoramaManager.isGamePaused)
                memoramaManager.AddACard(this);
            isHidden = false;

            if (!memoramaManager.isGamePaused)
                StartCoroutine(WaitForCheck());

        }

        IEnumerator WaitForCheck()
        {
            yield return new WaitForEndOfFrame(); //Check if its connection is showing too
            if (connection != null && connection.connection != null)
            {
                if (connection.IsShowing() && memoramaManager.IsCardTurned(this) && memoramaManager.IsCardTurned(connection) && connection.gameObject.activeSelf)
                {
#if UNITY_EDITOR
                    Debug.Log("2. They were indeed a match.");
#endif
                    wasAMatch = true;
                    memoramaManager.RemoveACard(this);
                    memoramaManager.RemoveACard(connection);
                    MarkAsCompleted();
                }
            }
        }

        public void Match(SCR_Card con)
        {
            wasAMatch = true;
            memoramaManager.RemoveACard(this);
            memoramaManager.RemoveACard(con);
            connection = con;
            MarkAsCompleted();
        }

        public void MarkAsCompleted()
        {
            if (wasAMatch || connection.wasAMatch)
            {
#if UNITY_EDITOR
                Debug.Log("3. They were marked as a match!");
#endif
                StartCoroutine(WaitToMatch());
            }
            else
            {
                Debug.Log("3. They were NOT marked as a match!");
            }
        }

        IEnumerator WaitToMatch()
        {
            yield return new WaitForSeconds(0.85f);
            //add Score
            FindObjectOfType<SCR_MemoramaManagr>().AddScore();
            //destroy this and connection
            connection.gameObject.SetActive(false);
            gameObject.SetActive(false);
            memoramaManager.CheckIfGameWon();
        }
    
}
