using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�[���؂�ւ����̏���������N���X
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// �Q�[���V�[���ɐ؂�ւ������ɌĂ�
    /// </summary>
    public void LoadGameScene()
    {
        GameManager.instance.InitializeGame();
    }
}
