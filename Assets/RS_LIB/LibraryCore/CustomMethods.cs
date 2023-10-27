using System;
namespace RSEngine
{
    #region 独自メソッド Original Methods
    /// <summary> 独自のメソッドを提供するクラス </summary>
    public static class OriginalMethods
    {
        /// <summary> 
        /// <para>第１引数が真の時のみ第２引数の処理を実行する </para>
        /// When 1st Argument is True, Do 2nd Arguments Process
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        public static void DOnce(bool condition, Action action)// Function Name Means Do Once
        {
            if (condition) { action(); }
        }
        /* ------------------------------------------------------------------ */
    }
    #endregion
}