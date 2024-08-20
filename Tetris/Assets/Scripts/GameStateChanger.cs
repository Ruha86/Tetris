using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    // ������ �������� ����
    public GameField GameField;

    // ������ �������� �����
    public ShapeMover ShapeMover;

    // ������ ��������� �����
    public ShapeSpawner ShapeSpawner;

    // ����� ���� 
    public GameObject GameScreen;

    // ����� ����� ����
    public GameObject GameEndScreen;

    private void Start()
    {
        // �������� ����� FirstStartGame();
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPositions();

        // �����: �������� ����� ������ ����
        StartGame();
    }

    public void SpawnNextShape()
    {
        // ������ ���������� nextShape, � ������� ���������� ��������� ������, ��������������� ShapeSpawner
        Shape nextShape = ShapeSpawner.SpawnNextShape();

        // ������������� ��������� ������ � ShapeMover, ������� �������� �� ����������� �����
        ShapeMover.SetTargetShape(nextShape);

        // �������� ������ � �������� ������� �� ������� ����
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - GameField.InvisibleYFieldSize + nextShape.ExtraSpawnYMove));
    }

    private void StartGame()
    {
        // ���������� ����� ������
        SpawnNextShape();

        // ������������� ����� ����
        SwitchScreens(true);

        // �������� �������� �����
        ShapeMover.SetActive(true);
    }

    public void EndGame()
    {
        // ������������� ����� ����� ����
        SwitchScreens(false);

        // ��������� �������� �����
        ShapeMover.SetActive(false);
    }

    private void SwitchScreens(bool isGame)
    {
        // ���������� ����� ����
        GameScreen.SetActive(isGame);

        // �������� ����� ���������� ����
        GameEndScreen.SetActive(!isGame);
    }

    public void RestartGame()
    {
        // ������ ����������� ������� �����
    }
}
