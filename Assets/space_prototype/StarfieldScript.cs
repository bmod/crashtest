using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfieldScript : MonoBehaviour {

    public int numOfStarsLayer1 = 30;
    public int numOfStarsLayer2 = 70;
    public int numOfStarsLayer3 = 100;
    public int starfieldHeight = 20;
    public int starfieldWidth = 30;
    public float layer1Speed = 0.4f;
    public float layer2Speed = 0.03f;
    public float layer3Speed = 0.01f;
    public float starTimeout = 1;
    public float rangeLow = -1.0f;
    public float rangeHigh = 1.0f;
    public float starScaleMinLayer1 = 0.3f;
    public float starScaleMaxLayer1 = 0.5f;
    public float starScaleMinLayer2 = 0.2f;
    public float starScaleMaxLayer2 = 0.3f;
    public float starScaleMinLayer3 = 0.08f;
    public float starScaleMaxLayer3 = 0.1f;
    public float edgeSpawnMin = 0f;
    public float edgeSpawnMax = 3f;

    public GameObjectPooler starPool;
    public GameObject camera;
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

        starfieldRect.position = camera.transform.position + starfieldOffset;
		

        GameObject obj;

        //Instantiate 3 star layers
        layer1 = new List<GameObject>();
        layer2 = new List<GameObject>();
        layer3 = new List<GameObject>();

        float totalArea = starfieldWidth * starfieldHeight;

        //layer1 initial starfield generation
        float starArea = totalArea / numOfStarsLayer1;
        float length = Mathf.Sqrt(starArea);

        for (float i = length/2; i < starfieldWidth; i+=length)
        {
            for (float j = length/2; j < starfieldHeight; j+=length)
            {
                obj = starPool.GetPooledObject();
                obj.GetComponent<SpriteRenderer>().color = Color.white;
                obj.transform.position = new Vector2(i+Random.Range(rangeLow, rangeHigh), j + Random.Range(rangeLow, rangeHigh)) - starfieldRect.position;
                float randScale = Random.Range(starScaleMinLayer1, starScaleMaxLayer1);
                obj.transform.localScale = new Vector3(randScale, randScale, randScale);
                obj.SetActive(true);
                layer1.Add(obj);
            }
        }

        starArea = totalArea / numOfStarsLayer2;
        length = Mathf.Sqrt(starArea);
        //layer2 initial starfield generation
        for (float i = length / 2; i < starfieldWidth; i += length)
        {
            for (float j = length / 2; j < starfieldHeight; j += length)
            {
                obj = starPool.GetPooledObject();
                float roll = Random.value;
                if (roll < 0.5f)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.grey;
                }                obj.transform.position = new Vector2(i + Random.Range(rangeLow, rangeHigh), j + Random.Range(rangeLow, rangeHigh)) - starfieldRect.position;
                float randScale = Random.Range(starScaleMinLayer2, starScaleMaxLayer2);
                obj.transform.localScale = new Vector3(randScale, randScale, randScale);
                obj.SetActive(true);
                layer2.Add(obj);
            }
        }

        //layer3 initial starfield generation
        starArea = totalArea / numOfStarsLayer3;
        length = Mathf.Sqrt(starArea);
        for (float i = length / 2; i < starfieldWidth; i += length)
        {
            for (float j = length / 2; j < starfieldHeight; j += length)
            {
                obj = starPool.GetPooledObject();
                float roll = Random.value;
                if (roll < 0.08f)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else if (roll < 0.15f)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                else if (roll < 0.4f)
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.grey;
                }
                else
                {
                    obj.GetComponent<SpriteRenderer>().color = Color.white;
                }
                obj.transform.position = new Vector2(i + Random.Range(rangeLow, rangeHigh), j + Random.Range(rangeLow, rangeHigh)) - starfieldRect.position;
                float randScale = Random.Range(starScaleMinLayer3, starScaleMaxLayer3);
                obj.transform.localScale = new Vector3(randScale, randScale, randScale);
                obj.SetActive(true);
                layer3.Add(obj);
            }
        }
    }


    void Update() {
		if (camera == null)
			return;
        //Center starfield rect on player
        starfieldRect.position = camera.transform.position - starfieldOffset;

        //Store player position delta
        deltaPosition = lastPosition - camera.transform.position;
        lastPosition = camera.transform.position;

        //Update 1st layer
        for (int i = layer1.Count - 1; i >= 0; i--)
        {
            layer1[i].transform.position += (deltaPosition * layer1Speed) - deltaPosition;
            StarLife script = layer1[i].GetComponent<StarLife>();

            if (!starfieldRect.Contains(layer1[i].transform.position))
            {
                script.timeOOB += Time.deltaTime;
            }
            else
            {
                script.timeOOB = 0;
            }

            if (script.timeOOB > starTimeout)
            {
                layer1[i].SetActive(false);
                layer1.RemoveAt(i);
            }
        }
        

        //Update 2nd Layer
        for (int i = layer2.Count - 1; i >= 0; i--)
        {
            layer2[i].transform.position += (deltaPosition * layer2Speed) - deltaPosition;
            StarLife script = layer2[i].GetComponent<StarLife>();

            if (!starfieldRect.Contains(layer2[i].transform.position))
            {
                script.timeOOB += Time.deltaTime;
            }
            else
            {
                script.timeOOB = 0;
            }

            if (script.timeOOB > starTimeout)
            {
                layer2[i].SetActive(false);
                layer2.RemoveAt(i);
                
            }
        }

        //Update 3rd Layer
        for (int i = layer3.Count - 1; i >= 0; i--)
        {
            layer3[i].transform.position += (deltaPosition * layer3Speed) - deltaPosition;
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
        while (layer1.Count < numOfStarsLayer1)
        {
            GameObject obj;
            Vector2 dirNormal;
            obj = starPool.GetPooledObject();
            StarLife script = obj.GetComponent<StarLife>();
            script.timeOOB = 0;
            dirNormal = player.GetComponent<Rigidbody2D>().velocity.normalized;
            if(dirNormal.x > 0)
            {
                if(dirNormal.y > 0)
                {
                    //top right
                    if(Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax);
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax, Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
                else
                {
                    //bottom right
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin);
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax, Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
            }
            else
            {
                if (dirNormal.y > 0)
                {
                    //top left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax);
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin, Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
                else
                {
                    //bottom left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin);
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin, Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
            }
            obj.SetActive(true);
            layer1.Add(obj);
        }

        //Check number of stars in 2nd layer and add more in direction of player movement
        if (layer2.Count < numOfStarsLayer2)
        {
            GameObject obj;
            Vector2 dirNormal;
            obj = starPool.GetPooledObject();
            StarLife script = obj.GetComponent<StarLife>();
            script.timeOOB = 0;
            dirNormal = player.GetComponent<Rigidbody2D>().velocity.normalized;
            if (dirNormal.x > 0)
            {
                if (dirNormal.y > 0)
                {
                    //top right
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax - Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax - Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
                else
                {
                    //bottom right
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin + Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax - Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }

            }
            else
            {
                if (dirNormal.y > 0)
                {
                    //top left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax - Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin + Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
                else
                {
                    //bottom left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin + Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin + Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
            }
            obj.SetActive(true);
            layer2.Add(obj);
        }

        //Check number of stars in 3rd layer and add more in direction of player movement
        if (layer3.Count < numOfStarsLayer3)
        {
            GameObject obj;
            Vector2 dirNormal;
            obj = starPool.GetPooledObject();
            StarLife script = obj.GetComponent<StarLife>();
            script.timeOOB = 0;
            dirNormal = player.GetComponent<Rigidbody2D>().velocity.normalized;
            if (dirNormal.x > 0)
            {
                if (dirNormal.y > 0)
                {
                    //top right
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax - Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax - Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }


                }
                else
                {
                    //bottom right
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin + Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMax - Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }


                }

            }
            else
            {
                if (dirNormal.y > 0)
                {
                    //top left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMax - Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin + Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }


                }
                else
                {
                    //bottom left
                    if (Random.value < 0.5f)
                    {
                        obj.transform.position = new Vector2(Random.Range(starfieldRect.xMin, starfieldRect.xMax), starfieldRect.yMin + Random.Range(edgeSpawnMin, edgeSpawnMax));
                    }
                    else
                    {
                        obj.transform.position = new Vector2(starfieldRect.xMin + Random.Range(edgeSpawnMin, edgeSpawnMax), Random.Range(starfieldRect.yMin, starfieldRect.yMax));
                    }
                }
            }
            obj.SetActive(true);
            layer3.Add(obj);
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