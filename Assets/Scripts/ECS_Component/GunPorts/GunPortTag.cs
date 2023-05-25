using Unity.Entities;
#if UNITY_EDITOR
using UnityEngine;
#endif

/// <summary>
/// �e�̔��˒n�_�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct GunPortTag : IComponentData
{
    // ���˂���e�̃G���e�B�e�B
    public Entity _BulletEntity;

    // �A�˂̊Ԋu
    [HideInInspector] public float _shootCoolTime;

    // �A�˂̃N�[���^�C��(���˒n�_���ƂɘA�˂̃N�[���^�C����ς��邽��Tag���ێ�����)
    [HideInInspector] public float _shootInterval;

    // ���˂���e�̑��x
    [HideInInspector] public float _bulletSpeed;
}
