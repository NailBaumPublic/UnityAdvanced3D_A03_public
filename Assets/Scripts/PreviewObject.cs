using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    // �浹ü ���
    public List<Collider> col = new List<Collider>();
    // ��ü�� ���� (�ٴ�, �� ��)
    public ObjectSort sort;
    // ���� ������ �� ����� ���� (�ʷϻ�)
    public Material green;
    // ���� �Ұ����� �� ����� ���� (������)
    public Material red;
    // ���� ���� ����
    public bool isBuildable;

    // �� ��° �̸����� ��ü���� ����
    public bool second;

    // �ڽ� �̸����� ��ü
    public PreviewObject childcol;

    // �׷����� ǥ���� Transform
    public Transform graphics;

    private void Update()
    {
        // �� ��° �̸����� ��ü�� �ƴϸ� ���� ����
        if (!second)
            ChangeColor();
    }

    // �ٸ� ��ü�� �浹 ���� �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        // ���̾ 10�� ���� ó��
        if (other.gameObject.layer == 10)
        {
            col.Add(other);
            UpdateBuildableState();
        }
    }

    // �ٸ� ��ü�� �浹 ���� �� ȣ��
    private void OnTriggerExit(Collider other)
    {
        // ���̾ 10�� ���� ó��
        if (other.gameObject.layer == 10)
        {
            col.Remove(other);
            UpdateBuildableState();
        }
    }

    // ���� ���� ���� ������Ʈ
    private void UpdateBuildableState()
    {
        if (sort == ObjectSort.Floor)
        {
            // �ٴ� ��ü�� �ٸ� ��ü�� ��� ���� ����
            isBuildable = col.Count == 0;
        }
        else
        {
            // �ٸ� ��ü�� �ڽ� �̸����� ��ü�� ������ ����
            isBuildable = col.Count == 0 && (childcol != null && childcol.col.Count > 0);
        }
    }

    // ���� ����
    public void ChangeColor()
    {
        // ���� ���� ���� ������Ʈ
        UpdateBuildableState();

        // ���� ������ ���� �ʷϻ�, �Ұ����� ���� ���������� ����
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

// ��ü ���� ������
public enum ObjectSort
{
    Normal,
    Floor,
    Wall,
    Roof,
    Stair
}
