using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public Action OnSaved;
    public Action OnResetData;

}
