using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCanim : MonoBehaviour
{
    public Animator anim;
    public bool anim_stay = true;
    public bool anim_none = false;
    private void Update()
    {
        anim.SetBool("Stay", anim_stay);
        anim.SetBool("NoneHandle", anim_none);

    }
}
