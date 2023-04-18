using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X�e�[�W�₻��UI���Ǘ�����N���X
/// </summary>
public class UIManager : MonoBehaviour
{
    // �ϐ��錾--------------------------
    private bool _isCallGameOver = false;
    // �^�C�}�[
    private float _timerCount = 0;

    // �e�L�X�g�I�u�W�F�N�g---------------------------
    // �Q�[���X�^�[�g�̃J�E���g
    [SerializeField] private Text _startCountText = default;

    // �Q�[���I�[�o�[�e�L�X�g
    [SerializeField] private GameObject _gameOverText = default;

    // ���U���g�e�L�X�g
    [SerializeField] private GameObject _resultUI = default;

    // �n�C�X�R�A�e�L�X�g
    [SerializeField] private Text _scoreCountText = default;

    // �^�C�}�[�e�L�X�g
    [SerializeField] private Text _timerCountText = default;

    // �萔�錾---------------------
    // 1���Ԃ̕b��
    const int _ONE_MINUTES = 60;

    // �f�t�H���g�̃^�C���X�P�[��
    const int _SEFAULT_TIMESCALE = 1;

    // �Q�[���I�[�o�[�e�L�X�g�̈ړ����Y���W
    const float _GAMEOVER_TEXT_POSITION_Y = 0;

    private void Start()
    {
        // �Q�[���̏�Ԃ��Q�[������
        GameManager.instance.gameState = GameManager.GameState.GameRedy;

        // �Q�[���X�^�[�g�̃J�E���g�_�E�����J�n
        StartCoroutine("CountdownCoroutine");

        // �^�C���X�P�[���̏�����
        Time.timeScale = _SEFAULT_TIMESCALE;
    }
    private void Update()
    {
        // �n�C�X�R�A�̕\�����X�V
        _scoreCountText.text = "" + GameManager.instance._nowScore;

        // �Q�[���X�e�[�g���Q�[�����̎�
        if (GameManager.instance.gameState == GameManager.GameState.GameNow)
        {
            // �^�C�}�[�̍X�V(����)
            _timerCount += Time.deltaTime;
            _timerCountText.text = "" + ((int)_timerCount / _ONE_MINUTES).ToString("00") + " : " + ((int)_timerCount % _ONE_MINUTES).ToString("00");

            // �^�C�}�[�̍X�V(����)
            //_timerCount -= Time.deltaTime;
            //_timerCountText.text = "" + ((int)_timerCount / _ONE_MINUTES).ToString("00") + " : " + ((int)_timerCount % _ONE_MINUTES).ToString("00");

            // �Q�[���X�e�[�g���Q�[���I�[�o�[�ŁA�Q�[���I�[�o�[�R���[�`�����Ă�ł��Ȃ��Ȃ�
            if (GameManager.instance.gameState == GameManager.GameState.GameOver && !_isCallGameOver)
            {
                StartCoroutine("GameOver");
                _isCallGameOver = true;
            }
        }
    }

    /// <summary>
    /// �Q�[���J�n�̂R�J�E���g�_�E���̃R���[�`��
    /// </summary>
    IEnumerator CountdownCoroutine()
    {
        _startCountText.gameObject.SetActive(true);

        _startCountText.text = "3";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "2";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "1";
        GameManager.instance.SEManager.OnStartCount3_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "GO!";
        GameManager.instance.SEManager.OnStartCountGo_SE();
        yield return new WaitForSeconds(1f);

        _startCountText.text = "";
        _startCountText.gameObject.SetActive(false);
        GameManager.instance.gameState = GameManager.GameState.GameNow;
    }

    /// <summary>
    /// �Q�[���I�[�o�[���Ă��烊�U���g�܂ł̏���
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOver()
    {
        // �Q�[���X�e�[�g��GameOver��
        GameManager.instance.gameState = GameManager.GameState.GameOver;

        _gameOverText.SetActive(true);

        // �Q�[���I�[�o�[�e�L�X�g�̃|�W�V������0���傫����
        while (_GAMEOVER_TEXT_POSITION_Y < _gameOverText.transform.localPosition.y)
        {
            // �Q�[���I�[�o�[�e�L�X�g�̃|�W�V������������
            _gameOverText.transform.localPosition += Vector3.down * 10;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(3f);

        // �Q�[���X�e�[�g��Result��
        GameManager.instance.gameState = GameManager.GameState.Result;

        _resultUI.SetActive(true);
    }
}