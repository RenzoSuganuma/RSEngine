using SgLib.Singleton;
using System.IO;
using UnityEngine;
// 作成 菅沼
namespace SgLib
{
    namespace Systems
    {
        public class ClientDataReader : SingletonBaseClass<ClientDataReader> // セーブデータの展開
        {
            protected override void ToDoAtAwakeSingleton() { }

            /// <summary> セーブデータの読み込み </summary>
            public ClientDataTemplate ReadSaveData()
            {
                string dataStr = "";
                try
                {
                    StreamReader sr = new StreamReader(Application.dataPath + "/PlayerSavedData.json");
                    dataStr = sr.ReadToEnd();
                    sr.Close();
                    return JsonUtility.FromJson<ClientDataTemplate>(dataStr);
                }
                catch(FileNotFoundException)
                {
                    var t = GameObject.FindGameObjectWithTag("Player_Pos_OnNoData").transform;
                    var data = new ClientDataTemplate();
                    data.lastLookingRotation = t.rotation;
                    data.lastStandingPosition = t.position;

                    return data;
                }
            }
        }

    }
}
