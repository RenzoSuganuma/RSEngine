using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RSEngine;
using RSEngine.Singleton;
public class SingletonTest : SingletonBaseClass<SingletonTest>
{
    protected override void ToDoAtAwakeSingleton()
    {
        Debug.Log("Awaken");
    }
}
