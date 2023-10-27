using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneralAICoreHyeralchyExtension : MonoBehaviour
{
    [MenuItem("GameObject/GeneralAI/PatrollingPath", false, 10)]
    static void CreateGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject[] gameObj = new GameObject[5];
        for(int i = 0; i < gameObj.Length; i++)
        {
            gameObj[i] = new GameObject();
        }
        for (int i = 0; i < gameObj.Length; i++)
        {
            gameObj[i].name = "AI_PatrollPath_" + i.ToString();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(gameObj[i], menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(gameObj[i], "Create " + gameObj[i]);
            Selection.activeObject = gameObj[i];
        }
    }
}
