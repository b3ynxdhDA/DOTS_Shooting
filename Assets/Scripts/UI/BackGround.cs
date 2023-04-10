using UnityEngine;

/// <summary>
/// 背景をスクロールさせるクラス
/// </summary>
public class BackGround : MonoBehaviour
{
    // スクロールスピード
    const int _SCROLL_SPEED = 1;

    // 移動してくる画面の上のy座標
    const float _TOP = 14.5f;

    // 移動する画面の下のy座標
    const float _LOWER = -11.3f;

    void Update()
    {
        // y軸方向にスクロール
        transform.Translate(Vector3.down * _SCROLL_SPEED * Time.deltaTime);

        // カメラの下端から完全に出たら、上端に移動
        if (transform.position.y < _LOWER)
        {
            transform.position = new Vector3(transform.position.x, _TOP, 0);
        }
    }
}
