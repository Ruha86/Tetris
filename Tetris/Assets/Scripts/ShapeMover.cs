using System.Collections.Generic;
using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    // ������ �������� ����
    public GameField GameField;

    // ������� ������
    private Shape _targetShape;

    // �������� �������� ����
    public float MoveDownDelay = 0.8f;

    // ������ �������� ����
    private float _moveDownTimer = 0;

    // ������ ��������� ��������� ����
    public GameStateChanger GameStateChanger;

    // ���� ���������� ����
    private bool _isActive;

    // ������ ���� �����
    private List<Shape> _allShapes = new List<Shape>();

    public void MoveShape(Vector2Int deltaMove)
    {
        if (!CheckMovePossible(deltaMove))
        {
            return;
        }

        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // �����: ������ ������ �������� �������� ����� ����� MoveShapePart()
            MoveShapePart(_targetShape.Parts[i], deltaMove);
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        // �����: � ������� ���������� �������� _targetShape
        SetShapePartCellsEmpty(_targetShape, true);

        HorizontalMove();
        VerticalMove();
        Rotate();

        bool reachBottom = CheckBottom();
        bool reachOtherShape = CheckOtherShape();

        // �����: � ������� ���������� �������� _targetShape
        SetShapePartCellsEmpty(_targetShape, false);

        if (reachBottom || reachOtherShape)
        {
            // �����: ������ ������ �������� �������� ����� ����� EndMovement()
            EndMovement();
        }
    }

    private void HorizontalMove()
    {
        // ���� ���� ������ ������� ����� ��� A
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            // ���������� ������ �����
            MoveShape(Vector2Int.left);
        }
        // �����, ���� ���� ������ ������� ������ ��� D
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            // ���������� ������ ������
            MoveShape(Vector2Int.right);
        }
    }

    private void VerticalMove()
    {
        // ����������� ������ �� �������� ���������� �������
        _moveDownTimer += Time.deltaTime;

        // ���� ������ ���������� ������� ��� ���� ������ ������� ���� ��� S
        if (_moveDownTimer >= MoveDownDelay || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            // �������� ������
            _moveDownTimer = 0;

            // ���������� ������ ����
            MoveShape(Vector2Int.down);
        }
    }

    private bool CheckMovePossible(Vector2Int deltaMove)
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2Int newPartCellId = _targetShape.Parts[i].CellId + deltaMove;

            if (newPartCellId.x < 0 || newPartCellId.y < 0
                || newPartCellId.x >= GameField.FieldSize.x || newPartCellId.y >= GameField.FieldSize.y)
            {
                return false;
            }
            // �����: ���� ������ � ����� ������� ����� ������ ������ ������ �������
            else if (!GameField.GetCellEmpty(newPartCellId))
            {
                // ���������� false (������������� �����������)
                return false;
            }
        }
        return true;
    }

    private bool CheckBottom()
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���������, ��������� �� ������� ����� ������ �� ������ ������� �������� ���� (������ � �������� y, ������ 0)
            if (_targetShape.Parts[i].CellId.y == 0)
            {
                // ���� ���� �� ���� ����� ������ ��������� �� ������ ������� �������� ����, ���������� true
                return true;
            }
        }
        // ���� �� ���� ����� ������ �� ��������� �� ������ ������� �������� ����, ���������� false
        return false;
    }

    public void SetTargetShape(Shape targetShape)
    {
        _targetShape = targetShape;

        // �����: ���� ������ �� �������� ������� ������
        if (!_allShapes.Contains(targetShape))
        {
            // �����: ��������� � ����
            _allShapes.Add(targetShape);
        }
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            // �����: ������ ����� ������ startCellIds 
            // ����������� ��� �������� ������� ������� ����� ������
            Vector2Int[] startCellIds = _targetShape.GetPartCellIds();

            _targetShape.Rotate();
            UpdateByWalls();
            UpdateByBottom();

            // �����: �������� ���������� ������ � ������ ����
            bool shapeSetted = TrySetShapeInCells();

            // �����: ���� ������ �� ���������� ����������
            if (!shapeSetted)
            {
                // �����: ���������� � �� �������� �������
                MoveShapeToCellIds(_targetShape, startCellIds);
            }
        }
    }

    private bool TrySetShapeInCells()
    {
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            Vector2 shapePartPosition = _targetShape.Parts[i].transform.position;
            Vector2Int newPartCellId = GameField.GetNearestCellId(shapePartPosition);

            // �����: ���� ������, � ������� �� �������� ���������� ������, ��� ������
            if (!GameField.GetCellEmpty(newPartCellId))
            {
                // �����: ���������� false
                return false;
            }
            Vector2 newPartPosition = GameField.GetCellPosition(newPartCellId);
            _targetShape.Parts[i].CellId = newPartCellId;
            _targetShape.Parts[i].SetPosition(newPartPosition);
        }
        // �����: ���� ������ ����� � ��������� ������, ���������� true
        return true;
    }

    private void UpdateByWalls()
    {
        // ��������� ������� ������ ������������ ������ �����
        UpdateByWall(true);

        // ��������� ������� ������ ������������ ����� �����
        UpdateByWall(false);
    }

    private void UpdateByWall(bool right)
    {
        // �������� �� ���� ������ ������ �� i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���� ����� ������ ������� �� �����
            if (CheckWallOver(_targetShape.Parts[i], right))
            {
                // �������� �� ���� ������ ������ �� j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // ������� ����� ������ � ��������������� ������� (����� ��� ������) �� ���� ������
                    _targetShape.Parts[j].transform.position += (right ? -1 : 1) * Vector3.right * GameField.CellSize.x;
                }
            }
        }
    }

    private bool CheckWallOver(ShapePart part, bool right)
    {
        // ����� ������� ���������� �� �����
        float wallDistance = 0;

        // ���� ����������� ������ �����
        if (right)
        {
            // ��������� ���������� ����� �������� ����� ������ � ������ ������
            wallDistance = part.transform.position.x - (GameField.FirstCellPoint.position.x + (GameField.FieldSize.x - 1) * GameField.CellSize.x);

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance > 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ ������� �� �����
                return true;
            }
        }
        // �����, ���� ����������� ����� �����
        else
        {
            // ��������� ���������� ����� �������� ����� ������ � ����� ������
            wallDistance = part.transform.position.x - GameField.FirstCellPoint.position.x;

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance < 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ ������� �� �����
                return true;
            }
        }
        // ���������� false, ����� �� ���� ����� ������ �� ������� �� �����
        return false;
    }

    private float GetRoundedWallDistance(float distance)
    {
        // ����� ����� ��� ���������� �� ���� ������ ����� �������
        int roundValue = 100;

        // ��������� ���������� �� ���������� ���������� ������ ����� �������
        distance = Mathf.Round(distance * roundValue);

        // ���������� ���������� �������� ����������
        return distance;
    }

    private void UpdateByBottom()
    {
        // �������� �� ���� ������ ������ �� i
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ���� ����� ������ ������� �� ���
            if (CheckBottomOver(_targetShape.Parts[i]))
            {
                // �������� �� ���� ������ ������ �� j
                for (int j = 0; j < _targetShape.Parts.Length; j++)
                {
                    // ������� ����� ������ �� ���� ������ �����
                    _targetShape.Parts[j].transform.position += Vector3.up * GameField.CellSize.y;
                }
            }
        }
    }

    private bool CheckBottomOver(ShapePart part)
    {
        // ��������� ���������� ����� �������� ����� ������ � �����
        float wallDistance = part.transform.position.y - GameField.FirstCellPoint.position.y;

        // ��������� ���������� �� ���������� ������ �����
        wallDistance = GetRoundedWallDistance(wallDistance);

        // ���� ���������� �� ����� 0 � ������������
        if (wallDistance != 0 && wallDistance < 0)
        {
            // ���������� true, ����� ��������, ��� ����� ������ ������� �� ���
            return true;
        }
        // ���������� false, ����� �� ���� ����� ������ �� ������� �� ���
        return false;
    }

    // ��������� ������� � ������ �������
    private bool CheckOtherShape()
    {
        // �����: ������ ���������� ��� �������� ������� �����
        Vector2Int checkingCellId;

        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // �����: ��������� ����� ������, ������� ��������� ��� ������� ������ ������
            checkingCellId = _targetShape.Parts[i].CellId + Vector2Int.down;

            // �����: ���� ������ ��������� � �� ����������� ������� ������
            if (!GameField.GetCellEmpty(checkingCellId) && !_targetShape.CheckContainsCellId(checkingCellId))
            {
                // ���������� true
                return true;
            }
        }
        // ���������� false
        return false;
    }
    // ������ ��������� ������ ������� ������
    private void SetShapePartCellsEmpty(Shape shape, bool value)
    {
        // �����: �������� _targetShape �� shape
        for (int i = 0; i < shape.Parts.Length; i++)
        {
            // �����: �������� ����� SetShapePartCellEmpty() ��� ������ ����� ������
            SetShapePartCellEmpty(shape.Parts[i], value);
        }
    }

    // ���������� ������ �� ��������� ������� �����
    private void MoveShapeToCellIds(Shape shape, Vector2Int[] cellIds)
    {
        // �������� �� ���� ������ ������
        for (int i = 0; i < shape.Parts.Length; i++)
        {
            // ���������� i-��� ����� ������ �� ������� ������ � �������� i � ������� cellIds
            MoveShapePartToCellId(shape.Parts[i], cellIds[i]);
        }
    }
    // ���������� ����� ������ �� ��������� ������� ������
    private void MoveShapePartToCellId(ShapePart part, Vector2Int cellId)
    {
        // �������� ����� ������� ��� ����� ������ �� ������ �������� ������� ������
        Vector2 newPartPosition = GameField.GetCellPosition(cellId);

        // ����������� ����� ������ ����� ������� ������
        part.CellId = cellId;

        // ������������� ������� ����� ������ �� ������� ����
        part.SetPosition(newPartPosition);
    }

    public void SetActive(bool value)
    {
        // ����������� ���������� _isActive �������� value
        _isActive = value;
    }

    private bool CheckShapeTopOver()
    {
        // ��������� ������� ����� ������� ������ �� ������� ����
        float topCellYPosition = GameField.FirstCellPoint.position.y + (GameField.FieldSize.y - GameField.InvisibleYFieldSize - 2) * GameField.CellSize.y;

        // �������� �� ���� ������ ������
        for (int i = 0; i < _targetShape.Parts.Length; i++)
        {
            // ��������� ���������� ����� �������� ����� ������ � ��������
            float wallDistance = _targetShape.Parts[i].transform.position.y - topCellYPosition;

            // ��������� ���������� �� ���������� ������ �����
            wallDistance = GetRoundedWallDistance(wallDistance);

            // ���� ���������� �� ����� 0 � ������������
            if (wallDistance != 0 && wallDistance > 0)
            {
                // ���������� true, ����� ��������, ��� ����� ������ �������� �������
                return true;
            }
        }
        // ���������� false, ����� �� ���� ����� ������ �� �������� �������
        return false;
    }

    private void MoveShapePart(ShapePart part, Vector2Int deltaMove)
    {
        // ��������� ����� ����� ������ ��� ������� ����� ������
        Vector2Int newPartCellId = part.CellId + deltaMove;

        // ���������� ����� ������ �� ����� �������
        MoveShapePartToCellId(part, newPartCellId);
    }

    private void SetShapePartCellEmpty(ShapePart part, bool value)
    {
        // ���� ����� ������ �� ������� (�� �����)
        if (!part.GetActive())
        {
            // ������� �� ������
            return;
        }
        // ������������� ��������� ������ (������ ��� ���) ��� �������� ����� ������
        GameField.SetCellEmpty(part.CellId, value);
    }

    private void TryRemoveFilledRows()
    {
        // �������� ������, ������� ���������, ��������� �� ������ ������ �������� ����
        bool[] rowFillings = GameField.GetRowFillings();

        // �������� �� ������ ������, ������� � ���������
        for (int i = rowFillings.Length - 1; i >= 0; i--)
        {
            // ���� ������ ���������
            if (rowFillings[i])
            {
                // ������� �
                RemoveRow(i);
            }
        }
    }

    private void RemoveRow(int id)
    {
        // ������ ���������� ��� �������� ������ �����
        int checkingRowId;

        // ������ ���������� ��� ������� ������
        Shape shape;

        // ������ ���������� ��� ������� ����� ������
        ShapePart part;

        // �������� �� ������ ������ �� �������� �������� ���� �� ���������� ����
        for (int i = 0; i < GameField.FieldSize.y - GameField.InvisibleYFieldSize; i++)
        {
            // ������������� ����������� ����� ������
            checkingRowId = i;

            // �������� �� ������ ������ � ������ ���� �����
            for (int j = 0; j < _allShapes.Count; j++)
            {
                // �������� ������� ������
                shape = _allShapes[j];

                // �������� �� ������ ����� ������
                for (int k = 0; k < shape.Parts.Length; k++)
                {
                    // �������� ������� ����� ������
                    part = shape.Parts[k];

                    // ���� ������� ����� ������ �� ��������� � ����������� ������ ��� �� �������
                    if (part.CellId.y != checkingRowId || !part.GetActive())
                    {
                        // ��������� � ��������� ����� � ����� � ������� ����������� ������� continue
                        continue;
                    }
                    // ���� ����� ������ ������� ����� ������ ������, ��� ����� ��������� ������
                    if (part.CellId.y > id)
                    {
                        // ������������� ��������� ������ ������� ����� ������ ��� ������
                        SetShapePartCellEmpty(part, true);

                        // ���������� ������� ����� ������ ����
                        MoveShapePart(part, Vector2Int.down);

                        // ������������� ��������� ������ ������� ����� ������ ��� �����������
                        SetShapePartCellEmpty(part, false);
                    }
                    // �����, ���� ����� ������ ������� ����� ������ ����� ������ ��������� ������
                    else if (part.CellId.y == id)
                    {
                        // ������������� ��������� ������ ������� ����� ������ ��� ������
                        SetShapePartCellEmpty(part, true);

                        // ������� ������� ����� ������
                        shape.RemovePart(part);

                        // ���� ������ ������ �� �����
                        if (shape.CheckNeedDestroy())
                        {
                            // ������� ������ �� ������ ���� �����
                            _allShapes.Remove(shape);

                            // ���������� ������ ������
                            Destroy(shape.gameObject);

                            // ��������� ������ ����� �� �������
                            // ����� �� ���������� ��������� ������ � ������ ����� �������� �������
                            j--;
                        }
                    }
                }
            }
        }
    }

    private void EndMovement()
    {
        // ���� ������ �������� �������
        if (CheckShapeTopOver())
        {
            // ��������� ����
            GameStateChanger.EndGame();
        }
        // �����
        else
        {
            // ������� ������� ����������� ������
            TryRemoveFilledRows();

            // ��������� ����� ������
            GameStateChanger.SpawnNextShape();
        }
    }
}
