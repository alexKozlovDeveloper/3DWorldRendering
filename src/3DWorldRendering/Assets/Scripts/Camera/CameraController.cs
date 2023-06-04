using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public float panSpeed = 20f;
    public float rotationSpeed = 50.0f;
    public Vector3 offset = new Vector3(0f, 15f, -15f);
    public float zoomSpeed = 10.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 20.0f;
    private void Start()
    {
        //mainCamera = Camera.main;
    }

    private void Update()
    {
        // Перемещаем камеру при нажатии на клавиши WASD
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 translate = new Vector3(hAxis * panSpeed * Time.deltaTime, 0f, vAxis * panSpeed * Time.deltaTime);
        transform.Translate(translate, Space.World);

        // Зумируем камеру при помощи колесика мыши
        //float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
        //float zoom = mainCamera.fieldOfView - scrollAxis * zoomSpeed;
        //zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        //mainCamera.fieldOfView = zoom;

        // Устанавливаем камеру в изометрический вид
        //transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        //transform.position = Vector3.Lerp(transform.position, transform.TransformPoint(offset), Time.deltaTime * 10f);

        // Обработка вращения колесика мыши
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta != 0)
        {
            // Изменение зума камеры
            float newZoom = Mathf.Clamp(mainCamera.orthographicSize - scrollDelta * zoomSpeed, minZoom, maxZoom);
            //float newZoom = Mathf.Clamp(transform.localScale.x - scrollDelta * zoomSpeed, minZoom, maxZoom);
            //transform.localScale = new Vector3(newZoom, newZoom, newZoom);
            mainCamera.orthographicSize = newZoom;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
