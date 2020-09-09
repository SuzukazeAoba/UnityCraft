using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soultia.Voxel;
using Soultia.Util;

public class BlockManager : MonoBehaviour {

    public byte block_id = 1;

	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ClickToDestroy();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ClickToCreate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeToDirt();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeToDWG();
        }
    }

    void ChangeToDirt()
    {
        block_id = 1;
    }

    void ChangeToDWG()
    {
        block_id = 2;
    }

    void ClickToDestroy()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit))
        {
            GameObject targetCube = hit.collider.gameObject;
            GameObject targetChunk = targetCube.transform.parent.gameObject;

            int block_x = targetCube.GetComponent<BlockInfo>().block_x;
            int block_y = targetCube.GetComponent<BlockInfo>().block_y;
            int block_z = targetCube.GetComponent<BlockInfo>().block_z;

            Destroy(hit.collider.gameObject);
            targetChunk.GetComponent<Chunk>().DestroyCube(block_x, block_y, block_z);
        }
    }

    void ClickToCreate()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit))
        {

            GameObject hitCube = hit.collider.gameObject;
            GameObject targetChunk = hitCube.transform.parent.gameObject;

            int block_x = hitCube.GetComponent<BlockInfo>().block_x + (int)hit.normal.x;
            int block_y = hitCube.GetComponent<BlockInfo>().block_y + (int)hit.normal.y;
            int block_z = hitCube.GetComponent<BlockInfo>().block_z + (int)hit.normal.z;

            Vector3i posChunk = new Vector3i(targetChunk.transform.position);

            if (block_x < 0)
            {
                block_x = block_x + Chunk.width;
                posChunk.x -= Chunk.width;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }
            else if (block_x >= Chunk.width)
            {
                block_x = block_x - Chunk.width;
                posChunk.x += Chunk.width;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }
            else if (block_y < 0)
            {
                block_y = block_y + Chunk.height;
                posChunk.y -= Chunk.height;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }
            else if (block_y >= Chunk.height)
            {
                block_y = block_y - Chunk.height;
                posChunk.y += Chunk.height;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }
            else if (block_z < 0)
            {
                block_z = block_z + Chunk.width;
                posChunk.z -= Chunk.width;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }
            else if (block_z >= Chunk.width)
            {
                block_z = block_z - Chunk.width;
                posChunk.z += Chunk.width;
                if (Map.instance.ChunkExists(posChunk)) targetChunk = Map.instance.chunks[posChunk];
                else targetChunk = null;
                if (targetChunk == null)
                {
                    Debug.Log("Target Chunk does not exist");
                    return;
                }
            }

            targetChunk.GetComponent<Chunk>().CreateCube(block_x, block_y, block_z, block_id);
        }
    }
}
