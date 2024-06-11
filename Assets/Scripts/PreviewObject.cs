using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // 충돌체 목록
    public List<Collider> col = new List<Collider>();
    // 객체의 종류 (바닥, 벽 등)
    public ObjectSort sort;
    // 건축 가능할 때 사용할 재질 (초록색)
    public Material green;
    // 건축 불가능할 때 사용할 재질 (빨간색)
    public Material red;
    // 건축 가능 여부
    public bool isBuildable;

    // 두 번째 미리보기 객체인지 여부
    public bool second;

    // 자식 미리보기 객체
    public PreviewObject childcol;

    // 그래픽을 표시할 Transform
    public Transform graphics;

    private void Update()
    {
        // 두 번째 미리보기 객체가 아니면 색상 변경
        if (!second)
            ChangeColor();
    }

    // 다른 객체와 충돌 시작 시 호출
    private void OnTriggerEnter(Collider other)
    {
        // 레이어가 10일 때만 처리
        if (other.gameObject.layer == 10)
        {
            col.Add(other);
            UpdateBuildableState();
        }
    }

    // 다른 객체와 충돌 종료 시 호출
    private void OnTriggerExit(Collider other)
    {
        // 레이어가 10일 때만 처리
        if (other.gameObject.layer == 10)
        {
            col.Remove(other);
            UpdateBuildableState();
        }
    }

    // 건축 가능 여부 업데이트
    private void UpdateBuildableState()
    {
        if (sort == ObjectSort.Floor)
        {
            // 바닥 객체는 다른 객체가 없어도 건축 가능
            isBuildable = col.Count == 0;
        }
        else
        {
            // 다른 객체는 자식 미리보기 객체의 조건을 따름
            isBuildable = col.Count == 0 && (childcol != null && childcol.col.Count > 0);
        }
    }

    // 색상 변경
    public void ChangeColor()
    {
        // 건축 가능 여부 업데이트
        UpdateBuildableState();

        // 건축 가능할 때는 초록색, 불가능할 때는 빨간색으로 변경
        if (isBuildable)
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = green;
            }
        }
        else
        {
            foreach (Transform child in graphics)
            {
                child.GetComponent<Renderer>().material = red;
            }
        }
    }
}

// 객체 종류 열거형
public enum ObjectSort
{
    Normal,
    Floor,
    Wall,
    Roof,
    Stair
}
