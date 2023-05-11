/// <summary>
/// �{�X�L�����N�^�[���Ǘ�����
/// </summary>
public class PlayerManager
{
    #region �ϐ��錾

    /// <summary>
    /// ���������̃v���C���[�̋�f�[�^
    /// </summary>
    public KomaData PlayerKomaData { get; private set; }

    // �v���C���[�̋����������Ă��邩
    private bool _isPlayerInitialize = false;

    public bool IsPlayerInitialize { get { return _isPlayerInitialize; } set { _isPlayerInitialize = value; } }

    #endregion

    #region ���J���\�b�h

    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��PlayerManager�̎Q�ƃ��\�b�h
    /// </summary>
    /// <param name="komaData1">��1�i�K�̃{�X�̋�f�[�^</param>
    public PlayerManager(KomaData komaData)
    {
        PlayerKomaData = komaData;
    }
    
    public void PlayerInitialize()
    {
        _isPlayerInitialize = false;
    }

    #endregion
}