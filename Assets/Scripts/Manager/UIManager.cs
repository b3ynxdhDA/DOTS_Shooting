using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �X�e�[�W��UI���Ǘ�����N���X
/// </summary>
public class UIManager : MonoBehaviour
{
    // �ϐ��錾--------------------------
    // �^�C�}�[
    private float _timerCount = 0;

    // �e�L�X�g�I�u�W�F�N�g---------------------------
    // ���U���g�e�L�X�g
    [SerializeField] private GameObject _resultUI = default;
    
    // �{�X�̗̑̓o�[
    [SerializeField] private Slider _bossHpSlider = default;

    // �Q�[���I�[�o�[�e�L�X�g
    [SerializeField] private GameObject _gameFinishText = default;

    // �Q�[���X�^�[�g�̃J�E���g
    [SerializeField] private Text _startCountText = default;

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

    // �Q�[���N���A���ɕ\������e�L�X�g
    const string _CLEAR_TEXT = "!! GAME CLEAR !!";

    // �Q�[���I�[�o�[���ɕ\������e�L�X�g
    const string _DEFEAT_TEXT = "GAME OVER";

    private void Start()
    {
        // �Q�[���X�^�[�g�̃J�E���g�_�E�����J�n
        StartCoroutine("CountdownCoroutine");

        // �^�C���X�P�[���̏�����
        Time.timeScale = _SEFAULT_TIMESCALE;

        GameManager.instance.InitializeGame();
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

        }
    }

    /// <summary>
    /// �Q�[���I�����̃R���[�`�����O������Ăԃ��\�b�h
    /// </summary>
    /// <param name="clear">true:�Q�[���N���A,false:�Q�[���I�[�o�[</param>
    public void CallGameFinish(bool clear)
    {
        // �Q�[�������炵���J�ڂ��Ȃ�
        if (GameManager.instance.gameState == GameManager.GameState.GameNow)
        {
            StartCoroutine("GameFinish", clear);
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
    /// �Q�[���I�����̃e�L�X�g���ĂԃR���[�`��
    /// </summary>
    /// <returns></returns>
    IEnumerator GameFinish(bool isClear)
    {
        // �Q�[���X�e�[�g��GameOver��
        GameManager.instance.gameState = GameManager.GameState.GameFinish;

        _gameFinishText.SetActive(true);

        // �N���A�ɂ��J�ڂ��s�k�ɂ��J�ڂ�
        if (isClear)
        {
            _gameFinishText.GetComponent<Text>().text = _CLEAR_TEXT;
        }
        else
        {
            _gameFinishText.GetComponent<Text>().text = _DEFEAT_TEXT;
        }

        // �Q�[���I�[�o�[�e�L�X�g�̃|�W�V������0���傫����
        while (_GAMEOVER_TEXT_POSITION_Y < _gameFinishText.transform.localPosition.y)
        {
            // �Q�[���I�[�o�[�e�L�X�g�̃|�W�V������������
            _gameFinishText.transform.localPosition += Vector3.down * 10;
            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(3f);

        // �Q�[���X�e�[�g��Result��
        GameManager.instance.gameState = GameManager.GameState.Result;


        _gameFinishText.SetActive(false);
        _resultUI.SetActive(true);
        
    }
}