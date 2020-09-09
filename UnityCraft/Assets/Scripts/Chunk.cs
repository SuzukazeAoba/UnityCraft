using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soultia.Util;

namespace Soultia.Voxel
{
    public class Chunk : MonoBehaviour
    {
        public static int width = 16;
        public static int height = 16;

        public byte[,,] blocks;
        public Vector3i position;

        public static bool isWorking = false;
        private bool isFinished = false;

        public GameObject cubePrefab;
        public Dictionary<Vector3i, GameObject> cubes = new Dictionary<Vector3i, GameObject>();
        
        void Start()
        {
            position = new Vector3i(this.transform.position);
            if (Map.instance.ChunkExists(position))
            {
                Destroy(this.gameObject);
            }
            else
            {
                Map.instance.chunks.Add(position, this.gameObject);
                this.name = "(" + position.x + "," + position.y + "," + position.z + ")";
                //StartFunction();
            }
        }

        void Update()
        {
            if(isWorking == false && isFinished == false)
            {
                isFinished = true;
                StartFunction();
            }
        }

        public void StartFunction()
        {
            StartCoroutine(CreateMap());
        }

        public void DestroyCube(int x, int y, int z)
        {
            StartCoroutine(DestroyCubeMesh(x, y, z));
        }

        public void CreateCube(int x, int y, int z, byte block_id)
        {
            Vector3i pos = new Vector3i(x + (int)this.transform.position.x, y + (int)this.transform.position.y, z + (int)this.transform.position.z);
            if (cubes.ContainsKey(pos))
            {
                return;
            }
            else StartCoroutine(CreateCubeMesh(x, y, z, block_id));
        }

        IEnumerator DestroyCubeMesh(int x, int y, int z)
        {
            while (isWorking)
            {
                yield return null;
            }
            isWorking = true;

            blocks[x, y, z] = 0;
            Vector3i pos = new Vector3i(x + (int)this.transform.position.x, y + (int)this.transform.position.y, z + (int)this.transform.position.z);

            if (cubes.ContainsKey(pos))
            {
                cubes.Remove(pos);
            }
            StartCoroutine(CreateMesh());
        }

        IEnumerator CreateCubeMesh(int x, int y, int z, byte block_id)
        {
            while (isWorking)
            {
                yield return null;
            }

            isWorking = true;

            blocks[x, y, z] = block_id;
            StartCoroutine(CreateMesh());
        }

        IEnumerator CreateMap()
        {
            while (isWorking)
            {
                yield return null;
            }
            isWorking = true;
            blocks = new byte[width, height, width];
            for (int x = 0; x < Chunk.width; x++)
            {
                for (int y = 0; y < Chunk.height; y++)
                {
                    for (int z = 0; z < Chunk.width; z++)
                    {
                        byte blockid = Terrain.GetTerrainBlock(new Vector3i(x, y, z) + position);
                        if (blockid == 1 && Terrain.GetTerrainBlock(new Vector3i(x, y + 1, z) + position) == 0)
                        {
                            blocks[x, y, z] = 2;
                        }
                        else
                        {
                            blocks[x, y, z] = Terrain.GetTerrainBlock(new Vector3i(x, y, z) + position);
                        }
                    }
                }
            }

            StartCoroutine(CreateMesh());
        }

        public IEnumerator CreateMesh()
        {
            for (int x = 0; x < Chunk.width; x++)
            {
                for (int y = 0; y < Chunk.height; y++)
                {
                    for (int z = 0; z < Chunk.width; z++)
                    {
                        cubePrefab = BlockList.GetBlock(blocks[x, y, z]);
                        if (cubePrefab == null) continue;
                        if (IsBlockTransparent(x + 1, y, z))
                        {
                            AddCube(x, y, z);
                        }
                        else if (IsBlockTransparent(x - 1, y, z))
                        {
                            AddCube(x, y, z);
                        }
                        else if (IsBlockTransparent(x, y, z + 1))
                        {
                            AddCube(x, y, z);
                        }
                        else if (IsBlockTransparent(x, y, z - 1))
                        {
                            AddCube(x, y, z);
                        }
                        else if (IsBlockTransparent(x, y + 1, z))
                        {
                            AddCube(x, y, z);
                        }
                        else if (IsBlockTransparent(x, y - 1, z))
                        {
                            AddCube(x, y, z);
                        }
                    }
                }
            }

            yield return null;
            isWorking = false;
        }

        public bool IsBlockTransparent(int x, int y, int z)
        {
            if (x >= width || y >= height || z >= width || x < 0 || y < 0 || z < 0)
            {
                return true;
            }
            else
            {
                return this.blocks[x, y, z] == 0;
            }
        }

        void AddCube(int x, int y, int z)
        {
            Vector3i pos = new Vector3i(x + (int)this.transform.position.x, y + (int)this.transform.position.y, z + (int)this.transform.position.z);

            if (cubes.ContainsKey(pos))
            {
                return;
            }

            GameObject cube = Instantiate(cubePrefab, pos, Quaternion.identity);
            cube.GetComponent<BlockInfo>().block_x = x;
            cube.GetComponent<BlockInfo>().block_y = y;
            cube.GetComponent<BlockInfo>().block_z = z;
            int bx = cube.GetComponent<BlockInfo>().block_x;
            int by = cube.GetComponent<BlockInfo>().block_y;
            int bz = cube.GetComponent<BlockInfo>().block_z;
            cube.name = "cube@" + pos + "Block@" + bx + "," + by + "," + bz;
            cube.transform.parent = this.transform;
            cubes.Add(pos, cube);
        }
    }
}