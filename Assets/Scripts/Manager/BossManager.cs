
/// <summary>
/// �{�X�L�����N�^�[���Ǘ�����
/// </summary>
public class BossManager 
{
    #region �ϐ��錾
    /// <summary>
    /// �{�X�L�����N�^�[�̏�Ԃ����i�K�ڂ�
    /// </summary>
    public int BossPhaseCount { get; private set; }

    #endregion

    #region ���\�b�h
    public void BossInitialize()
    {
        BossPhaseCount = 0;

    }

    #endregion
}
