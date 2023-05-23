using Unity.Entities;
/// <summary>
/// ����Ǘ�����N���X
/// </summary>
public class NormalEnemyManager
{
    #region �ϐ��錾
    // �t�B�[���h�ɂ���{�X�ȊO�̓G�̐�
    private int _enemyCount;

    /// <summary>
    /// ���������̃v���C���[�̋�f�[�^
    /// </summary>
    public Entity EnemyBasePrefab { get; private set; }

    /// <summary>
    /// ���������̃v���C���[�̋�f�[�^
    /// </summary>
    public KomaData[] NormalEnemyKomaData { get; private set; }

    #endregion
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��KomaManager�̎Q�ƃ��\�b�h
    /// </summary>
    public NormalEnemyManager(KomaData[] komaData)
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