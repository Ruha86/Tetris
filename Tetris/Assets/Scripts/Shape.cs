using UnityEngine;

public class Shape : MonoBehaviour
{
    // Массив объектов типа ShapePart с начальным размером 0
    public ShapePart[] Parts = new ShapePart[0];

    // Переменная для дополнительного смещения двух фигур
    // Объявим её позже на префабах в инспекторе
    public int ExtraSpawnYMove;

    // Виртуальный метод вращения фигуры
    public virtual void Rotate() { }

    public Vector2Int[] GetPartCellIds()
    {
        // Создаём новый массив типа Vector2Int с размером, равным длине массива частей фигуры
        Vector2Int[] startCellIds = new Vector2Int[Parts.Length];

        // Проходим по всем частям фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // Записываем в элемент startCellIds[i] значение номера ячейки i-того элемента массива Parts
            startCellIds[i] = Parts[i].CellId;
        }

        // Возвращаем массив startCellIds
        return startCellIds;
    }

    // Удаляем часть фигуры
    public void RemovePart(ShapePart part)
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // Если текущая часть равна части, которую нужно удалить
            if (Parts[i] == part)
            {
                // Устанавливаем активность части в false (она становится невидимой)
                part.SetActive(false);
            }
        }
    }
    // Проверяем, нужно ли удалить фигуру
    public bool CheckNeedDestroy()
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // Если активность текущей части равна true (её видно)
            if (Parts[i].GetActive())
            {
                // Возвращаем false (фигуру не нужно удалять)
                return false;
            }
        }
        // Если все части фигуры не видимы, возвращаем true
        // В этом случае фигуру нужно удалить
        return true;
    }
    // Проверяем, есть ли в фигуре часть с заданным номером
    public bool CheckContainsCellId(Vector2Int cellId)
    {
        // Проходим по всем частям фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // Если номер текущей части равен указанному
            if (Parts[i].CellId == cellId)
            {
                // Возвращаем true
                return true;
            }
        }
        // Если не нашли такой номер, возвращаем false
        return false;
    }
}
