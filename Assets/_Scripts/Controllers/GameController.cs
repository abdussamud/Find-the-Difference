using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Variables
    public static GameController gc;

    public bool gameOver;
    public bool inAnimation;
    public int level;
    public int findDifferences;
    public int totalDifferences;
    public float maxHealth = 100;
    public GameObject crossGO;
    public GameObject crossGO2;
    public GameObject[] completionGOs;
    public Level[] levels;
    public LayerMask wrongLayer;
    public LayerMask clickableLayer;

    private RaycastHit2D rayHit;
    private RaycastHit2D rayHit2;
    private float health;
    private Coroutine disablerRoutine;
    private const float DURATION = 1f;
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
        health = maxHealth;
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
        if (inAnimation) { return; }
        if (Input.GetMouseButtonUp(0) && !gameOver)
        {
            rayHit = Cast2DRay(clickableLayer);
            rayHit2 = Cast2DRay(wrongLayer);
            if (rayHit.collider && rayHit.collider.CompareTag("Clickable")
                && rayHit.collider.GetComponent<Clickable>().solved) { return; }
            if (rayHit.collider && rayHit.collider.CompareTag("Clickable"))
            {
                findDifferences++;
                DifferSolved(rayHit.collider.GetComponent<Clickable>());
                if (findDifferences >= totalDifferences)
                {
                    Gui.GameWon();
                    gameOver = true;
                }
            }
            else if (rayHit2.collider && rayHit2.collider.CompareTag("wrong"))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (rayHit2.transform.position.y < 0)
                {
                    crossGO2.transform.position = pos;
                    crossGO.transform.localPosition = crossGO2.transform.localPosition;
                }
                else
                {
                    crossGO.transform.position = pos;
                    crossGO2.transform.localPosition = crossGO.transform.localPosition;
                }
                Disabler(crossGO, crossGO2);
                health -= 10;
                if (health <= 0) { gameOver = true; }
                Gui.SetHealthBar(health);
            }
        }
    }
    #endregion

    #region Private Methods
    private RaycastHit2D Cast2DRay(LayerMask layer) => Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 90f, layer);

    private void Disabler(GameObject obj1, GameObject obj2)
    {
        hintIndex = 0;
        if (disablerRoutine != null)
        {
            StopCoroutine(disablerRoutine);
            disablerRoutine = null;
        }
        disablerRoutine = StartCoroutine(DisableObject(obj1, obj2));
    }

    private IEnumerator DisableObject(GameObject obj1, GameObject obj2)
    {
        inAnimation = true;
        for (int i = 0; i < 6; i++)
        {
            obj1.SetActive(i % 2 == 0);
            obj2.SetActive(i % 2 == 0);
            yield return new WaitForSeconds(0.2f);
        }
        inAnimation = false;
    }

    private IEnumerator CompletionRoutine(Transform endPos = null)
    {
        inAnimation = true;
        float progress = 0;
        float elapsedTime = 0;

        completionGOs[0].SetActive(true);
        completionGOs[1].SetActive(true);

        while (progress <= 1)
        {
            completionGOs[0].transform.position = Vector3.Lerp(completionGOs[0].transform.position,
                endPos.position, progress);
            completionGOs[1].transform.position = Vector3.Lerp(completionGOs[1].transform.position,
                endPos.position, progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / DURATION;
            yield return null;
        }

        completionGOs[0].transform.position = endPos.position;
        completionGOs[1].transform.position = endPos.position;

        Gui.ActivateDot(findDifferences);

        completionGOs[0].SetActive(false);
        completionGOs[1].SetActive(false);
        inAnimation = false;
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
        completionGOs[0].transform.position = upperClickable.transform.position;
        completionGOs[1].transform.position = lowerClickable.transform.position;
        Transform endPos = Gui.completionDot[findDifferences - 1].transform;
        _ = StartCoroutine(CompletionRoutine(endPos));
    }
    #endregion

    #region Public Methods
    int hintIndex = 0;
    public void ShowHint()
    {
        if (!levels[level].upperSimilar[hintIndex].solved)
        {
            Clickable hintUpper = levels[level].upperSimilar[hintIndex];
            Clickable hintLower = levels[level].lowerSimilar[hintIndex];
            Disabler(hintUpper.hintIndicator, hintLower.hintIndicator);
        }
        else
        {
            hintIndex++;
            ShowHint();
        }
    }
    #endregion
}

public class CameraOrthoSizeController : MonoBehaviour
{
    public Sprite targetSprite; // Assign the sprite you want to fit in the viewport

    private void Start()
    {
        AdjustOrthographicSize();
    }

    private void AdjustOrthographicSize()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null && targetSprite != null)
        {
            float targetSpriteWidth = targetSprite.bounds.size.x;
            float targetSpriteHeight = targetSprite.bounds.size.y;
            float targetSpriteAspect = targetSpriteWidth / targetSpriteHeight;

            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;

            float requiredOrthoSize = targetSpriteAspect >= mainCamera.aspect
                ? targetSpriteHeight / 2f
                : cameraWidth / (2f * mainCamera.aspect);

            mainCamera.orthographicSize = requiredOrthoSize;
        }
    }
}
