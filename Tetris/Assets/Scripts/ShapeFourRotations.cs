using UnityEngine;

public class ShapeFourRotations : Shape
{
    // ѕереопределЄнный метод вращени€ фигуры
    public override void Rotate()
    {
        // ѕолучаем позицию части, которую будем использовать как центр дл€ поворотов
        Vector2 rotatePosition = Parts[0].transform.position;

        // ѕроходим по всем част€м фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // ѕоворачиваем часть вокруг позиции, которую мы выбрали, чтобы изменить еЄ положение
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f);

            // ѕоворачиваем часть на -90 градусов вокруг оси Z, чтобы еЄ спрайт всегда отображалс€ вертикально
            Parts[i].transform.Rotate(Vector3.forward, -90f);
        }
    }
}
