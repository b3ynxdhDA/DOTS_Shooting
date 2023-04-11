using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create KomaData")]
public class KomaData : ScriptableObject
{
    /// <summary>
    /// ��̌����ڂ̃X�v���C�g
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// ��̗̑�
    /// </summary>
    public float hp;

    /// <summary>
    /// ���˂���e�̎�ނ̃X�e�[�g
    /// </summary>
    public enum ShootKind
    {
        StraightGunPortTag,
        WideGunPortTag,
        AimGunPortTag
    }

    /// <summary>
    /// ���˂���e�̎��
    /// </summary>
    public ShootKind shootKind;

    /// <summary>
    /// ���˂���e�̗�
    /// </summary>
    public int shootLine;

    /// <summary>
    /// ���˂���e�̃X�s�[�h
    /// </summary>
    public float bulletSpeed;

    /// <summary>
    /// �ˌ��̃N�[���^�C��
    /// </summary>
    public float shootCoolTime;
}
