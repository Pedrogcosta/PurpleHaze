using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBaseState : State
{
    //quanto tempo o state vai ficar ativo antes de passar
    public float duration;

    //componente do animator
    protected Animator animator;

    //checar se o próximo ataque da sequencia deve ser rodado
    protected bool shouldCombo;

    //a sequencia dos ataques
    protected int attackIndex;

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(Input.GetMouseButtonDown(0))
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }


}
