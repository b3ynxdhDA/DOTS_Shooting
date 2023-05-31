using Unity.Entities;
using Unity.Rendering;

/// <summary>
/// ����Ǘ�����N���X
/// </summary>
public class KomaManager
{
    /// <summary>
    /// �Q�[���J�n����GameManager����Ă΂��KomaManager�̎Q�ƃ��\�b�h
    /// </summary>
    public KomaManager()
    {

    }

    /// <summary>
    /// �{�X�̋�̃X�e�[�^�X��ݒ肷��
    /// </summary>
    /// <param name="entity">�Ώۂ̃G���e�B�e�B</param>
    /// <param name="hpTag">�Ώۂ�HPTag�R���|�[�l���g</param>
    /// <param name="gunPortTag">�Ώۂ�GunPortTag�R���|�[�l���g</param>
    /// <param name="komaData">�ݒ肷��KomaDate</param>
    public void SetKomaDate(Entity entity, KomaData komaData, GunPortTag gunPortTag, EntityCommandBuffer commandBuffer)
    {
        // �{�X�̊�b�X�e�[�^�X��ݒ肷��
        commandBuffer.SetComponent(entity, new HPTag
        {
            _hp = komaData.hp
        });

        // �Z�b�g�R���|�[�l���g���ƃG���e�B�e�B�v���n�u��Null�ɂȂ�̂�
        gunPortTag._shootCoolTime = komaData.shootCoolTime;
        gunPortTag._bulletSpeed = komaData.bulletSpeed;

        // �}�e���A����ύX
        commandBuffer.SetSharedComponent(entity, new RenderMesh
        {
            mesh = GameManager.instance.Quad,
            material = komaData.material
        });

        // �ˌ��̎�ނ̃R���|�[�l���g��ݒ肷��
        // ���ɃZ�b�g����GunPort�̎�ނ͉���
        switch (komaData.shootKind)
        {
            case KomaData.ShootKind.StraightGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // StraightGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new StraightGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.WideGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<AimGunPortTag>(entity);

                // WideGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new WideGunPortTag
                {
                    _lines = komaData.shootLine
                });
                break;
            case KomaData.ShootKind.AimGunPortTag:

                // ���ɂ���GunPort�̎�ʃ^�O���폜����
                commandBuffer.RemoveComponent<StraightGunPortTag>(entity);
                commandBuffer.RemoveComponent<WideGunPortTag>(entity);

                // AimGunPortTag��ǉ�����
                commandBuffer.AddComponent(entity, new AimGunPortTag { });
                break;
        }
    }
}