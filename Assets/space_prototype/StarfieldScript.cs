using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldScript : MonoBehaviour {

    public int numOfStarsLayer1 = 1;
    public int numOfStarsLayer2 = 1;
    public int numOfStarsLayer3 = 1;
    public int starfieldHeight = 50;
    public int starfieldWidth = 50;
    public float layer2Speed = 1.2f;
    public float layer3Speed = 1.8f;
    public int starTimeout = 1;

    public GameObjectPooler starPool;
    public GameObject player;

    public List<GameObject> layer1;
    public List<GameObject> layer2;
    public List<GameObject> layer3;

    public Rect starfieldRect;
    public Vector3 starfieldOffset;
    public Vector3 lastPosition;
    public Vector3 deltaPosition;

    void Start () {
        //Get Pool
        starPool = GameObjectPooler.current;
 
        //Generate Rect for Starfield bounds
        starfieldRect.height = starfieldHeight;
        starfieldRect.width = starfieldWidth;

        starfieldOffset = new Vector3(starfieldRect.width / 2.0f, starfieldRect.height / 2.0f);

        GameObject obj;

        //Instantiate 3 star layers
        layer1 = new List<GameObject>();
        layer2 = new List<GameObject>();
        layer3 = new List<GameObject>();

        for (int i = 0; i < numOfStarsLayer1; i++)
        {
                obj = starPool.GetPooledObject();
                obj.transform.position = player.transform.position - new Vector3(-4f, 0f);
                obj.SetActive(true);
                layer1.Add(obj);
        }

        for (int i = 0; i < numOfStarsLayer2; i++)
        {
            obj = starPool.GetPooledObject();
            obj.transform.position = player.transform.position - new Vector3(-2f, 0f);
            obj.SetActive(true);
            layer2.Add(obj);
        }

        for (int i = 0; i < numOfStarsLayer3; i++)
        {
            obj = starPool.GetPooledObject();
            obj.transform.position = player.transform.position - new Vector3(-0f, 0f);
            obj.SetActive(true);
            layer3.Add(obj);
        }
    }


    void Update() {
        //Center starfield rect on player
        starfieldRect.position = player.transform.position - starfieldOffset;

        //Store player position delta
        deltaPosition = lastPosition - player.transform.position;
        lastPosition = player.transform.position;

        //Update 1st layer
        for (int i = layer1.Count - 1; i >= 0; i--)
        {
            layer1[i].transform.position += deltaPosition;
            StarLife script = layer1[i].GetComponent<StarLife>();

            if (!starfieldRect.Contains(layer1[i].transform.position))
            {
                script.timeOOB += Time.deltaTime;
            }
            else
            {
                script.timeOOB = 0;
            }

            if (script.timeOOB > 3)
            {
                layer1[i].SetActive(false);
                layer1.RemoveAt(i);
            }
        }
        

        //Update 2nd Layer
        for (int i = layer2.Count - 1; i >= 0; i--)
        {
            layer2[i].transform.position += (deltaPosition * layer2Speed);
            StarLife script = layer2[i].GetComponent<StarLife>();

            if (!starfieldRect.Contains(layer2[i].transform.position))
            {
                script.timeOOB += Time.deltaTime;
            }
            else
            {
                script.timeOOB = 0;
            }

            if (script.timeOOB > 3)
            {
                layer2[i].SetActive(false);
                layer2.RemoveAt(i);
                
            }
        }



        //Update 3rd Layer
        for (int i = layer3.Count - 1; i >= 0; i--)
        {
            layer3[i].transform.position += (deltaPosition * layer3Speed);
            StarLife script = layer3[i].GetComponent<StarLife>();

            if (!starfieldRect.Contains(layer3[i].transform.position))
            {
                script.timeOOB += Time.deltaTime;
            }
            else
            {
                script.timeOOB = 0;
            }

            if (script.timeOOB > starTimeout)
            {
                layer3[i].SetActive(false);
                layer3.RemoveAt(i);
            }
        }

        //Check number of stars in 1st layer and add more in direction of player movement
        if (layer1.Count < numOfStarsLayer1)
        {

        }

        //Check number of stars in 2nd layer and add more in direction of player movement
        if (layer2.Count < numOfStarsLayer2)
        {

        }

        //Check number of stars in 3rd layer and add more in direction of player movement
        if (layer3.Count < numOfStarsLayer3)
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        DrawRect(starfieldRect);
    }

    private void DrawRect(Rect r)
    {
        Vector3[] pts =
        {
            new Vector3(r.xMin, r.yMin, 0),
            new Vector3(r.xMax, r.yMin, 0),
            new Vector3(r.xMax, r.yMax, 0),
            new Vector3(r.xMin, r.yMax, 0),
        };
        Gizmos.DrawLine(pts[0], pts[1]);
        Gizmos.DrawLine(pts[1], pts[2]);
        Gizmos.DrawLine(pts[2], pts[3]);
        Gizmos.DrawLine(pts[3], pts[0]);
    }
}