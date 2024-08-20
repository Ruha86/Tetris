using UnityEngine;

public class ShapePart : MonoBehaviour
{
    // Координаты ячейки (X, Y)
    public Vector2Int CellId;

    // Устанавливаем позицию ячейки
    public void SetPosition(Vector2 position)
    {
        // Делаем позицию равной заданной
        transform.position = position;
    }

    // Управляем активностью (видимостью) части фигуры
    public void SetActive(bool value)
    {
        // Устанавливаем переданное значение активности
        gameObject.SetActive(value);
    }
    // Получаем значение активности (видимости) части фигуры
    public bool GetActive()
    {
        // Возвращаем текущее значение активности
        return gameObject.activeSelf;
    }
}
