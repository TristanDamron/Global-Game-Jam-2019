using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCounter : MonoBehaviour
{

    [SerializeField] Text memoryCounter;
    int numberOfMemories = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        memoryCounter.text = numberOfMemories.ToString() + (" / 3");
    }

    public void CountMemories ()
    {
        numberOfMemories++;
    }

    

}
