using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DyamicUiDisplayScreen : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] LayerMask rayCastMask = ~0;
    [SerializeField] float rayCastDistance = 5.0f;
    [SerializeField] UnityEvent<Vector2> OnCursorInput = new();

    // Update is called once per frame
    void Update()
    {
        

        Vector3 mousePosition = UnityEngine.InputSystem.Mouse.current.position.value;

        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        
        RaycastHit hit;

        if(Physics.Raycast(mouseRay, out hit, rayCastDistance, rayCastMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject != gameObject)
            {
                return;
            }

            OnCursorInput.Invoke(hit.textureCoord);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
