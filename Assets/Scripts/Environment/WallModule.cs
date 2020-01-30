using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallModule : RunnableModule
{
    public WallShaderHelper wallShaderHelper;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (autoRun)
        {
            HidePerfectTimeIndicator();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HidePerfectTimeIndicator()
    {
        animator.SetBool("HidePerfectTimeIndicator", true);
    }

    public override void Ready()
    {
        base.Ready();

        if (!autoRun)
        {
            animator.SetBool("Ready", true);
        }
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
