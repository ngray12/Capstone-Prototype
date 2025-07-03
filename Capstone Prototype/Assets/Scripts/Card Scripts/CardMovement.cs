using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;

    
    [SerializeField]private int currentState = 0;

    private Quaternion originalRotation;
    private Vector3 originalPosition;


    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private float lerpFactor = 0.1f;

    [SerializeField] private Vector2 cardPlayLine;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;

    [SerializeField] private AudioClip cardFlick;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition;
        originalRotation = rectTransform.rotation;
    }

    private void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;

            case 3:
                HandlePlayState();
                if (!Input.GetMouseButton(0))
                {
                    TransitionToState0();
                }
                break;

        }
    }


    private void TransitionToState0()
    {
        currentState = 0;
        rectTransform.localPosition = originalPosition;
        rectTransform.localRotation = originalRotation;
        rectTransform.localScale = originalScale;
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.rotation;
            originalScale = rectTransform.localScale;

            currentState = 1;

            AudioManager.instance.PlaySoundFXClip(cardFlick, transform, 1f);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1) 
        {
            currentState = 2;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            Vector2 localPointerPosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out  localPointerPosition))
            {
                rectTransform.localPosition = Vector3.Lerp(rectTransform.position, Input.mousePosition,lerpFactor);

                if (rectTransform.localPosition.y > cardPlayLine.y) 
                {
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = Vector3.Lerp(rectTransform.position, playPosition, lerpFactor);
                }
            }
        }
    }
    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;
        rectTransform.localRotation= Quaternion.identity;
        
        if (Input.mousePosition.y < cardPlayLine.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }
}
