using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTiles();
    }

    private void GenerateTiles()
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
                int random = Random.Range(0, 10);
                Vector3 tilePosition = new Vector3((width/2 + tileWidth/2) - (i * tileWidth),-yPosistion, (height/2 + tileHeight/2) - (y * tileHeight));
                GameObject tile = Instantiate(tilePrefab, ground.transform.position + tilePosition, Quaternion.identity, transform);
                //tile.GetComponent<Renderer>().enabled = false;
                if (random >= 8)
                {
                    tile.GetComponent<TileCost>().tileCost = 1000;
                    tile.GetComponent<Renderer>().material.color = Color.red; 
                    Instantiate(wallPrefab, tile.transform.position + new Vector3(0, wallPrefab.GetComponent<Renderer>().bounds.size.y/2, 0), Quaternion.identity, tile.transform);
                }
                else
                {
                    tile.GetComponent<TileCost>().tileCost = 1;
                }
                tile.tag = "Tile";
            }
        }
    }
}
