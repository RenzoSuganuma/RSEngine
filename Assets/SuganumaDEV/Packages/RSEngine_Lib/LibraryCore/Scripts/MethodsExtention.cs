// 管理者 菅沼
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RSEngine
{
    /// <summary> 拡張メソッドを提供するクラス </summary>
    public static class MethodsExtention
    {
        /* GameObjects */
        /// <summary>指定されたトランスフォームの子オブジェクトにする</summary>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        public static void ToChildObject(this GameObject obj, Transform parent)
        {
            obj.transform.parent = parent;
        }
        /// <summary>オブジェクトの親子関係を切る</summary>
        /// <param name="obj"></param>
        public static void ToParenObject(this GameObject obj)
        {
            obj.transform.parent = null;
        }
        /// <summary>子オブジェクトのみ取得する</summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<Transform> GetChildObjects(this GameObject parent)
        {
            List<Transform> list = new();
            var cnt = parent.transform.childCount;
            for (int i = 0; i < cnt; i++)
            {
                var child = parent.transform.GetChild(i);
                list.Add(child);
            }
            return list;
        }
        /* Delegates */
        /// <summary>
        /// Actionにデリゲート登録をする
        /// <para>Add Delegate To Action</para>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="appendProcess"></param>
        public static void Add(this Action action, Action appendProcess)
        {
            action += appendProcess;
        }
        /// <summary>
        /// Actionにデリゲート登録した関数を登録解除する
        /// <para>Remove Delegate From Action</para>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="removeTarget"></param>
        public static void Remove(this Action action, Action removeTarget)
        {
            action -= removeTarget;
        }
    }
}