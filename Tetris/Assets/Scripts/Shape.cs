using UnityEngine;

public class Shape : MonoBehaviour
{
    // ������ �������� ���� ShapePart � ��������� �������� 0
    public ShapePart[] Parts = new ShapePart[0];

    // ���������� ��� ��������������� �������� ���� �����
    // ������� � ����� �� �������� � ����������
    public int ExtraSpawnYMove;

    // ����������� ����� �������� ������
    public virtual void Rotate() { }

    public Vector2Int[] GetPartCellIds()
    {
        // ������ ����� ������ ���� Vector2Int � ��������, ������ ����� ������� ������ ������
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];

        // �������� �� ���� ������ ������
        for (int i = 0; i < Parts.Length; i++)
        {
            // ���������� � ������� startCellIds[i] �������� ������ ������ i-���� �������� ������� Parts
            startCellIds[i] = Parts[i].CellId;
        }

        // ���������� ������ startCellIds
        return startCellIds;
    }

    // ������� ����� ������
    public void RemovePart(ShapePart part)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < Parts.Length; i++)
        {
            // ���� ������� ����� ����� �����, ������� ����� �������
            if (Parts[i] == part)
            {
                // ������������� ���������� ����� � false (��� ���������� ���������)
                part.SetActive(false);
            }
        }
    }
    // ���������, ����� �� ������� ������
    public bool CheckNeedDestroy()
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < Parts.Length; i++)
        {
            // ���� ���������� ������� ����� ����� true (� �����)
            if (Parts[i].GetActive())
            {
                // ���������� false (������ �� ����� �������)
                return false;
            }
        }
        // ���� ��� ����� ������ �� ������, ���������� true
        // � ���� ������ ������ ����� �������
        return true;
    }
    // ���������, ���� �� � ������ ����� � �������� �������
    public bool CheckContainsCellId(Vector2Int cellId)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < Parts.Length; i++)
        {
            // ���� ����� ������� ����� ����� ����������
            if (Parts[i].CellId == cellId)
            {
                // ���������� true
                return true;
            }
        }
        // ���� �� ����� ����� �����, ���������� false
        return false;
    }
}
