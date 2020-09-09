using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockList : MonoBehaviour {

    public static Dictionary<byte, GameObject> blocks = new Dictionary<byte, GameObject>();

    public GameObject dirtPrefab;
    public GameObject dWgPrefab;

    void Awake()
    {
        blocks.Add(1,dirtPrefab);
        blocks.Add(2, dWgPrefab);
    }

    public static GameObject GetBlock(byte id)
    {
        if (!blocks.ContainsKey(id)) return null;
        else return blocks[id];
    }
}
