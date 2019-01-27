using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField]
    private Animator parent_;

    [SerializeField]
    private Animator child_;

    void Start()
    {
        parent_.Play("Cutscene");        
        child_.Play("Walk");        
    }
}
