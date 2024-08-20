using UnityEngine;

public class ShapeTwoRotations : Shape
{
    // ‘лаг состо€ни€ поворота
    bool _rotated = false;

    // ѕереопределЄнный метод вращени€ фигуры
    public override void Rotate()
    {
        // ќпредел€ем множитель дл€ поворота в зависимости от текущего состо€ни€ поворота
        // ≈сли фигура уже повЄрнута, устанавливаем отрицательный множитель
        float rotateMultiplier = _rotated ? -1 : 1;

        // ѕолучаем позицию части, которую будем использовать дл€ поворота 
        Vector2 rotatePosition = Parts[0].transform.position;

        // ѕроходим по всем част€м фигуры
        for (int i = 0; i < Parts.Length; i++)
        {
            // ѕоворачиваем часть вокруг позиции, которую мы выбрали, чтобы изменить еЄ положение
            Parts[i].transform.RotateAround(rotatePosition, Vector3.forward, 90f * rotateMultiplier);

            // ѕоворачиваем часть на -90 градусов вокруг оси Z, чтобы еЄ спрайт всегда отображалс€ вертикально
            Parts[i].transform.Rotate(Vector3.forward, -90f * rotateMultiplier);
        }

        // ћен€ем значение флага дл€ следующего поворота
        _rotated = !_rotated;
    }
}
