using UnityEngine;

public class GameField : MonoBehaviour
{
    // Позиция первой ячейки
    public Transform FirstCellPoint;

    // Размер ячейки (по X и Y)
    public Vector2 CellSize;

    // Размер поля (по X и Y)
    public Vector2Int FieldSize;

    // двухмерный массив из позиций каждой ячейки
    private GameFieldCell[,] _cells;

    // Количество скрытых ячеек сверху поля
    public int InvisibleYFieldSize = 4;

    public void FillCellsPositions()
    {
        // Создаём двухмерный массив ячеек с заданными размерами
        _cells = new GameFieldCell[FieldSize.x, FieldSize.y];

        // Проходим по первым координатам всех ячеек (i)
        for (int i = 0; i < FieldSize.x; i++)
        {
            // Проходим по вторым координатам всех ячеек (j)
            for (int j = 0; j < FieldSize.y; j++)
            {
                // Вычисляем позицию ячейки на основе позиции первой ячейки, размеров ячейки и значений i, j
                Vector2 cellPosition = (Vector2)FirstCellPoint.position + Vector2.right * i * CellSize.x + Vector2.up * j * CellSize.y;

                // Создаём новую ячейку с вычисленной позицией
                GameFieldCell newCell = new GameFieldCell(cellPosition);

                // Записываем созданную ячейку в соответствующую позицию двухмерного массива ячеек
                _cells[i, j] = newCell;
            }
        }
    }

    public Vector2 GetCellPosition(Vector2Int cellId)
    {
        // Получаем ячейку по заданным координатам
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // Если такой ячейки нет
        if (cell == null)
        {
            // Возвращаем нулевые значения (0, 0)
            return Vector2.zero;
        }
        // Иначе возвращаем позицию ячейки
        return cell.GetPosition();
    }

    private GameFieldCell GetCell(int x, int y)
    {
        // Если номер ячейки некорректный
        if (x < 0 || y < 0 || x >= FieldSize.x || y >= FieldSize.y)
        {
            // Возвращаем пустое значение null
            return null;
        }
        // Иначе возвращаем ячейку с заданными координатами в двухмерном массиве ячеек
        return _cells[x, y];
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        // Получаем ячейку по её координатам
        GameFieldCell cell = GetCell(x, y);

        // Если такой ячейки нет
        if (cell == null)
        {
            // Возвращаем нулевые значения (0, 0)
            return Vector2.zero;
        }
        // Иначе возвращаем позицию ячейки
        return cell.GetPosition();
    }

    public Vector2Int GetNearestCellId(Vector2 position)
    {
        // Записываем в переменную resultDistance максимально возможное значение между проверяемой позицией и ячейкой поля
        // То есть мы находим ближайшую ячейку к заданной
        float resultDistance = float.MaxValue;

        // Записываем в переменные resultX и resultY нули
        int resultX = 0, resultY = 0;

        // Проходим по всем значениям X игрового поля
        for (int i = 0; i < FieldSize.x; i++)
        {
            // Проходим по всем значениям Y игрового поля
            for (int j = 0; j < FieldSize.y; j++)
            {
                // Получаем позицию ячейки с координатами i, j
                Vector2 cellPosition = GetCellPosition(i, j);

                // Вычисляем расстояние между текущей ячейкой и переданной позицией
                float distance = (cellPosition - position).magnitude;

                // Если текущее расстояние меньше resultDistance
                if (distance < resultDistance)
                {
                    // Записываем в resultDistance новое значение distance
                    resultDistance = distance;

                    // Записываем в resultX новое значение i
                    resultX = i;

                    // Записываем в resultY новое значение j
                    resultY = j;
                }
            }
        }
        // Возвращаем новый вектор Vector2Int, который означает номер заданной ячейки
        return new Vector2Int(resultX, resultY);
    }

    // Делаем ячейку пустой
    public void SetCellEmpty(Vector2Int cellId, bool value)
    {
        // Получаем ячейку по указанным координатам
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // Если такой ячейки нет
        if (cell == null)
        {
            // Выходим из метода
            return;
        }
        // Устанавливаем значение пустоты ячейки
        cell.SetIsEmpty(value);
    }
    // Получаем значение пустоты ячейки
    public bool GetCellEmpty(Vector2Int cellId)
    {
        // Получаем ячейку по указанным координатам
        GameFieldCell cell = GetCell(cellId.x, cellId.y);

        // Если такой ячейки нет
        if (cell == null)
        {
            // Возвращаем false
            return false;
        }
        // Возвращаем значение пустоты ячейки
        return cell.GetIsEmpty();
    }

    public bool[] GetRowFillings()
    {
        // Создаём массив значений заполненности строк
        // Размер массива = высота поля - невидимая область
        bool[] rowFillings = new bool[FieldSize.y - InvisibleYFieldSize];

        // Флаг заполненности текущей строки
        bool isRowFilled;

        // Проходим по всем строкам игрового поля
        for (int j = 0; j < rowFillings.Length; j++)
        {
            // Предполагаем, что текущая строка полностью заполнена
            isRowFilled = true;

            // Проходим по всем столбцам игрового поля
            for (int i = 0; i < FieldSize.x; i++)
            {
                // Если текущая ячейка пуста
                if (_cells[i, j].GetIsEmpty())
                {
                    // То текущая строка не заполнена
                    isRowFilled = false;

                    // Выходим из цикла
                    break;
                }
            }
            // Записываем значение заполненности текущей строки в соответствующий элемент массива
            rowFillings[j] = isRowFilled;
        }
        // Возвращаем массив значений заполненности строк
        return rowFillings;
    }
}
