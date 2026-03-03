using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [SerializeField] private float radialLimit; //Limite en el que se mueve el joystick

    [SerializeField] RectTransform joystick; //Lo vamos a estar moviendo
    [SerializeField] RectTransform center; //Para saber cual es el centro

    [SerializeField] private float resetTime; //Cuando

    [SerializeField] private Vector2 axis; //Este es el

    private Touch currentTouch; //Aqui se almacena la
    // touch se encuentra el jugador (Down,Up,Drag)
    private Vector3 firstPos;
    private int touchState;

    private RectTransform margin;


    private void Start()
    {
        margin = GetComponent<RectTransform>();
        firstPos = center.transform.position;
    }


    private void Update()
    {

        //Este switch es para acceder al estado del touch en la pantalla

        switch ()
        {
            case TouchPhase.Began:
                {
                    currentTouch.phase = TouchPhase.Began;
                    break;
                }

            case TouchPhase.Moved:
                {
                    currentTouch.phase = TouchPhase.Moved;
                    break;
                }

            case TouchPhase.Ended:
                {
                    currentTouch.phase = TouchPhase.Ended;
                    break;
                }
        }

        //////////////////////////////////////////////////////////////////////
        ///
        //Este switch es para definir que va a pasar en cada estado del touch

        switch (currentTouch.phase)
        {
            case TouchPhase.Began:
                {
                    break;
                }

            case TouchPhase.Moved:
                {
                    break;
                }
            case TouchPhase.Ended:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }

    }

    //Cuando nosotros presionamos la pantalla TouchState = 1

    public void OnPointerDown(PointerEventData data)
    {
        touchState = 1;
    }


}
