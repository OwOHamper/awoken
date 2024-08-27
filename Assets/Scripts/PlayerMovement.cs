using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private IDictionary<int, Vector2> objectPositions = new Dictionary<int, Vector2>();
    private IDictionary<int, Vector2> flowerPositions = new Dictionary<int, Vector2>();
    private int changeScene = -1;
    private string destinationDialog = null;

    private Vector3 mousePos;
    private Vector2 targetPos;
    private List<Vector2> targetPositions = new List<Vector2>();
    private List<GameObject> changeToRoads = new List<GameObject>();

    public GameObject currentRoad;
    public GameObject crossroadMainOne;
    public GameObject crossroadMainTwo;

    private Vector3 cameraTargetPos;
    private Vector3 previousPosition;

    private float cameraDepth;
    public float xBorderOffset = 4f;
    public float yBorderOffset = 2f;
    private float startTime;
    private float animTime = 0.4f;

    public RectTransform[] roads;

    private List<Bounds> yBorders = new List<Bounds>();
    private List<Bounds> xBorders = new List<Bounds>();

    private SpriteRenderer spriteRenderer = null;
    private Sprite[] moveAnimation = new Sprite[2];

    private flower wateringFlower;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerData.isMoving = false;

        // Setting up positions dictionary
        objectPositions.Add(2, new Vector2(-6.91f, 2.88f));
        objectPositions.Add(3, new Vector2(3f, -0.34f));
        objectPositions.Add(4, new Vector2(6.97f, 2.02f));
        objectPositions.Add(6, new Vector2(4.32f, -5.8f));
        objectPositions.Add(7, new Vector2(-0.74f, 6.08f));

        flowerPositions.Add(11, new Vector2(-4.82f, -0.05f));
        flowerPositions.Add(12, new Vector2(-3.02f, 4.96f));
        flowerPositions.Add(13, new Vector2(7.219f, 1.833f));
        flowerPositions.Add(14, new Vector2(3.52f, -5.55f));
        flowerPositions.Add(15, new Vector2(-1.63f, 5.45f));

        if (PlayerData.respawn)
        {
            PlayerData.respawn = true;
            transform.position = new Vector3(PlayerData.lastPos.x, PlayerData.lastPos.y, transform.position.z);
            Camera.main.transform.position = new Vector3(PlayerData.cameraLastPos.x, PlayerData.cameraLastPos.y, PlayerData.cameraLastPos.z);
        }


        previousPosition = transform.position;
        cameraDepth = Camera.main.transform.position.z;

        if(PlayerData.lastRoadIdx != -1)
        {
            currentRoad = roads[PlayerData.lastRoadIdx].gameObject;
            PlayerData.lastRoadIdx = -1;
        }

        targetPos = new Vector2(transform.position.x, transform.position.y);
        cameraTargetPos = Camera.main.transform.position;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        startTime = Time.time;

        for (int i = 0; i < 2; i++)
        {
            moveAnimation[i] = Resources.Load<Sprite>("Sprites/botRun_" + i);
        }
    }

    private void Start()
    {
        foreach (GameObject border in GameObject.FindGameObjectsWithTag("Border"))
        {
            SpriteRenderer renderer = border.GetComponent<SpriteRenderer>();
            if (border.transform.rotation.z % 180 == 0)
            {
                xBorders.Add(renderer.bounds);
            }
            else
            {
                yBorders.Add(renderer.bounds);
            }

        }
    }

    void RecountCamera(Vector2 position)
    {
        float minX = -Mathf.Infinity, minY = -Mathf.Infinity, maxX = Mathf.Infinity, maxY = Mathf.Infinity;
        foreach (Bounds borderPos in yBorders)
        {
            float startX = borderPos.min.x, endX = borderPos.max.x, y = borderPos.center.y;
            if (y > minY && startX <= position.x && position.x <= endX && position.y > y)
            {
                minY = y;
            }
            if (y < maxY && startX <= position.x && position.x <= endX && position.y < y)
            {
                maxY = y;
            }
        }
        foreach (Bounds borderPos in xBorders)
        {
            float startY = borderPos.min.y, endY = borderPos.max.y, x = borderPos.center.x;
            if (x > minX && startY <= position.y && position.y <= endY && position.x > x)
            {
                minX = x;
            }
            if (x < maxX && startY <= position.y && position.y <= endY && position.x < x)
            {
                maxX = x;
            }
        }

        maxX -= xBorderOffset;
        minX += xBorderOffset;
        maxY -= yBorderOffset;
        minY += yBorderOffset;
        float finalx, finaly;
        if (maxX <= minX)
        {
            finalx = (minX + maxX) / 2;
        } else
        {
            finalx = Mathf.Clamp(position.x, minX, maxX);
        }
        if (maxY <= minY)
        {
            finaly = (minY + maxY) / 2;
        } else
        {
            finaly = Mathf.Clamp(position.y, minY, maxY);
        }
        cameraTargetPos = new Vector3(finalx, finaly, cameraDepth);
    }

    void MoveToTargetPos() {
        if (new Vector2(transform.position.x, transform.position.y) == targetPos)
        {
            if (targetPositions.Count != 0)
            {
                targetPos = targetPositions[0];
                targetPos.y += 0.65f;
                targetPositions.RemoveAt(0);
                currentRoad = changeToRoads[0];
                changeToRoads.RemoveAt(0);
                RecountCamera(targetPos);
            }
            else if (changeScene != -1)
            {
                PlayerData.isMoving = false;
                int tempChangeScene = changeScene;
                changeScene = -1;
                PlayerData.lastPos = transform.position;
                PlayerData.respawn = true;
                PlayerData.lastRoadIdx = Array.IndexOf(roads, currentRoad.GetComponent<RectTransform>());
                PlayerData.cameraLastPos = Camera.main.transform.position;
                FindObjectOfType<LevelLoader>().LoadNextLevel(tempChangeScene);
            }
            else if (destinationDialog != null)
            {
                PlayerData.isMoving = false;
                string tempDialog = destinationDialog;
                destinationDialog = null;
                PlayerData.canMove = true;
                FindObjectOfType<ClickHandler>().runDialog(tempDialog);
            } else if (wateringFlower != null)
            {
                PlayerData.isMoving = false;
                flower tempFlower = wateringFlower;
                wateringFlower = null;
                PlayerData.canMove = true;
                tempFlower.WaterAfterMovement();
            }
            else
            {
                PlayerData.isMoving = false;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraTargetPos, moveSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previousPosition = transform.position;

        if (PlayerData.canMove && Input.GetMouseButton(1)) {

            ChoosePath(mousePos);
            RecountCamera(targetPos);
        };


        if ((targetPos.x < transform.position.x) && (transform.localScale.x >= 0) ||
           ((targetPos.x > transform.position.x) && (transform.localScale.x < 0)))
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        MoveToTargetPos();

        // is moving
        if (previousPosition != transform.position)
        {
            if (Time.time - startTime >= animTime)
            {
                spriteRenderer.sprite = spriteRenderer.sprite == moveAnimation[1] ? moveAnimation[0] : moveAnimation[1];
                startTime = Time.time;
            }
        }
        else
        {
            spriteRenderer.sprite = moveAnimation[0];
        }
    }


    public void ChoosePath(Vector3 position)
    {
        foreach (RectTransform road in roads)
        {
            Vector2 lp;
            if (position == mousePos)
                RectTransformUtility.ScreenPointToLocalPointInRectangle(road, Input.mousePosition, Camera.main, out lp);
            else
            {
                lp = road.InverseTransformPoint(position);
            }

            if (road.rect.Contains(lp))
            {
                PlayerData.isMoving = true;
                targetPositions.Clear();
                changeToRoads.Clear();
                if (road.gameObject == currentRoad)
                    targetPos = position;
                else
                {
                    if (road == roads[0] && currentRoad == roads[1].gameObject)
                    {
                        targetPos = crossroadMainOne.transform.position;
                    }
                    else if (road == roads[0] && currentRoad == roads[2].gameObject)
                    {
                        targetPos = crossroadMainTwo.transform.position;
                    }
                    else if (road == roads[1] && currentRoad == roads[0].gameObject)
                    {
                        targetPos = crossroadMainOne.transform.position;
                    }
                    else if (road == roads[2] && currentRoad == roads[0].gameObject)
                    {
                        targetPos = crossroadMainTwo.transform.position;
                    }
                    else if (road == roads[1] && currentRoad == roads[2].gameObject)
                    {
                        targetPos = crossroadMainTwo.transform.position;
                        targetPositions.Add(crossroadMainOne.transform.position);
                        changeToRoads.Add(roads[0].gameObject);
                    }
                    else if (road == roads[2] && currentRoad == roads[1].gameObject)
                    {
                        targetPos = crossroadMainOne.transform.position;
                        targetPositions.Add(crossroadMainTwo.transform.position);
                        changeToRoads.Add(roads[0].gameObject);
                    }
                    else
                    {
                        Debug.LogWarning("Didn't find the right case while choosing path!");
                    }
                    targetPositions.Add(position);
                    changeToRoads.Add(road.gameObject);
                }
                targetPos.y += 0.65f;
            }
        }
    }


    public void GoToBuilding(int sceneIdx, bool load = true, string dialog=null)
    {
        PlayerData.canMove = false;
        Vector2 position = objectPositions[sceneIdx];
        ChoosePath(position);
        RecountCamera(targetPos);
        if (load)
        {
            changeScene = sceneIdx;
        } else
        {
            destinationDialog = dialog;
        }

    }

    public void GoToFlower(int flowerIdx, flower fl)
    {
        PlayerData.canMove = false;
        Vector2 position = flowerPositions[flowerIdx+10];
        ChoosePath(position);
        RecountCamera(targetPos);
        wateringFlower = fl;
    }
}
