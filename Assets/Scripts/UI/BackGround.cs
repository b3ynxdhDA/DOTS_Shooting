using UnityEngine;

/// <summary>
/// �w�i���X�N���[��������N���X
/// </summary>
public class BackGround : MonoBehaviour
{
    // �X�N���[���X�s�[�h
    const int _SCROLL_SPEED = 1;

    // �ړ����Ă����ʂ̏��y���W
    const float _TOP = 14.5f;

    // �ړ������ʂ̉���y���W
    const float _LOWER = -11.3f;

    void Update()
    {
        // y�������ɃX�N���[��
        transform.Translate(Vector3.down * _SCROLL_SPEED * Time.deltaTime);

        // �J�����̉��[���犮�S�ɏo����A��[�Ɉړ�
        if (transform.position.y < _LOWER)
        {
            transform.position = new Vector3(transform.position.x, _TOP, 0);
        }
    }
}
