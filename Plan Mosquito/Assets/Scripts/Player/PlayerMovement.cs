using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    public float _speed;

    [Header("Límites")]
    public float topLimit = 4f;
    public float bottomLimit = -4f;

    private float _inputY;

    // Update is called once per frame
    void Update()
    {
        float keyboardInput = UnityEngine.Input.GetAxisRaw("Vertical");
        float finalInput = keyboardInput != 0 ? keyboardInput : _inputY;

        Vector3 move = new Vector3(0, finalInput, 0) * _speed * Time.deltaTime;
        transform.Translate(move);

        // Limitar dentro de la pantalla
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, bottomLimit, topLimit);
        transform.position = pos;
    }


    //botones
    public void MoveUp()
    {
        _inputY = 1f;
        Debug.Log("Move up");
    }

    public void MoveDown()
    {
        _inputY = -1f;
        Debug.Log("Move down");
    }

    public void StopMove()
    {
        _inputY = 0f;
    }
}
