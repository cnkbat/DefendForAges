using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFadeable 
{
    void FadeOut();
    void ResetFade();
    void SetFade(bool value);
}