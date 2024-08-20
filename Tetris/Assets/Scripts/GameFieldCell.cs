using UnityEngine;

public class GameFieldCell
{
    // Позиция ячейки на игровом поле
    private Vector2 _position;

    // Флаг пустоты ячейки
    private bool _isEmpty;

    // Создаём объект класса GameFieldCell
    public GameFieldCell(Vector2 position)
    {
        // Присваиваем позиции переданное значение
        _position = position;

        // Делаем ячейку изначально пустой
        _isEmpty = true;
    }
    // Получаем позицию ячейки
    public Vector2 GetPosition()
    {
        // Возвращаем значение _position
        return _position;
    }
    // Делаем ячейку пустой или заполненной
    public void SetIsEmpty(bool value)
    {
        // Устанавливаем флаг пустоты или заполненности
        _isEmpty = value;
    }
    // Узнаём, пуста ли ячейка
    public bool GetIsEmpty()
    {
        // Возвращаем значение _isEmpty
        return _isEmpty;
    }
}
