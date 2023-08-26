using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    public static GameController gc;

    public bool gameOver;
    public int level;
    public int findDifferences;
    public int totalDifferences;
    public float health = 100, maxHealth = 100;
    public GameObject crossGO;
    public Level[] levels;
    public LayerMask wrongLayer;
    public LayerMask clickableLayer;
    private RaycastHit2D rayHit;
    private RaycastHit2D rayHit2;
    private Coroutine disablerRoutine;
    private GameplayUI Gui => GameplayUI.gui;
    private GameManager Gm => GameManager.Instance;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        gc = this;
    }

    private void Start()
    {
        level = Gm.level;
        levels[level].levelGO.SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            levels[level].upperSimilar[i].objectID = i;
            levels[level].lowerSimilar[i].objectID = i;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !gameOver)
        {
            rayHit = Cast2DRay(clickableLayer);
            rayHit2 = Cast2DRay(wrongLayer);
            if (rayHit.collider && rayHit.collider.CompareTag("Clickable") &&
                !rayHit.collider.GetComponent<Clickable>().solved)
            {
                DifferSolved(rayHit.collider.GetComponent<Clickable>());
                findDifferences++;
                Invoke(nameof(ActiavateCompletionDot), 0.5f);
                if (findDifferences >= totalDifferences)
                {
                    Gui.GameWon();
                    gameOver = true;
                }
            }
            else if (rayHit2.collider && rayHit2.collider.CompareTag("wrong"))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                crossGO.transform.position = pos;
                Disabler(crossGO);
                health -= 10;
                if (health <= 0) { gameOver = true; }
                Gui.SetHealthBar(health);
            }
        }
    }
    #endregion

    private RaycastHit2D Cast2DRay(LayerMask layer) => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, layer);

    private void Disabler(GameObject obj)
    {
        if (disablerRoutine != null)
        {
            StopCoroutine(disablerRoutine);
            disablerRoutine = null;
        }
        disablerRoutine = StartCoroutine(DisableObject(obj));
    }

    private IEnumerator DisableObject(GameObject obj)
    {
        for (int i = 0; i < 6; i++)
        {
            obj.SetActive(i % 2 == 0);
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void DifferSolved(Clickable clickable)
    {
        int objectID = clickable.objectID;
        Level level = levels[this.level];
        Clickable upperClickable = level.upperSimilar[objectID];
        Clickable lowerClickable = level.lowerSimilar[objectID];
        upperClickable.solved = true;
        lowerClickable.solved = true;
        upperClickable.differIndicator.SetActive(true);
        lowerClickable.differIndicator.SetActive(true);
    }
    private void ActiavateCompletionDot()
    {
        Gui.completionDot[findDifferences - 1].innerGO.SetActive(true);
    }
}
