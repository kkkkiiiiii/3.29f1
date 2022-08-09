using UnityEngine;
public interface IAttachable
{
    void Drop(GameObject player);
    // 꽁초, 흡연자 그냥 떨어뜨림. → 게임 매니저로 접근 → 점수, 콤보 감소→ UI UpdateScore 처리
    void OnAttach(GameObject player);
    // 꽁초, 흡연자 플레이어 뒤에 붙인다. → 게임 매니저로 접근 → 점수 증가→ UI UpdateScore 처리
    void Detach(GameObject scoreZone);
    // 꽁초, 흡연자 scoreZone에 놔둔다. → 게임 매니저로 접근 → 점수, 콤보 증가→ UI UpdateScore 처리
}
