using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soultia.Voxel;
using Soultia.Util;

public class PlayerController : MonoBehaviour {

    //public int viewRange = 30;

	void Update () {
        for (float x = transform.position.x - Chunk.width; x < transform.position.x + Chunk.width; x += Chunk.width)
        {
            for (float y = transform.position.y - Chunk.height; y < transform.position.y + Chunk.height; y += Chunk.height)
            {
                if(y <= Chunk.height * 16 && y > 0)
                {
                    for (float z = transform.position.z - Chunk.width; z < transform.position.z + Chunk.width; z += Chunk.width)
                    {
                        int xx = Chunk.width * Mathf.FloorToInt(x / Chunk.width);
                        int yy = Chunk.height * Mathf.FloorToInt(y / Chunk.height);
                        int zz = Chunk.width * Mathf.FloorToInt(z / Chunk.width);
                        if (!Map.instance.ChunkExists(xx, yy, zz))
                        {
                            Map.instance.CreateChunk(new Vector3i(xx, yy, zz));
                        }
                    }
                }
            } 
        }
    }
}
