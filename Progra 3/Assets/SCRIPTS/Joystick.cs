using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float radialLimit; // Limite en el que se mueve el joystick

    [SerializeField] RectTransform joystick; // Lo vamos a estar moviendo
    [SerializeField] RectTransform center; // Para saber cual es el centro

    [SerializeField] private float resetTime; // Cuando sueltas el joystick, el tiempo que tarda en regresar al centro

    [SerializeField] public Vector2 axis; // Este es el valor que los codigos que hagan uso del joystick van a leer

    private Touch currentTouch; // Aqui se almacenará la información sobre en que fase de touch se encuentra el jugador (Down, Up, Drag) y la posicion del dedo

    private Vector3 firstPos; // Se guarda la posicion inicial de todo el joystick en este caso la esquina inferior izquierda y derecha

    private int touchState; // Hice esta variable porque puedo tener diferentes touch states de cuando presiono algo, dejo de pulsar algo, arrastro algo, que si son

    private RectTransform margin; // El que me indique en que parte de la pantalla puedo usar el joystick

    private bool onUse = false; // Para saber si estamos pulsando

    private bool isDragging = false; // Para saber si estamos arrastrando el dedo

    private void Start()
    {
        margin = GetComponent<RectTransform>();
        firstPos = center.transform.position;
    }

    private void Update()
    {

        // Este Switch es para acceder al estado del touch en la pantalla
        if (touchState == 1)
        {
            foreach (Touch touch in Input.touches) // Por cada touch que este presionando la pantalla voy a
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(margin, touch.position))
                {
                    currentTouch = touch; // Este touch va a ser el que más esté tocando el area asignada de el joystick o el último en tocar
                    break;
                }
                else
                {
                    currentTouch = touch; // Si no encuentra uno dentro agarra el más cercano, pero no nos servirá de mucho mas que para saber que existe uno
                }
            }

            // Este switch es para definir que va a pasar en cada estado del touch
            // current touch nos da tanto el estado del touch, que si estamos arrastrando, que si lo levantamos, que si esta quieto
            // y tambien nos da la posicion de la pantalla donde se encuentra
            switch (currentTouch.phase)
            {

                case TouchPhase.Began:

                    if (!onUse)
                    {
                        OnTouchDown();
                    }

                    break;

                case TouchPhase.Moved:

                    if (!isDragging)
                    {
                        OnTouchDrag();
                    }

                    break;

                case TouchPhase.Ended:

                    OnTouchUp();
                    break;

                default:

                    Debug.Log($"Ups, la {currentTouch.phase} falló");
                    break;
            }

        }

        // Lerp te mueve un vector a otro punto a razon tiempo indicado
        if (!isDragging && !onUse)
        {
            joystick.position = Vector3.Lerp(joystick.position, center.position, resetTime * Time.deltaTime);
        }

    }

    public void OnTouchDown()
    {
        onUse = true;
        transform.position = currentTouch.position;
    }

    /// <summary>
    /// Saber hacia donde estoy moviendo el dedo.
    /// Que mi joystick siga esa posicion
    /// Si mi dedo se mueve más alla del joystick, este debe quedarse dentro de su rango limite, pero siguiendo la posicion de mi dedo
    /// Aqui voy a modificar mi Vector2 llamado axis, el cual le servirá al control de movimiento y cámara para moverse o rotar
    /// </summary>
    public void OnTouchDrag()
    {
        Vector2 inputVector; // Este vector es donde voy a hacer toda mi lógica, calculos, etc para mandarselos a mi vector 2 axis

        Vector3 touchPosition = currentTouch.position; // Esto me guarda la posicion de mi dedo

        // Estas 3 lineas mueven el joystick y lo limitan segun nuestro radio definido
        joystick.position = touchPosition; // Esto nos mueve el joystick siguiendo nuestro dedo
        Vector3 offset = joystick.position - center.position; // Esto nos da la distancia del centro al joystick en vectores
        // Clamp limita un valor segun lo que tu dictes
        joystick.localPosition = Vector3.ClampMagnitude(offset, radialLimit); // El clamp es limitar un valor entre 2 valores dados
        inputVector = new Vector2(joystick.position.x - center.position.x, joystick.position.y - center.position.y);
        inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;

        axis = inputVector;
    }

    /// <summary>
    /// Este metodo se va a usar para desactivar cambios y resetear posiciones que se hayan modificado
    /// </summary>
    public void OnTouchUp()
    {
        onUse = false;
        isDragging = false;
        transform.position = firstPos;
        axis = Vector3.zero;
        touchState = 0;
    }

    // Cuando nosotros presionemos la pantalla TouchState = 1
    public void OnPointerDown(PointerEventData data)
    {
        touchState = 1;
        
    }
}