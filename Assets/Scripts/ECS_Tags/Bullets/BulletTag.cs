using Unity.Entities;
using UnityEngine;

/// <summary>
/// �e�̃R���|�[�l���g�^�O
/// </summary>
[GenerateAuthoringComponent]
public struct BulletTag : IComponentData
{
    // �e�̐i�ރX�s�[�h
    [HideInInspector] public float _bulletSpeed;
}
