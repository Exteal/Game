using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;


public class Ui_script : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] RectTransform CanvasTransform;
    [SerializeField] GraphicRaycaster Raycaster;

    [SerializeField] GameObject panel;

    List<GameObject> DragTargets = new();

    [SerializeField] GameObject passwordInput;
    [SerializeField] GameObject passwordManager;
    
    public void OnCursorInput(Vector2 InNormalizedPosition)
    {
        Vector3 InputPosition = new Vector3(CanvasTransform.sizeDelta.x * InNormalizedPosition.x,
                                            CanvasTransform.sizeDelta.y * InNormalizedPosition.y
                                            , 0);

        PointerEventData PointerEvent = new PointerEventData(EventSystem.current);

        PointerEvent.position = InputPosition;

        List<RaycastResult> Results = new();

        Raycaster.Raycast(PointerEvent, Results);

        bool bMouseDownThisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
        bool bMouseUpthisFrame = UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame;
        bool bIsMouseDown = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;

        if (bMouseUpthisFrame)
        {
            foreach (var target in DragTargets)
            {
                if (ExecuteEvents.Execute(target, PointerEvent, ExecuteEvents.endDragHandler))
                {
                    break;
                }
            }
            DragTargets.Clear();
        }

            

        foreach(var result in Results)
        {
            PointerEventData pointereventresult = new PointerEventData(EventSystem.current);
            pointereventresult.position = InputPosition;
            pointereventresult.pointerCurrentRaycast = result;
            pointereventresult.pointerPressRaycast = result;

            if (bIsMouseDown)
            {
                pointereventresult.button = PointerEventData.InputButton.Left;
            }

            var hitSlider = result.gameObject.GetComponentInParent<Slider>();

            if (bMouseDownThisFrame)
            {
                if (ExecuteEvents.Execute(result.gameObject, pointereventresult, ExecuteEvents.beginDragHandler))
                {
                    DragTargets.Add(result.gameObject);
                }

                if (hitSlider)
                {
                    hitSlider.OnInitializePotentialDrag(pointereventresult);

                    if(!DragTargets.Contains(result.gameObject))
                    {
                        DragTargets.Add(result.gameObject);
                    }
                }
            }

            else if (DragTargets.Contains(result.gameObject))
            {
                pointereventresult.dragging = true;
                ExecuteEvents.Execute(result.gameObject, pointereventresult, ExecuteEvents.dragHandler);
                
                if (hitSlider != null)
                {
                    hitSlider.OnDrag(pointereventresult);
                }
            }

            if (bMouseDownThisFrame)
            {
                if (ExecuteEvents.Execute(result.gameObject, pointereventresult, ExecuteEvents.pointerDownHandler))
                {
                    break;
                }
            }

            else if (bMouseUpthisFrame)
            {
                bool bDidRun = false;

                bDidRun |= ExecuteEvents.Execute(result.gameObject, pointereventresult, ExecuteEvents.pointerUpHandler);
                bDidRun |= ExecuteEvents.Execute(result.gameObject, pointereventresult, ExecuteEvents.pointerClickHandler);
                if (bDidRun)
                {
                    break;
                }

            }

        }

    }


    public void OnQuitButtonClick()
    {
        SceneManager.LoadScene("CharacterScene");
    }

    public void OnInputPasswordClick()
    {
        passwordInput.SetActive(!passwordInput.activeInHierarchy);
    }



    public void OnPasswordEdited(string text)
    {
        if (text == passwordManager.GetComponent<PasswordScript>().getPassword())
        {
            Debug.Log("Gagne");
            Application.Quit();
        }
    }
}
