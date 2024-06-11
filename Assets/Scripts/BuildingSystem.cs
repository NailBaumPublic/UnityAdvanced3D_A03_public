using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    // 현재 위치와 회전을 저장하는 변수
    private Vector3 currentPosition;
    private Vector3 currentRotation;

    // 현재 프리뷰 오브젝트의 Transform
    public Transform currentPreview;

    // 카메라의 Transform
    public Transform cam;

    // RaycastHit 정보를 저장
    public RaycastHit hit;

    // 레이어 마스크 (Raycast와 충돌할 레이어 지정)
    public LayerMask layerMask;

    // 오프셋 및 그리드 크기
    public float offset = 1.0f;
    public float gridSize = 1.0f;

    // 건설 모드 활성화 여부
    public bool isBuilding;

    // 히트된 면의 방향 저장
    public MCFace dir;

    // UI 인벤토리와 선택된 아이템
    public UIInventory inventory;
    public ItemData selectedItem;

    private void Update()
    {
        if (isBuilding)
        {
            // 현재 프리뷰 오브젝트가 없고 선택된 아이템이 있을 경우 새로운 프리뷰 오브젝트 생성
            if (currentPreview == null && selectedItem != null)
            {
                ChangeCurrentBuilding(selectedItem);
            }

            StartPreview();
            RotatePreview();
        }

        // 마우스 오른쪽 버튼 클릭 시 건설
        if (Input.GetMouseButtonDown(1) && currentPreview != null)
        {
            Build();
        }
    }

    // 현재 건설할 오브젝트 변경
    public void ChangeCurrentBuilding(ItemData item)
    {
        // 기존 프리뷰 오브젝트 삭제
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
        }

        // 새로운 프리뷰 오브젝트 생성
        GameObject curPrev = Instantiate(item.previewPrefab, currentPosition, Quaternion.Euler(currentRotation)) as GameObject;
        currentPreview = curPrev.transform;
    }

    // 프리뷰 시작
    public void StartPreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 3, layerMask))
        {
            if (hit.transform != this.transform)
            {
                ShowPreview(hit);
            }
        }
    }

    // 프리뷰 표시
    public void ShowPreview(RaycastHit hit2)
    {
        // 바닥 아이템일 경우 방향에 따라 위치 조정
        if (selectedItem.sort == ObjectSort.Floor)
        {
            dir = GetHitFace(hit2);
            if (dir == MCFace.Up || dir == MCFace.Down)
            {
                currentPosition = hit2.point;
            }
            else
            {
                if (dir == MCFace.North)
                {
                    currentPosition = hit2.point + new Vector3(0, 0, 0.5f);
                }
                if (dir == MCFace.South)
                {
                    currentPosition = hit2.point + new Vector3(0, 0, -0.5f);
                }
                if (dir == MCFace.East)
                {
                    currentPosition = hit2.point + new Vector3(0.5f, 0, 0);
                }
                if (dir == MCFace.West)
                {
                    currentPosition = hit2.point + new Vector3(-0.5f, 0, 0);
                }
            }
        }
        else
        {
            currentPosition = hit2.point;
        }

        // 그리드에 맞춰 위치 조정
        currentPosition -= Vector3.one * offset;
        currentPosition /= gridSize;
        currentPosition = new Vector3(Mathf.Round(currentPosition.x), Mathf.Round(currentPosition.y), Mathf.Round(currentPosition.z));
        currentPosition *= gridSize;
        currentPosition += Vector3.one * offset;

        // 프리뷰 오브젝트의 위치와 회전 적용
        currentPreview.position = currentPosition;
        currentPreview.rotation = Quaternion.Euler(currentRotation);
    }

    public void Build()
    {
        PreviewObject PO = currentPreview.GetComponent<PreviewObject>();
        if (PO != null && PO.isBuildable)
        {
            // 선택된 아이템의 건설 프리팹을 현재 위치에 생성
            Instantiate(selectedItem.buildPrefab, currentPosition, currentPreview.rotation);
            isBuilding = false;
            // 프리뷰 오브젝트 삭제
            Destroy(currentPreview.gameObject);
            currentPreview = null;
        }
    }

    public void CancelBuilding()
    {
        isBuilding = false;
        if (currentPreview != null)
        {
            Destroy(currentPreview.gameObject);
            currentPreview = null;
        }
        inventory.RestoreSelectedItem();
    }

    // 프리뷰 회전 함수
    public void RotatePreview()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput > 0)
        {
            currentRotation += new Vector3(0, 90, 0);
        }
        else if (scrollInput < 0)
        {
            currentRotation -= new Vector3(0, 90, 0);
        }

        // 프리뷰 오브젝트 회전 적용
        if (currentPreview != null)
        {
            currentPreview.rotation = Quaternion.Euler(currentRotation);
        }
    }

    // 히트된 면의 방향을 반환하는 함수
    public static MCFace GetHitFace(RaycastHit hit)
    {
        Vector3 incomingVec = hit.normal;

        if (incomingVec == new Vector3(0, 1, 0))
        {
            return MCFace.Up;
        }
        if (incomingVec == new Vector3(0, -1, 0))
        {
            return MCFace.Down;
        }
        if (incomingVec == new Vector3(0, 0, 1))
        {
            return MCFace.North;
        }
        if (incomingVec == new Vector3(0, 0, -1))
        {
            return MCFace.South;
        }
        if (incomingVec == new Vector3(1, 0, 0))
        {
            return MCFace.East;
        }
        if (incomingVec == new Vector3(-1, 0, 0))
        {
            return MCFace.West;
        }

        return MCFace.None;
    }

}

public enum MCFace
{
    None,
    Up,
    Down,
    East,
    West,
    North,
    South
}