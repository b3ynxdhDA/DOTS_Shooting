using UnityEngine;
namespace shooting
{
    /// <summary>
    /// シーン切り替え時の処理をするクラス
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// ゲームシーンに切り替えた時に呼ぶ
        /// </summary>
        public void LoadGameScene()
        {
            GameManager.instance.InitializeGame();
        }
    }
}