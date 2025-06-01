using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject tilePrefab;
    
    [SerializeField] private GameObject enemies;
    private List<GameObject> tiles = new List<GameObject>();
    
    [SerializeField] private GameObject startEnemies;

    private bool start;
    private float arenaTimer = 4;
    private float animationTimer = 1;
    
    private GameManager gameManager;

    private GameObject timerHolder;
    private Text timerText;
    // Start is called before the first frame update
    void Start()
    {
        enemies.GetComponent<EndArena>().endArena.AddListener(RemoveTiles);
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        timerHolder = gameManager.arenaTimer;
        timerText = timerHolder.GetComponent<Text>();
    }

    public void GenerateTiles()
    {
        float width = ground.GetComponent<Renderer>().bounds.size.x;
        float height = ground.GetComponent<Renderer>().bounds.size.z;
        float tileWidth = tilePrefab.GetComponent<Renderer>().bounds.size.x;
        float tileHeight = tilePrefab.GetComponent<Renderer>().bounds.size.z;
        
        float yPosistion = tilePrefab.GetComponent<Renderer>().bounds.size.y / 2;
        
        float widthAmount = width / tileWidth;
        float heightAmount = height / tileHeight;

        for (int y = 1; y <= heightAmount; y++)
        {
            for (int i = 1; i <= widthAmount; i++)
            {
                Vector3 tilePosition = new Vector3((width/2 + tileWidth/2) - (i * tileWidth),-yPosistion, (height/2 + tileHeight/2) - (y * tileHeight));
                GameObject tile = Instantiate(tilePrefab, ground.transform.position + tilePosition, Quaternion.identity, transform);
                tiles.Add(tile);
                tile.GetComponent<Renderer>().enabled = false;
                tile.GetComponent<TileCost>().tileCost = 1;
                tile.tag = "Tile";
            }
        }

        start = true;
    }

    private void RemoveTiles()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
    }

    private void Update()
    {
        if (start)
        {
           Timer();
           TimerAnimation();
        }
    }

    private void Timer()
    {
        string text = "";
        arenaTimer -= Time.deltaTime;
        switch (arenaTimer)
        {
            case >3:
                text = "3";
                break;
            case >2:
                text = "2";
                break;
            case >1:
                text = "1";
                break;
            case >0:
                startEnemies.SetActive(true);
                text = "GO";
                break;
            case <0:
                text = "";
                start = false;
                break;
        }
        timerText.text = text;
    }

    private void TimerAnimation()
    {
        animationTimer -= Time.deltaTime;
        if (animationTimer <= 0)
        {
            LeanTween.rotateZ(timerHolder, 10, 0.1f).setLoopPingPong(1).setOnComplete(()=>timerHolder.transform.rotation = Quaternion.Euler(0, 0, 0));
            LeanTween.scale(timerHolder, new Vector3(1.5f, 1.5f, 1.5f), 0.1f).setLoopPingPong(1).setOnComplete(()=>timerHolder.transform.localScale = new Vector3(1, 1, 1));
            animationTimer = 1;
        }
    }
}
