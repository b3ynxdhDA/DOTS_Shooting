/// <summary>
/// �{�X�L�����N�^�[���Ǘ�����
/// </summary>
public class BossManager
{
    #region �ϐ��錾
    // �Q�Ɨp�̃{�X��HP
    private float _bossHP;

    // ��1�i�K�̋�ݒ肳��Ă��邩
    private bool _isBossInitialize = false;

    // �{�X�L�����N�^�[�̏�Ԃ����i�K�ڂ�
    private int _bossPhaseCount;
    #endregion

    #region �v���p�e�B
    /// <summary>
    /// ��1�i�K�̃{�X�̋�f�[�^
    /// </summary>
    public KomaData BossKomaData1 { get; private set; }
    /// <summary>
    /// ��2�i�K�̃{�X�̋�f�[�^
    /// </summary>
    public KomaData BossKomaData2 { get; private set; }
    /// <summary>
    /// ��3�i�K�̃{�X�̋�f�[�^
    /// </summary>
    public KomaData BossKomaData3 { get; private set; }

    public float GetBossHP { get { return _bossHP; } set { _bossHP = value; } }

    /// <summary>
    /// �{�X�L�����N�^�[�̏�Ԃ����i�K�ڂ�
    /// </summary>
    public int BossPhaseCount { get { return _bossPhaseCount; }}

    /// <summary>
    /// ��1�i�K�̋�ݒ肳��Ă��邩
    /// </summary>
    public bool IsBossInitialize { get { return _isBossInitialize; } set { _isBossInitialize = value; } }

    #endregion

    #region ���J���\�b�h
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��BossManager�̎Q�ƃ��\�b�h
    /// </summary>
    /// <param name="komaData1">��1�i�K�̃{�X�̋�f�[�^</param>
    /// <param name="komaData2">��2�i�K�̃{�X�̋�f�[�^</param>
    /// <param name="komaData3">��3�i�K�̃{�X�̋�f�[�^</param>
    public BossManager(KomaData komaData1, KomaData komaData2, KomaData komaData3)
    {
        BossKomaData1 = komaData1;
        BossKomaData2 = komaData2;
        BossKomaData3 = komaData3;
    }

    /// <summary>
    /// �{�X�̏����������郁�\�b�h
    /// </summary>
    public void BossInitialize()
    {
        _bossPhaseCount = 1;
        _isBossInitialize = false;
    }

    /// <summary>
    /// �{�X�̍U���i�K���グ�郁�\�b�h
    /// </summary>
    public void UpdateBossCount()
    {
        _bossPhaseCount++;
    }

    #endregion
}