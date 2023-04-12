using Unity.Entities;
using UnityEngine;

/// <summary>
/// �{�X�L�����N�^�[���Ǘ�����
/// </summary>
public class BossManager
{
    #region �ϐ��錾
    /// <summary>
    /// ��2�i�K�̃{�X�̋�f�[�^
    /// </summary>
    public KomaData BossKomaData1 { get; private set; }
    /// <summary>
    /// ��3�i�K�̃{�X�̋�f�[�^
    /// </summary>
    public KomaData BossKomaData2 { get; private set; }

    /// <summary>
    /// �{�X�L�����N�^�[�̏�Ԃ����i�K�ڂ�
    /// </summary>
    public int BossPhaseCount { get; private set; }

    #endregion

    #region ���J���\�b�h
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��BossManager�̏��������\�b�h
    /// </summary>
    /// <param name="komaData0">��1�i�K�̃{�X�̋�f�[�^</param>
    /// <param name="komaData1">��2�i�K�̃{�X�̋�f�[�^</param>
    /// <param name="komaData2">��3�i�K�̃{�X�̋�f�[�^</param>
    public BossManager(KomaData komaData1, KomaData komaData2)
    {
        BossKomaData1 = komaData1;
        BossKomaData2 = komaData2;
    }
    public void BossInitialize()
    {
        BossPhaseCount = 0;

    }

    /// <summary>
    /// �{�X�̍U���i�K���グ�郁�\�b�h
    /// </summary>
    public void UpdateBossCount()
    {
        BossPhaseCount++;
        Debug.Log(BossPhaseCount);
    }

    #endregion
}