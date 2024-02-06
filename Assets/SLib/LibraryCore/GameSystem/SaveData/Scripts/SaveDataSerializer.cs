using SgLib.Singleton;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SgLib.Systems;
// �쐬 ����
namespace SgLib
{
    namespace Systems
    {
        public class SaveDataSerializer : SingletonBaseClass<SaveDataSerializer> // �Z�[�u�f�[�^�̓W�J
        {
            protected override void ToDoAtAwakeSingleton() { }

            public SaveDataTemplate ReadSaveData()
            {
                string dataStr = "";
                try
                {
                    StreamReader sr = new StreamReader(Application.dataPath + "/PlayerSavedData.json");
                    dataStr = sr.ReadToEnd();
                    sr.Close();
                    return JsonUtility.FromJson<SaveDataTemplate>(dataStr);
                }
                catch(FileNotFoundException)
                {
                    var t = GameObject.FindGameObjectWithTag("Player_Pos_OnNoData").transform;
                    var data = new SaveDataTemplate();
                    data.lastLookingRotation = t.rotation;
                    data.lastStandingPosition = t.position;

                    return data;
                }
            }
        }

    }
}
