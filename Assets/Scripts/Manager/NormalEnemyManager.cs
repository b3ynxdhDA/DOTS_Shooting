
/// <summary>
/// ����Ǘ�����N���X
/// </summary>
public class NormalEnemyManager
{

    #region �ϐ��錾

    /// <summary>
    /// ���������̃v���C���[�̋�f�[�^
    /// </summary>
    public KomaData NormalEnemyKomaData { get; private set; }

    #endregion
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��KomaManager�̎Q�ƃ��\�b�h
    /// </summary>
    public NormalEnemyManager(KomaData komaData)
    {
        NormalEnemyKomaData = komaData;
    }

    /// <summary>
    /// �G���G�̏�����
    /// </summary>
    public void NormalEnemyInitialize()
    {

    }
}