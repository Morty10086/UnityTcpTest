using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFallRocks : MonoBehaviour
{

    public int rocksCounter;
    public float periodTime;
    public bool isRandomPos;
    public Vector2 xRandomPosRange;
    public Vector2 yRandomPosRange;
    public float posZ;
    public List<Vector3> rockPosList;
    public float intervalTime=0;
    public bool isCanGenerateFallRocks;
    private void Awake()
    {
        
    }

   
    void Update()
    {
        if(isCanGenerateFallRocks)
        {
            intervalTime += Time.deltaTime;
            if (intervalTime >= periodTime)
            {
                intervalTime = 0;
                for(int i=0;i<rocksCounter;i++)
                {
                    GameObject rockObj= PoolManager.Instance.GetObj("Component/GameLevel/FallRock");
                    if(!isRandomPos)
                    {
                        rockObj.transform.position = rockPosList[i];
                    }
                    else
                    {
                        Vector3 randomPos=new Vector3(Random.Range(xRandomPosRange.x,xRandomPosRange.y), Random.Range(yRandomPosRange.x, yRandomPosRange.y),posZ);
                        rockObj.transform.position= randomPos;
                    }   
                }
               
            }
        }
        
    }
}
