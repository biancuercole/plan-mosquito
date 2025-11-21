using UnityEngine;

public class Option : MonoBehaviour
{
    public string theme;
    public bool isCorrect;
    private Transform targetPoint;
    public Vector3 offset;

    public Menu menu;             // Referencia al script Menu

    private void Awake()
    {
        if (targetPoint == null)
            targetPoint = transform;
    }

    public void OnClickChoice()
    {
        if (isCorrect)
            menu.CorrectAnswer(targetPoint, offset, theme);
        else
            menu.WrongAnswer(targetPoint, offset, theme);
    }
}