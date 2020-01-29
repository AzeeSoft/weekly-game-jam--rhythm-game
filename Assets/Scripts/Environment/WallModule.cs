using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallModule : RunnableModule
{
    public WallShaderHelper wallShaderHelper { get; private set; }

    private Animator animator;

    void Awake()
    {
        wallShaderHelper = GetComponent<WallShaderHelper>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Ready()
    {
        base.Ready();
        animator.SetBool("Ready", true);
    }

    public override void Perfect()
    {
        base.Perfect();
        animator.SetTrigger("Perfect");
    }

    public override void Acceptable()
    {
        base.Acceptable();
        animator.SetTrigger("Acceptable");
    }

    public override void Fail()
    {
        base.Fail();
        animator.SetTrigger("Fail");
    }
}
