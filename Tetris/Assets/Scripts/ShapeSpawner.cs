using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    // Массив префабов фигур, пока пустой
    // Заполним его позже в инспекторе
    public Shape[] ShapePrefabs = new Shape[0];

    // Метод для появления случайной фигуры
    public Shape SpawnNextShape()
    {
        // Выбираем случайную фигуру из массива
        Shape randomPrefab = ShapePrefabs[Random.Range(0, ShapePrefabs.Length)];

        // Возвращаем объект этой фигуры
        return Instantiate(randomPrefab);
    }
}
