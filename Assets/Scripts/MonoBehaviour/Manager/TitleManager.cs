﻿using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルを管理するクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// ゲームシーンに遷移するボタンが押されたら
    /// </summary>
    public void OnGameScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// 設定ボタンが押されたら
    /// </summary>
    public void OnConfigButton()
    {
        // ゲームマネージャーの終了メソッドを呼び出す
        GameManager.instance.CallConfigUI();
    }

    /// <summary>
    /// 終了ボタンが押されたら
    /// </summary>
    public void OnExitButton()
    {
        // ゲームマネージャーの終了メソッドを呼び出す
        GameManager.instance.OnExit();
    }
}