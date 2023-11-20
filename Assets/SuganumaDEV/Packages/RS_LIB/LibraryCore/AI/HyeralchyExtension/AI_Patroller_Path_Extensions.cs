using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RSEngine.AI;
public class AI_Patroller_Path_Extensions : MonoBehaviour
{
    [MenuItem("GameObject/RSE_AI/PatrollingPath(RSEngine)", false, 10)]
    static void CreateGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject root = new();
        root.AddComponent<PartollingPathHolder>();
        root.name = "AI_PatrollPath_Root";
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(root, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(root, "Create " + root);
        Selection.activeObject = root;

        GameObject[] gameObj = new GameObject[5];
        for (int i = 0; i < gameObj.Length; i++)
        {
            gameObj[i] = new();
        }
        for (int i = 0; i < gameObj.Length; i++)
        {
            gameObj[i].name = "AI_PatrollPath_" + i.ToString();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(gameObj[i], menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(gameObj[i], "Create " + gameObj[i]);
            Selection.activeObject = gameObj[i];

            gameObj[i].transform.parent = root.transform;
        }
    }
}
